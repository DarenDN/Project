namespace ProjectManagementService.Models;
using Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public sealed class Project : DbEntity
{
    [Required]
    [MaxLength(40)]
    public string Title { get; set; }

    [MaxLength(10000)]
    public string Description { get; set; }

    [Required]
    [Column(TypeName = "timestamp without time zone")]
    public DateTime Created { get; set; } = DateTime.Now;
}
