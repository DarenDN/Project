namespace ProjectManagementService.Services.ProductBacklog;

using System.Threading.Tasks;
using Dtos.UserStory;

public interface IUserStoryService
{
    Task CreateUserStoryAsync(CreateUserStoryDto userStoryDto);
    Task<UserStoryDto> GetUserStoryAsync(Guid storyId);
    Task<IEnumerable<UserStoryShortInfoDto>> GetUserStoriesAsync();
    Task DeleteUserStoryAsync(Guid storyId);
    Task UpdateUserStoryAsync(UserStoryUpdateDto userStoryDto);
    // TODO set sprint method 

}
