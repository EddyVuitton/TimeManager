using System.ComponentModel.DataAnnotations;

namespace TimeManager.Domain.Entities;

public class Activity
{
    //[Required]
    public int Id { get; set; }
    //[Required]
    public DateTime Day { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    //[Required]
    public string Task { get; set; } = null!;
    //[Required]
    public string Hour { get; set; } = null!;
    //[Required]
    public int RepetitionId { get; set; }
    //[Required]
    public int UserId { get; set; }

    //public virtual Repetition Repetition { get; set; } = null!;
    //public virtual User User { get; set; } = null!;
}