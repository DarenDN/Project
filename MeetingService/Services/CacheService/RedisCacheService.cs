namespace MeetingService.Services.CacheService;

using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Threading.Tasks;
using Models;
using Dtos;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;
    private readonly DistributedCacheEntryOptions _distributedCacheEntryOptions = new DistributedCacheEntryOptions
    {
        // TODO
    }; 
    public RedisCacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task CreateCacheMeetingAsync(string meetingCode, List<Guid?> tasks)
    {
        var taskSelection = new TasksSelection
        {
            Code = Guid.NewGuid().ToString(),
            ProjectBacklog = tasks.ToHashSet()
        };

        var evaluation = new Evaluations { Code = Guid.NewGuid().ToString() };
        var meeting = new Meeting
        {
            MeetingCode = meetingCode,
            TaskSelectionCode = taskSelection.Code,
            EvaluationsCode = evaluation.Code
        };
        var tasksSave = new Task[]
        {
            SaveMeetingChangesAsync(meetingCode, meeting),
            SaveEvaluationsChangesAsync(evaluation.Code, evaluation),
            SaveTaskSelectionChangesAsync(taskSelection.Code, taskSelection)
        };
        foreach (var taskSave in tasksSave)
        {
            taskSave.Start();
        }
        var awaiter = Task.WhenAll(tasksSave);
        await awaiter;
    }

    public async Task DeleteCasheMeetingAsync(string meetingCode)
    {
        var meeting = await GetMeetingFromCacheAsync(meetingCode);
        var tasks = new Task[]
        {
             _distributedCache.RemoveAsync(meeting.EvaluationsCode),
             _distributedCache.RemoveAsync(meeting.TaskSelectionCode),
             _distributedCache.RemoveAsync(meetingCode)
        };
        foreach (var task in tasks)
        {
            task.Start();
        }

        var awaiter = Task.WhenAll(tasks);
        await awaiter;
    }

    public async Task UpdateCacheMeetingBacklogAsync(string meetingCode, List<Guid?> tasks)
    {
        var meeting = await GetMeetingFromCacheAsync(meetingCode);
        var taskSelectionCode = meeting.TaskSelectionCode;
        var taskSelection = await GetTaskSelectionFromCacheAsync(taskSelectionCode);
        taskSelection.ProjectBacklog = tasks.ToHashSet();
        await SaveTaskSelectionChangesAsync(taskSelectionCode, taskSelection);
    }

    public async Task AddCasheUserConnectionAsync(string meetingCode, string connectionId, Guid userId)
    {
        var meeting = await GetMeetingFromCacheAsync(meetingCode);
        meeting.Participants.Add(userId, connectionId);
        await SaveMeetingChangesAsync(meetingCode, meeting);
    }

    public async Task RemoveCasheUserConnectionAsync(string meetingCode, Guid userId)
    {
        var meeting = await GetMeetingFromCacheAsync(meetingCode);
        meeting.Participants.Remove(userId);
        await SaveMeetingChangesAsync(meetingCode, meeting);
    }

    public async Task<IEnumerable<string>> GetCasheUserConnectionAsync(string meetingCode)
    {
        var meeting = await GetMeetingFromCacheAsync(meetingCode);
        return meeting == null
            ? Enumerable.Empty<string>()
            : meeting.Participants.Values;
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
        var taskOpened = tasksSelection.TasksOpened.TryGetValue(taskId, out var opened)
            ? opened
            : false;
        var evaluationDto = evaluations.TaskFinalEvaluations.TryGetValue(taskId, out var evaluation)
            ? new EvaluationDto(evaluation.EvaluationPoints, evaluation.EvaluationTime)
            : null;
        var evaluationByUsers = evaluations.EvaluationsByParticipant
            .Select(evalsByUsers =>
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

        return await GetCurrentTaskStateDtoAsync(taskId, meetingCode, tasksSelection, evaluations);
    }

    public async Task SetTaskOpenedAsync(string meetingCode, Guid taskId)
    {
        var meeting = await GetMeetingFromCacheAsync(meetingCode);
        var selections = await GetTaskSelectionFromCacheAsync(meeting.TaskSelectionCode);
        if (!selections.TasksOpened.ContainsKey(taskId))
        {
            selections.TasksOpened.Add(taskId, true);
            return;
        }

        selections.TasksOpened[taskId] = true;
    }

    public async Task<CurrentTaskStateDto> SetEvaluationAsync(string meetingCode, Guid userId, TaskEvaluationDto evaluationDto)
    {
        var meeting = await GetMeetingFromCacheAsync(meetingCode);
        var evaluations = await GetEvaluationsFromCacheAsync(meeting.EvaluationsCode);
        // obviously to user dictionary is better, but ... 
        if (evaluations.EvaluationsByParticipant.TryGetValue(userId, out var taskEvaluations))
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

        return await GetCurrentTaskStateDtoAsync(evaluationDto.TaskId, meetingCode, evaluations: evaluations);
    }

    public async Task<CurrentTaskStateDto> SetEvaluationFinalAsync(string meetingCode, TaskEvaluationDto evaluationDto)
    {
        var meeting = await GetMeetingFromCacheAsync(meetingCode);
        var evaluationsCode = meeting.EvaluationsCode;
        var taskId = evaluationDto.TaskId;
        var evaluations = await GetEvaluationsFromCacheAsync(evaluationsCode);
        evaluations.TaskFinalEvaluations.Add(
            taskId, 
            new TaskEvaluation 
            { 
                Id = taskId,
                EvaluationPoints = evaluationDto.EvaluationPoints,
                EvaluationTime = evaluationDto.EvaluationTime
            });
        await SaveEvaluationsChangesAsync(evaluationsCode, evaluations);
        return await GetCurrentTaskStateDtoAsync(taskId, meetingCode, evaluations: evaluations);
    }

    public async Task<MeetingStateDto> GetCacheMeetingStateAsync(string meetingCode)
    {
        var meeting = await GetMeetingFromCacheAsync(meetingCode);
        var taskSelection = await GetTaskSelectionFromCacheAsync(meeting.TaskSelectionCode);
        var evaluations = await GetEvaluationsFromCacheAsync(meeting.EvaluationsCode);
        var meetingStateDto = new MeetingStateDto(
            taskSelection.ActiveTask,
            taskSelection.SprintBacklog,
            taskSelection.ProjectBacklog,
            meeting.Participants.Keys,
            evaluations.TaskFinalEvaluations,
            evaluations.EvaluationsByParticipant
            );
        return meetingStateDto;
    }

    public async Task DeleteTaskEvaluationsAsync(string meetingCode, Guid taskId)
    {
        var meeting = await GetMeetingFromCacheAsync(meetingCode);
        var evaluation = await GetEvaluationsFromCacheAsync(meeting.EvaluationsCode);
        var selection = await GetTaskSelectionFromCacheAsync(meeting.TaskSelectionCode);
        evaluation.TaskFinalEvaluations.Remove(taskId);
        evaluation.EvaluationsByParticipant.Clear();
        selection.TasksOpened.Remove(taskId);
        var tasks = new Task[]
        {
            SaveEvaluationsChangesAsync(meeting.EvaluationsCode, evaluation),
            SaveTaskSelectionChangesAsync(meeting.TaskSelectionCode, selection)
        };
        foreach (var task in tasks)
        {
            task.Start();
        }
        var awaiter = Task.WhenAll(tasks);
        await awaiter;
    }

    private async Task<CurrentTaskStateDto> GetCurrentTaskStateDtoAsync(
        Guid taskId,
        string meetingCode,
        TasksSelection? tasksSelection = null,
        Evaluations? evaluations = null)
    {
        tasksSelection ??= await GetTaskSelectionFromCacheAsync(meetingCode);
        evaluations ??= await GetEvaluationsFromCacheAsync(meetingCode);

        var taskOpened = tasksSelection.TasksOpened.TryGetValue(taskId, out var opened)
            ? opened
            : false;
        var evaluationDto = evaluations.TaskFinalEvaluations.TryGetValue(taskId, out var evaluation)
            ? new EvaluationDto(evaluation.EvaluationPoints, evaluation.EvaluationTime)
            : null;
        var evaluationByUsers = evaluations.EvaluationsByParticipant
            .Select(evalsByUsers =>
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

    private async Task<Meeting> GetMeetingFromCacheAsync(string meetingCode)
    {
        var meetingString = await _distributedCache.GetStringAsync(meetingCode);
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
        await _distributedCache.SetStringAsync(meetingCode, JsonSerializer.Serialize(meeting), _distributedCacheEntryOptions);
    }

    private async Task SaveTaskSelectionChangesAsync(string taskSelectionCode, TasksSelection taskSelection)
    {
        await _distributedCache.SetStringAsync(taskSelectionCode, JsonSerializer.Serialize(taskSelection), _distributedCacheEntryOptions);
    }

    private async Task SaveEvaluationsChangesAsync(string evaluationCode, Evaluations evaluations)
    {
        await _distributedCache.SetStringAsync(evaluationCode, JsonSerializer.Serialize(evaluations), _distributedCacheEntryOptions);
    }
}
