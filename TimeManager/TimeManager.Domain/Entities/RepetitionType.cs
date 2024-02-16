using System.ComponentModel.DataAnnotations;

namespace TimeManager.Domain.Entities;

public class RepetitionType
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}