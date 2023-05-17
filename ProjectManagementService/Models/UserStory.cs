namespace ProjectManagementService.Models;
using System.ComponentModel.DataAnnotations;
using Data;
using Enums;

public sealed class UserStory : DbEntity
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string? Description { get; set; }

    [Required]
    public Guid ProjectId { get; set; }

    public Sprint? Sprint { get; set; } = null!;

    // TODO enum?
    [Required]
    public ImportanceLevel Importance { get; set; } = ImportanceLevel.Low;

    // TODO create a user story state ... enum?
    [Required]
    public UserStoryState State { get; set; } = UserStoryState.Idle;
}
