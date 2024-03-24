using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeManager.Domain.Entities;

public class Activity
{
    [Key]
    public int Id { get; set; }
    [Column(TypeName = "date")]
    public DateTime Day { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string Task { get; set; } = null!;
    [ForeignKey(nameof(HourType))]
    public int HourTypeId { get; set; }
    [ForeignKey(nameof(Repetition))]
    public int RepetitionId { get; set; }
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    [ForeignKey(nameof(ActivityList))]
    public int ActivityListId { get; set; }

    public virtual Repetition Repetition { get; set; } = null!;
    public virtual UserAccount User { get; set; } = null!;
    public virtual HourType HourType { get; set; } = null!;
    public virtual ActivityList ActivityList { get; set; } = null!;
}