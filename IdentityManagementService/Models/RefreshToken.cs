namespace IdentityManagementService.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class RefreshToken
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Token { get; set; }

    [Required]
    [Column(TypeName = "timestamp without time zone")]
    public DateTime Expires { get; set; }

    [Required]
    [Column(TypeName = "timestamp without time zone")]
    public DateTime Created { get; set; } = DateTime.Now;
}
