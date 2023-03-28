namespace ProjectManagementService.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public abstract class DbEntity
{
    [Key]
    [Required]
    [Column(TypeName = "uuid")]
    public Guid ID { get; set; } = Guid.NewGuid();
}
