namespace DataBaseContract.Models.UserRelated;

using DataBaseContract.Data;
using DataBaseContract.Models.ScrumRelated;
using System.ComponentModel.DataAnnotations;

public sealed class User : DbEntity
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    [MaxLength(50)]
    public string SecondName { get; set; }
    [Required]
    public Project CorrespondingProject { get; set; }

    [Required]
    public DateTime RegisterTime { get; set; } = DateTime.Now;

    public UserRole Role { get; set; }
}
