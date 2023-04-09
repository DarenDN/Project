namespace ProjectManagementService.Models;

using System.ComponentModel.DataAnnotations;
using Data;

public sealed class Task : DbEntity
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    public DateTime CreateTime { get; set; } = DateTime.Now;

    public DateTime UpdateTime { get; set; } = DateTime.Now;

    public User CreatorId { get; set; }

    public User? PerformerId { get; set; }

    [MaxLength(10000)]
    public string Description { get; set; } = string.Empty;

    public Dashboard Dashboard { get; set; }

    public TaskType Type { get; set; }

    public TaskStatus Status { get; set; }
}
