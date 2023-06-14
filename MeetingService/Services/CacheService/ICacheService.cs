namespace MeetingService.Services.CacheService;

using Dtos;
using Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICacheService
{
    Task AddCasheUserConnectionAsync(string meetingCode, string connectionId, Guid userId);
    Task<ParticipantEvaluationDto> GetUserCachedEvaluationsAsync(string meetingCode, Guid userId);
    Task<string> CreateCacheMeetingAsync(Guid projectId, Dictionary<Guid, BacklogType> tasks);
    Task<string> CreateCacheMeetingAsync(Guid projectId, IEnumerable<BacklogTaskDto> tasks);
    Task<IEnumerable<BacklogTaskDto>> GetFinalEvaluationsAsync(string meetingCode);
    Task DeleteCasheMeetingAsync(string meetingCode);
    Task DeleteTaskEvaluationsAsync(string meetingCode, Guid taskId);
    Task<MeetingStateDto> GetCacheMeetingStateAsync(string meetingCode);
    Task<IEnumerable<string>> GetCasheUserConnectionAsync(string meetingCode);
    Task<string?> GetMeetingCodeOrNullAsync(Guid projectId);
    Task RemoveCasheUserConnectionAsync(string meetingCode, Guid userId);
    Task<CurrentTaskStateDto> SetActiveTaskAsync(string meetingCode, Guid taskId);
    Task SetEvaluationAsync(string meetingCode, Guid userId, TaskEvaluationDto evaluationDto);
    Task SetEvaluationFinalAsync(string meetingCode, TaskEvaluationDto evaluationDto);
    Task SetEvaluationsOpenAsync(string meetingCode, Guid taskId);
    Task ChangeTaskBacklogTypeAsync(string meetingCode, Guid taskId, BacklogType backlogType);
    Task UpdateMeetingBacklogAsync(string meetingCode, Dictionary<Guid, BacklogType> tasks);
}