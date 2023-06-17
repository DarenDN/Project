namespace MeetingService.Services.CacheService;

using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Threading.Tasks;
using Models;
using Dtos;
using Enums;
using System.Linq;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;
    private readonly DistributedCacheEntryOptions _distributedCacheEntryOptions = new DistributedCacheEntryOptions
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
        SlidingExpiration = TimeSpan.FromHours(1),
    };
    public RedisCacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<string?> GetMeetingCodeOrNullAsync(Guid projectId)
    {
        var meeting = await GetMeetingFromCacheAsync(projectId);
        return meeting?.MeetingCode;
    }

    public async Task<string> CreateCacheMeetingAsync(Guid projectId, Dictionary<Guid, BacklogType> tasks)
    {// TODO check if one already exists
        if(!string.IsNullOrWhiteSpace(await GetMeetingCodeOrNullAsync(projectId)))
        {
            throw new ArgumentException($"Key {nameof(projectId)} already exists");
        }

        var taskSelection = new TasksSelection
        {
            Code = Guid.NewGuid().ToString(),
            Backlog = tasks
        };

        var evaluation = new Evaluations
        {
            Code = Guid.NewGuid().ToString()
        };

        var meeting = new Meeting
        {
            MeetingCode = projectId.ToString(),
            TaskSelectionCode = taskSelection.Code,
            EvaluationsCode = evaluation.Code
        };

        var tasksSave = new Task[]
        {
            SaveMeetingChangesAsync(meeting.MeetingCode, meeting),
            SaveEvaluationsChangesAsync(evaluation.Code, evaluation),
            SaveTaskSelectionChangesAsync(taskSelection.Code, taskSelection)
        };
        var awaiter = Task.WhenAll(tasksSave);
        await awaiter;
        return meeting.MeetingCode;
    }

    public async Task DeleteCasheMeetingAsync(string meetingCode)
    {
        var meeting = await GetMeetingFromCacheAsync(meetingCode);
        var tasks = new Task[]
        {
             _distributedCache.RemoveAsync(meeting.EvaluationsCode),
             _distributedCache.RemoveAsync(meeting.TaskSelectionCode),
             _distributedCache.RemoveAsync(meeting.MeetingCode)
        };

        var awaiter = Task.WhenAll(tasks);
        await awaiter;
    }

    public async Task UpdateMeetingBacklogAsync(string meetingCode, Dictionary<Guid, BacklogType> tasks)
    {
        var meeting = await GetMeetingFromCacheAsync(meetingCode);
        var taskSelectionCode = meeting.TaskSelectionCode;
        var taskSelection = await GetTaskSelectionFromCacheAsync(taskSelectionCode);
        var backlog = taskSelection.Backlog.Keys;
        var addToBacklog = tasks.Where(t => !backlog.Contains(t.Key));
        foreach(var task in addToBacklog)
        {
            taskSelection.Backlog.Add(task.Key, task.Value);
        }

        await SaveTaskSelectionChangesAsync(taskSelectionCode, taskSelection);
    }

    public async Task AddCasheUserConnectionAsync(string meetingCode, string connectionId, Guid userId, string userName)
    {
        var meeting = await GetMeetingFromCacheAsync(meetingCode);
        var participant = meeting.Participants.FirstOrDefault(p => p.Id == userId);
        if (participant is not null)
        {
            if( string.Equals(connectionId, participant.ConnectionId))
            {
                return;
            }
            participant.ConnectionId = connectionId;
        }
        else
        {
            meeting.Participants.Add(new Participant { Id = userId, Name = userName, ConnectionId = connectionId });
        }

        await SaveMeetingChangesAsync(meeting.MeetingCode, meeting);
    }

    public async Task RemoveCasheUserConnectionAsync(string meetingCode, Guid userId)
    {
        var meeting = await GetMeetingFromCacheAsync(meetingCode);
        var participant = meeting.Participants.FirstOrDefault(p => p.Id == userId);
        if(participant is null)
        {
            return;
        }
        meeting.Participants.Remove(participant);
        await SaveMeetingChangesAsync(meeting.MeetingCode, meeting);
    }

    public async Task<IEnumerable<string>> GetCasheUserConnectionAsync(string meetingCode)
    {
        var meeting = await GetMeetingFromCacheAsync(meetingCode);
        return meeting == null
            ? Enumerable.Empty<string>()
            : meeting.Participants.Select(p => p.ConnectionId);
    }

    /// <summary>
    /// </summary>
    /// <param name="meetingCode"></param>
    /// <param name="taskId"></param>
    /// <returns></returns>
    public async Task<CurrentTaskStateDto> SetActiveTaskAsync(string meetingCode, Guid taskId)
    {
        var meeting = await GetMeetingFromCacheAsync(meetingCode);
        var taskSelectionCode = meeting.TaskSelectionCode;
        var evaluations = await GetEvaluationsFromCacheAsync(meeting.EvaluationsCode);
        var tasksSelection = await GetTaskSelectionFromCacheAsync(taskSelectionCode);
        tasksSelection.ActiveTask = taskId;
        await SaveTaskSelectionChangesAsync(taskSelectionCode, tasksSelection);

        return await GetCurrentTaskStateDtoAsync(taskId, meeting.MeetingCode, tasksSelection, evaluations);
    }

    public async Task SetEvaluationsOpenAsync(string meetingCode, Guid taskId)
    {
        var meeting = await GetMeetingFromCacheAsync(meetingCode);
        var taskSelectionCode = meeting.TaskSelectionCode;
        var selections = await GetTaskSelectionFromCacheAsync(taskSelectionCode);
        if (!selections.TasksEvaluationsOpen.ContainsKey(taskId))
        {
            selections.TasksEvaluationsOpen.Add(taskId, true);
        }
        else
        {
            selections.TasksEvaluationsOpen[taskId] = true;
        }

        await SaveTaskSelectionChangesAsync(taskSelectionCode, selections);
    }

    public async Task SetEvaluationAsync(string meetingCode, Guid userId, TaskEvaluationDto evaluationDto)
    {
        var meeting = await GetMeetingFromCacheAsync(meetingCode);
        var evaluations = await GetEvaluationsFromCacheAsync(meeting.EvaluationsCode);
        var selections = await GetTaskSelectionFromCacheAsync(meeting.TaskSelectionCode);
        if(!selections.Backlog.ContainsKey(evaluationDto.TaskId))
        {
            throw new ArgumentException($"Key {nameof(evaluationDto.TaskId)} does not exist in {nameof(selections.Backlog)}");
        }
        // obviously to user dictionary is better, but ... 
        if (evaluations.EvaluationsByParticipant?.TryGetValue(userId, out var taskEvaluations) ?? false)
        {
            var task = taskEvaluations.FirstOrDefault(e => e.Id == evaluationDto.TaskId);
            if (task is not null)
            {
                task.EvaluationPoints = evaluationDto.EvaluationPoints;
                task.EvaluationTime = evaluationDto.EvaluationTime;
            }
            else
            {
                taskEvaluations.Add(new TaskEvaluation
                {
                    Id = evaluationDto.TaskId,
                    EvaluationPoints = evaluationDto.EvaluationPoints,
                    EvaluationTime = evaluationDto.EvaluationTime
                });
            }
        }
        else
        {
            evaluations.EvaluationsByParticipant.Add(
                userId,
                new List<TaskEvaluation> { new TaskEvaluation
                {
                    Id = evaluationDto.TaskId,
                    EvaluationPoints = evaluationDto.EvaluationPoints,
                    EvaluationTime = evaluationDto.EvaluationTime
                } });
        }

        await SaveEvaluationsChangesAsync(meeting.EvaluationsCode, evaluations);
    }

    public async Task SetEvaluationFinalAsync(string meetingCode, TaskEvaluationDto evaluationDto)
    {
        var meeting = await GetMeetingFromCacheAsync(meetingCode);
        var evaluationsCode = meeting.EvaluationsCode;
        var taskId = evaluationDto.TaskId;
        var evaluations = await GetEvaluationsFromCacheAsync(evaluationsCode);
        if(evaluations.TaskFinalEvaluations.ContainsKey(taskId))
        {
            evaluations.TaskFinalEvaluations[taskId] = new TaskEvaluation
            {
                Id = taskId,
                EvaluationPoints = evaluationDto.EvaluationPoints,
                EvaluationTime = evaluationDto.EvaluationTime
            };
        }
        else
        {
            evaluations.TaskFinalEvaluations.Add(
            taskId,
            new TaskEvaluation
            {
                Id = taskId,
                EvaluationPoints = evaluationDto.EvaluationPoints,
                EvaluationTime = evaluationDto.EvaluationTime
            });
        }
        
        await SaveEvaluationsChangesAsync(evaluationsCode, evaluations);
    }

    public async Task<MeetingStateDto> GetCacheMeetingStateAsync(string meetingCode)
    {
        var meeting = await GetMeetingFromCacheAsync(meetingCode);
        var taskSelection = await GetTaskSelectionFromCacheAsync(meeting.TaskSelectionCode);
        var evaluations = await GetEvaluationsFromCacheAsync(meeting.EvaluationsCode);
        var finalEvaluations = evaluations.TaskFinalEvaluations;
        var activeTaskId = taskSelection.ActiveTask;
        var finalEvaluation = activeTaskId.HasValue
                && (evaluations.TaskFinalEvaluations?.ContainsKey(activeTaskId.Value) ?? false)
                ? evaluations.TaskFinalEvaluations[activeTaskId.Value]
                : null;

        var meetingStateDto = new MeetingStateDto(
            taskSelection.ActiveTask,
            finalEvaluation,
            taskSelection.Backlog.ToDictionary(
                    k=>k.Key, 
                    v=>new BacklogDto(v.Value, finalEvaluations.TryGetValue(v.Key, out var evaluation) && evaluation != null)),
            meeting.Participants.Select(p => new ParticipantDto(p.Id, p.Name)),
            evaluations.EvaluationsByParticipant?.ToDictionary(key => key.Key, value => value.Value?.FirstOrDefault(e=>e.Id == activeTaskId))
            );


        return meetingStateDto;
    }

    public async Task<CurrentTaskStateDto> DeleteTaskEvaluationsAsync(string meetingCode, Guid taskId)
    {
        var meeting = await GetMeetingFromCacheAsync(meetingCode);
        var evaluations = await GetEvaluationsFromCacheAsync(meeting.EvaluationsCode);
        var selections = await GetTaskSelectionFromCacheAsync(meeting.TaskSelectionCode);
        evaluations.TaskFinalEvaluations.Remove(taskId);
        evaluations.EvaluationsByParticipant.Clear();
        selections.TasksEvaluationsOpen.Remove(taskId);
        var tasks = new Task[]
        {
            SaveEvaluationsChangesAsync(meeting.EvaluationsCode, evaluations),
            SaveTaskSelectionChangesAsync(meeting.TaskSelectionCode, selections)
        };
        var awaiter = Task.WhenAll(tasks);
        await awaiter;

        return await GetCurrentTaskStateDtoAsync(taskId, meeting.MeetingCode, selections, evaluations);
    }

    public async Task<ParticipantEvaluationDto> GetUserCachedEvaluationsAsync(string meetingCode, Guid userId)
    {
        var meeting = await GetMeetingFromCacheAsync(meetingCode);
        var selection = await GetTaskSelectionFromCacheAsync(meeting.TaskSelectionCode);
        var evaluations = await GetEvaluationsFromCacheAsync(meeting.EvaluationsCode);
        var participant = meeting.Participants.FirstOrDefault(p => p.Id == userId);
        if (evaluations.EvaluationsByParticipant?.TryGetValue(userId, out var evaluationsByUser) ?? false)
        {
            var currentTaskEvaluation = evaluationsByUser.FirstOrDefault(e=>e.Id == selection.ActiveTask);
            var participantEvaluation = new ParticipantEvaluationDto(
                new ParticipantDto(participant.Id, participant.Name),
                currentTaskEvaluation == null 
                                    ? null
                                    : new EvaluationDto(currentTaskEvaluation.EvaluationPoints, currentTaskEvaluation.EvaluationTime));
            return participantEvaluation;
        }
        else
        {
            if(evaluations.EvaluationsByParticipant is null)
            {
                evaluations.EvaluationsByParticipant = new Dictionary<Guid, List<TaskEvaluation>>
                {
                    { userId, new List<TaskEvaluation>()}
                };
            }
            else
            {
                evaluations.EvaluationsByParticipant.Add(userId, new List<TaskEvaluation>());
            }

            await SaveEvaluationsChangesAsync(meeting.EvaluationsCode, evaluations);
        }
        
        return new ParticipantEvaluationDto(
            new ParticipantDto(participant.Id, participant.Name), 
            null);
    }

    public async Task ChangeTaskBacklogTypeAsync(string meetingCode, Guid taskId, BacklogType backlogType)
    {
        var meeting = await GetMeetingFromCacheAsync(meetingCode);
        var taskSelectionsCode = meeting.TaskSelectionCode;
        var selections = await GetTaskSelectionFromCacheAsync(taskSelectionsCode);
        if(selections.Backlog.ContainsKey(taskId))
        {
            selections.Backlog[taskId] = backlogType;
        }
        else
        {
            selections.Backlog.Add(taskId, backlogType);
        }
        await SaveTaskSelectionChangesAsync(taskSelectionsCode, selections);
    }

    private async Task<CurrentTaskStateDto> GetCurrentTaskStateDtoAsync(
        Guid taskId,
        string meetingCode,
        TasksSelection? tasksSelection = null,
        Evaluations? evaluations = null)
    {
        tasksSelection ??= await GetTaskSelectionFromCacheAsync(meetingCode);
        evaluations ??= await GetEvaluationsFromCacheAsync(meetingCode);

        var taskOpened = tasksSelection.TasksEvaluationsOpen?.TryGetValue(taskId, out var opened) ?? false
            ? opened
            : false;
        var evaluationDto = evaluations.TaskFinalEvaluations?.TryGetValue(taskId, out var evaluation) ?? false
            ? new EvaluationDto(evaluation.EvaluationPoints, evaluation.EvaluationTime)
            : null;
        var evaluationByUsers = evaluations.EvaluationsByParticipant
            ?.Select(evalsByUsers =>
                    new
                    {
                        User = evalsByUsers.Key,
                        Evaluation = evalsByUsers.Value.FirstOrDefault(e => e.Id == taskId)
                    })
            .ToDictionary(
                    key => key.User,
                    value => value.Evaluation == null
                                         ? null
                                         : new EvaluationDto(
                                             value.Evaluation.EvaluationPoints,
                                             value.Evaluation.EvaluationTime));
        var taskDto = new CurrentTaskStateDto(
            taskId,
            taskOpened,
            evaluationDto,
            evaluationByUsers);

        return taskDto;
    }

    private async Task<Meeting> GetMeetingFromCacheAsync(Guid projectId)
    {
        return await GetMeetingFromCacheAsync(projectId.ToString());
    }

    private async Task<Meeting> GetMeetingFromCacheAsync(string meetingCode)
    {
        var meetingString = await _distributedCache.GetStringAsync(meetingCode);
        if (string.IsNullOrWhiteSpace(meetingString))
        {
            return null;
        }
        var meeting = JsonSerializer.Deserialize<Meeting>(meetingString);
        return meeting;
    }

    private async Task<TasksSelection> GetTaskSelectionFromCacheAsync(string taskSelectionCode)
    {
        var taskSelectionString = await _distributedCache.GetStringAsync(taskSelectionCode);
        var taskSelection = JsonSerializer.Deserialize<TasksSelection>(taskSelectionString);
        return taskSelection;
    }

    private async Task<Evaluations> GetEvaluationsFromCacheAsync(string participantEvaluationCode)
    {
        var participantEvaluationString = await _distributedCache.GetStringAsync(participantEvaluationCode);
        var participantEvaluation = JsonSerializer.Deserialize<Evaluations>(participantEvaluationString);
        return participantEvaluation;
    }

    private async Task SaveMeetingChangesAsync(string meetingCode, Meeting meeting)
    {
        await _distributedCache.RefreshAsync(meetingCode);
        await _distributedCache.SetStringAsync(meetingCode, JsonSerializer.Serialize(meeting), _distributedCacheEntryOptions);
    }

    private async Task SaveTaskSelectionChangesAsync(string taskSelectionCode, TasksSelection taskSelection)
    {
        await _distributedCache.RefreshAsync(taskSelectionCode);
        await _distributedCache.SetStringAsync(taskSelectionCode, JsonSerializer.Serialize(taskSelection), _distributedCacheEntryOptions);
    }

    private async Task SaveEvaluationsChangesAsync(string evaluationCode, Evaluations evaluations)
    {
        await _distributedCache.RefreshAsync(evaluationCode);
        await _distributedCache.SetStringAsync(evaluationCode, JsonSerializer.Serialize(evaluations), _distributedCacheEntryOptions);
    }

    private async Task SaveChangesAsync<T>(string cacheCode, T cache)
    {
        await _distributedCache.RefreshAsync(cacheCode);
        await _distributedCache.SetStringAsync(cacheCode, JsonSerializer.Serialize<T>(cache), _distributedCacheEntryOptions);
    }

    public async Task<IEnumerable<BacklogTaskDto>> GetFinalEvaluationsAsync(string meetingCode)
    {
        var meeting = await GetMeetingFromCacheAsync(meetingCode);
        var selections = await GetTaskSelectionFromCacheAsync(meeting.TaskSelectionCode);
        var evaluations = (await GetEvaluationsFromCacheAsync(meeting.EvaluationsCode)).TaskFinalEvaluations;
        var taskSprintEvaluationInfos = selections.Backlog.Select(bl=> 
                new BacklogTaskDto(
                    bl.Key, 
                    evaluations.GetValueOrDefault(bl.Key)?.EvaluationTime, 
                    evaluations.GetValueOrDefault(bl.Key)?.EvaluationPoints, 
                    bl.Value));

        return taskSprintEvaluationInfos;
    }

    public async Task<string> CreateCacheMeetingAsync(Guid projectId, IEnumerable<BacklogTaskDto> tasks)
    {
        if (!string.IsNullOrWhiteSpace(await GetMeetingCodeOrNullAsync(projectId)))
        {
            throw new ArgumentException($"Key {nameof(projectId)} already exists");
        }

        var taskSelection = new TasksSelection
        {
            Code = Guid.NewGuid().ToString(),
            Backlog = tasks.Select(t=>new { t.TaskId, t.BacklogType }).ToDictionary(k => k.TaskId, v=>v.BacklogType)
        };

        var evaluation = new Evaluations
        {
            Code = Guid.NewGuid().ToString(),
            TaskFinalEvaluations = tasks
                .Where(t=>t.EstimationTime.HasValue || t.EstimationPoint.HasValue)
                .Select(t=>new {t.TaskId, t.EstimationPoint, t.EstimationTime})
                .ToDictionary(k=>k.TaskId, v=> new TaskEvaluation 
                    { 
                        Id = v.TaskId, 
                        EvaluationPoints = v.EstimationPoint, 
                        EvaluationTime = v.EstimationTime
                    })
        };

        var meeting = new Meeting
        {
            MeetingCode = projectId.ToString(),
            TaskSelectionCode = taskSelection.Code,
            EvaluationsCode = evaluation.Code
        };

        var tasksSave = new Task[]
        {
            SaveMeetingChangesAsync(meeting.MeetingCode, meeting),
            SaveEvaluationsChangesAsync(evaluation.Code, evaluation),
            SaveTaskSelectionChangesAsync(taskSelection.Code, taskSelection)
        };
        var awaiter = Task.WhenAll(tasksSave);
        await awaiter;
        return meeting.MeetingCode;
    }
}
