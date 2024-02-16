using System.ComponentModel.DataAnnotations;

namespace TimeManager.Domain.Entities;

public class Repetition
{
    [Key]
    public int Id { get; set; }
    public DateTime? Day { get; set; }
    public int RepetitionTypeId { get; set; }

    public virtual RepetitionType RepetitionType { get; set; } = null!;
}