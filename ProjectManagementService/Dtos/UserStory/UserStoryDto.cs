using ProjectManagementService.Enums;

namespace ProjectManagementService.Dtos.UserStory;

public sealed record UserStoryDto(
    Guid Id,
    string Name,
    string Description,
    string? SprintName,
    ImportanceLevel ImportanceLevel,
    UserStoryState UserStoryState);
