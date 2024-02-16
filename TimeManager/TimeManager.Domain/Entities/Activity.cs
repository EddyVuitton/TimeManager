using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeManager.Domain.Entities;

public class Activity
{
    [Key]
    public int Id { get; set; }
    public DateTime Day { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string Task { get; set; } = null!;
    public string Hour { get; set; } = null!;
    [ForeignKey(nameof(Repetition))]
    public int RepetitionId { get; set; }
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }

    public virtual Repetition Repetition { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}