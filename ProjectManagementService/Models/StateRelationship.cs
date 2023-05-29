namespace ProjectManagementService.Models;
using ProjectManagementService.Data;
using System.ComponentModel.DataAnnotations;

public class StateRelationship : DbEntity
{
    [Required]
    public Guid StateCurrent { get; set; }

    [Required]
    public Guid StateNext { get; set; }
}
