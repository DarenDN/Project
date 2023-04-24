namespace DataBaseContract.Models.ScrumRelated;

using System.ComponentModel.DataAnnotations;
using Data;

public sealed class TaskType : DbEntity
{
    [Required]
    [MaxLength(20)]
    public string Name { get; set; }
}
