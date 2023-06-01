namespace ProjectManagementService.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data;

// split by models eg Task and TaskInfo

public sealed class Task : DbEntity
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [Required]
    [Column(TypeName = "timestamp without time zone")]
    public DateTime CreateTime { get; set; } = DateTime.Now;
    [Required]
    [Column(TypeName = "timestamp without time zone")]
    public DateTime UpdateTime { get; set; } = DateTime.Now;
    [Required]
    public Guid CreatorId { get; set; }

    public Guid? PerformerId { get; set; } = null!;

    [MaxLength(10000)]
    public string? Description { get; set; } = string.Empty;

    public TimeSpan? EstimationInTime { get; set; } = null!;
    public int? EstimationInPoints { get; set; } = null!;

    public UserStory? CorrespondingUserStory { get; set; } = null!;

    public Guid? SprintId { get; set; } = null!;

    [Required]
    public Guid ProjectId { get; set; }

    [Required]
    public TaskType Type { get; set; }

    [Required]
    public TaskState State { get; set; }

    
}
