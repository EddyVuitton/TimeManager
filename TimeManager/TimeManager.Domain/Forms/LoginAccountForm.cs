using System.ComponentModel.DataAnnotations;

namespace TimeManager.Domain.Forms;

public class LoginAccountForm
{
    [Required(ErrorMessage = "To pole jest wymagane...")]
    [EmailAddress(ErrorMessage = "Nieprawidłowy adres email...")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "To pole jest wymagane...")]
    public string? Password { get; set; }
}