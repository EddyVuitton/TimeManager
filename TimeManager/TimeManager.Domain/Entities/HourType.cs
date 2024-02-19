using System.ComponentModel.DataAnnotations;

namespace TimeManager.Domain.Entities;

public class HourType
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}