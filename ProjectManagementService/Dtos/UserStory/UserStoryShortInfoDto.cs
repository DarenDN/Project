namespace ProjectManagementService.Dtos.UserStory;
using Enums;

public record UserStoryShortInfoDto(
    Guid Id,
    string Name,
    string? SprintName,
    ImportanceLevel ImportanceLevel,
    UserStoryState UserStoryState);
