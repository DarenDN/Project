// TODO incapsulate namespaces
namespace ProjectManagementService.Models;

using System.ComponentModel.DataAnnotations;
using Data;
using ProjectManagementService.Enums;

public sealed class DashboardModel : DbEntity
{
    [Required]
    [MaxLength(40)]
    public string Title { get; set; }

    public string Description { get; set; }

    public List<UserType> AllowedUserTypes { get; set; } = new List<UserType>();

    public ProjectModel Project { get; set; }

    public List<TaskModel> Tasks { get; set; } = new List<TaskModel>();
}
