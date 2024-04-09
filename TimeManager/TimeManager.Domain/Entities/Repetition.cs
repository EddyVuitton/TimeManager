using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeManager.Domain.Entities;

public class Repetition
{
    [Key]
    public int Id { get; set; }
    public int RepetitionTypeId { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime CreatedOn { get; set; } = DateTime.Now;

    public virtual RepetitionType RepetitionType { get; set; } = null!;
    public virtual List<Activity> Activities { get; set; } = null!;
}