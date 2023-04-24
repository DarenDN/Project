namespace DataBaseContract.Data;
using System.ComponentModel.DataAnnotations;

public abstract class DbEntity
{
    [Required]
    public Guid Id { get; set; } = Guid.NewGuid();
}
