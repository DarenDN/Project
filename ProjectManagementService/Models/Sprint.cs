namespace ProjectManagementService.Models;
using Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Sprint
public sealed class Sprint : DbEntity
{
    [Required]
    [MaxLength(150)]
    public string Name { get; set; }

    public string Description { get; set; } = string.Empty;

    [Required]
    public Guid ProjectId { get; set; }

    [Required]
    [Column(TypeName = "date")]
    public DateTime DateStart { get; set; }

    [Required]
    [Column(TypeName = "date")]
    public DateTime DateEnd { get; set;}
}
