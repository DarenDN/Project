namespace UserRelatedModelsLibrary.Models;

using System.ComponentModel.DataAnnotations;

// TODO change to Enum
public sealed class UserRole
{
    [Required]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(20)]
    public string RoleName { get; }
}
