namespace MeetingService.Services.Interfaces;
using Models;

public interface ICacheService
{
    // TODO what type of key?
    Task<Meeting> GetAsync(Guid projectId);
    Task<bool> UpdateAsync(Meeting meeting, Guid projectId);
    Task<bool> DeleteAsync(Guid projectId);
    Task<bool> SaveAsync(Meeting meeting, Guid projectId);
}
