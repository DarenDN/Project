namespace ProjectManagementService.Services.ProductBacklog;

using Dtos.UserStory;
using Microsoft.EntityFrameworkCore;
using ProjectManagementService.Data;
using ProjectManagementService.Models;
using System.Security.Claims;
using Task = System.Threading.Tasks.Task;

public class UserStoryService : IUserStoryService
{
    private readonly ApplicationDbContext _appDbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserStoryService(ApplicationDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
    {
        this._appDbContext = appDbContext;
        this._httpContextAccessor = httpContextAccessor;
    }

    public async Task CreateUserStoryAsync(CreateUserStoryDto userStoryDto)
    {
        var projectId = await GetRequestingUsersProjectIdAsync();
        if (projectId is null)
        {
            throw new ArgumentNullException(nameof(projectId));
        }

        var story = new UserStory
        {
            Name = userStoryDto.Name,
            Description = userStoryDto.Description,
            Importance = userStoryDto.Importance,
            ProjectId = projectId.Value
        };

        _appDbContext.UserStories.Add(story);

        await TrySaveChangesAsync();
    }

    public async Task DeleteUserStoryAsync(Guid storyId)
    {
        var story = await _appDbContext.UserStories.FirstOrDefaultAsync(s => s.Id == storyId);
        _appDbContext.UserStories.Remove(story);
        await TrySaveChangesAsync();
    }

    public async Task<IEnumerable<UserStoryShortInfoDto>> GetUserStoriesAsync()
    {
        var projectId = await GetRequestingUsersProjectIdAsync();
        var stories = _appDbContext.UserStories
            .Where(s => s.ProjectId == projectId.Value)
            .Include(s => s.Sprint)
            .Select(s => new UserStoryShortInfoDto(s.Id, s.Name, s.Sprint.Name, s.Importance, s.State));

        return stories;
    }

    public async Task<UserStoryDto> GetUserStoryAsync(Guid storyId)
    {
        var story = await _appDbContext.UserStories
            .Include(s => s.Sprint)
            .FirstOrDefaultAsync(s => s.Id == storyId);

        return new UserStoryDto(
            story.Id, 
            story.Name, 
            story.Description, 
            story.Sprint.Name, 
            story.Importance, 
            story.State);
    }

    // TODO split by methods
    public async Task UpdateUserStoryAsync(UserStoryUpdateDto userStoryDto)
    {
        var story = await _appDbContext.UserStories
            .Include(s => s.Sprint)
            .FirstOrDefaultAsync(s => s.Id == userStoryDto.Id);

        story.Name = userStoryDto.Name;
        story.Description = userStoryDto.Description;
        story.State = userStoryDto.UserStoryState;
        story.Importance = userStoryDto.ImportanceLevel;


    }

    private async Task TrySaveChangesAsync()
    {
        try
        {
            await _appDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async Task<Guid?> GetRequestingUsersProjectIdAsync()
    {
        var identityId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(identityId))
        {
            return null;
        }

        var projectsIdentity = await _appDbContext.ProjectsIdentities.FirstOrDefaultAsync(pi => pi.IdentityId == Guid.Parse(identityId));

        return projectsIdentity?.ProjectId;
    }
}
