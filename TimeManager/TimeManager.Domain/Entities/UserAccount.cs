using System.ComponentModel.DataAnnotations;

namespace TimeManager.Domain.Entities;

public class UserAccount
{
    [Key]
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}