namespace MeetingService.Services.CacheService;

using global::MeetingService.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICacheService
{
    Task AddCasheUserConnectionAsync(string meetingCode, string connectionId, Guid userId);
    Task CreateCacheMeetingAsync(string meetingCode, List<Guid?> tasks);
    Task DeleteCasheMeetingAsync(string meetingCode);
    Task DeleteTaskEvaluationsAsync(string meetingCode, Guid taskId);
    Task<Dtos.MeetingStateDto> GetCacheMeetingStateAsync(string meetingCode);
    Task<IEnumerable<string>> GetCasheUserConnectionAsync(string meetingCode);
    Task RemoveCasheUserConnectionAsync(string meetingCode, Guid userId);
    Task<Dtos.CurrentTaskStateDto?> SetActiveTaskAsync(string meetingCode, Guid taskId);
    Task<CurrentTaskStateDto> SetEvaluationAsync(string meetingCode, Guid userId, TaskEvaluationDto evaluationDto);
    Task<CurrentTaskStateDto> SetEvaluationFinalAsync(string meetingCode, TaskEvaluationDto evaluationDto);
    Task SetTaskOpenedAsync(string meetingCode, Guid taskId);
    Task UpdateCacheMeetingBacklogAsync(string meetingCode, List<Guid?> tasks);
}