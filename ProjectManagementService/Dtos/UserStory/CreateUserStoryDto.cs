using ProjectManagementService.Enums;

namespace ProjectManagementService.Dtos.UserStory;

public record CreateUserStoryDto(
    string Name, 
    string? Description,
    ImportanceLevel Importance);
