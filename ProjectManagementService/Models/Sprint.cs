namespace ProjectManagementService.Models;
using ProjectManagementService.Data;
using System.ComponentModel.DataAnnotations;

// Sprint
public sealed class Sprint : DbEntity
{
    [Required]
    [MaxLength(150)]
    public string Name { get; set; }

    public string Description { get; set; } = string.Empty;

    [Required]
    public DateTime DateStart { get; set; }

    [Required] 
    public DateTime DateEnd { get; set;}
}
