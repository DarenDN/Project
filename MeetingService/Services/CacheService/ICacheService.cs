namespace MeetingService.Services.CacheService;
using Models;

public interface ICacheService
{
    // TODO what type of key?
    Task<ParticipantEvaluation> GetAsync(Guid projectId);
    Task<bool> UpdateAsync(ParticipantEvaluation meeting, Guid projectId);
    Task<bool> DeleteAsync(Guid projectId);
    Task<bool> SaveAsync(ParticipantEvaluation meeting, Guid projectId);
}
