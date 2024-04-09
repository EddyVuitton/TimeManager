using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeManager.Domain.Entities;

public class UserAccount
{
    [Key]
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    [Column(TypeName = "datetime")]
    public DateTime CreatedOn { get; set; } = DateTime.Now;
}