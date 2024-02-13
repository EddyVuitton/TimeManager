using System.ComponentModel.DataAnnotations;

namespace TimeManager.Domain.Entities;

public class Repetition
{
    //[Required]
    public int Id { get; set; }
    public DateTime? Day { get; set; }
    //[Required]
    public int RepetitionTypeId { get; set; }

    //public virtual RepetitionType RepetitionType { get; set; } = null!;
}