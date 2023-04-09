namespace ProjectManagementService.Models;
using Data;
using System.ComponentModel.DataAnnotations;

public sealed class Project : DbEntity
{
    [Required]
    [MaxLength(40)]
    public string Title { get; set; }
}
