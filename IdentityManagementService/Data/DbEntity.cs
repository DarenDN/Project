namespace IdentityManagementService.Data;
using System.ComponentModel.DataAnnotations;

public abstract class DbEntity
{
    [Key]
    [Required]
    public Guid Id { get; set; } = Guid.NewGuid();
}
