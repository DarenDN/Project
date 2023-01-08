namespace ProjectManagementService.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class DbEntity
{
    [Key]
    [Column(TypeName = "uuid")]
    public Guid ID { get; } = Guid.NewGuid();
}
