// TODO incapsulate namespaces
namespace ProjectManagementService.Models;
using System.ComponentModel.DataAnnotations;
using Data;

internal sealed class DashboardModel : DbEntity
{
    [Required]
    [MaxLength(40)]
    public string Title { get; set; }

    public ProjectModel Project { get; set; }

    public List<TaskModel> Tasks { get; set; } = new List<TaskModel>();
}
