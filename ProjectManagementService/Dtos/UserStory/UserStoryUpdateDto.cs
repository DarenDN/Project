
namespace ProjectManagementService.Dtos.UserStory;
using Enums;

public record UserStoryUpdateDto(
    Guid Id,
    string Name,
    string Description,
    ImportanceLevel ImportanceLevel,
    UserStoryState UserStoryState);
