using System.ComponentModel.DataAnnotations;

namespace TimeManager.Domain.Entities;

public class User
{
    //[Required]
    public int Id { get; set; }
    //[Required]
    public string Email { get; set; } = null!;
    //[Required]
    public string Password { get; set; } = null!;
}