namespace ProjectManagementService.Models;
using Data;
using System.ComponentModel.DataAnnotations;

public sealed class Project : DbEntity
{
    [Required]
    [MaxLength(40)]
    public string Title { get; set; }

    [MaxLength(200)]
    public string Description { get; set; }

    // TODO do we need an estimation type here?
    [Required]
    public EstimationType Estimation { get; set; }
}
