namespace MeetingService.Services.Implementations;

using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Threading.Tasks;
using Interfaces;
using Models;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;

    public RedisCacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }


    public async Task<bool> DeleteAsync(Guid projectId)
    {
        // TODO try catch this bad boy coz could be deleted already
        await _distributedCache.RemoveAsync(projectId.ToString());
        return true;
    }

    public async Task<Meeting> GetAsync(Guid projectId)
    {
        var storedMeeting = await _distributedCache.GetStringAsync(projectId.ToString());
        if(string.IsNullOrWhiteSpace(storedMeeting))
        {
            throw new Exception("Meeting does not found");
        }

        var deserializedMeeting = JsonSerializer.Deserialize<Meeting>(storedMeeting);

        if(deserializedMeeting is null)
        {
            // TODO smt
        }

        return deserializedMeeting;
    }

    public async Task<bool> UpdateAsync(Meeting meeting, Guid projectId)
    {
        // TODO update logic

        // TODO try catch coz instance could be deleted 
        await _distributedCache.RefreshAsync(projectId.ToString());
        return true;
    }

    public async Task<bool> SaveAsync(Meeting meeting, Guid projectId)
    {
        // TODO cashe expiration time need to be set somewhere
        // Config?
        
        var options = new DistributedCacheEntryOptions()
        {
            // TODO expiration time
        };

        var jsonMeetingData = JsonSerializer.Serialize(meeting);

        await _distributedCache.SetStringAsync(projectId.ToString(), jsonMeetingData, options);

        return true;
    }
}
