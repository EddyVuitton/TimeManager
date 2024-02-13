using System.ComponentModel.DataAnnotations;

namespace TimeManager.Domain.Entities;

public class RepetitionType
{
    //[Required]
    public int Id { get; set; }
    //[Required]
    public string Name { get; set; } = string.Empty;
}