namespace ProjectManagementService.Models;

using System.ComponentModel.DataAnnotations;
using Data;

public sealed class TaskModel : DbEntity
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    public DateTime CreateTime { get; set; } = DateTime.Now;

    public DateTime UpdateTime { get; set; } = DateTime.Now;

    public Guid CreatorId { get; set; }

    public Guid? PerformerId { get; set; }

    [MaxLength(10000)]
    public string Description { get; set; } = string.Empty;

    public DashboardModel Dashboard { get; set; }

    public TaskTypeModel Type { get; set; }

    public TaskStatusModel Status { get; set; }
}
