using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeManager.Domain.Entities;

public class Repetition
{
    [Key]
    public int Id { get; set; }
    public int RepetitionTypeId { get; set; }
    public string? InitialTitle { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime CreatedOn { get; set; } = DateTime.Now;

    public virtual RepetitionType RepetitionType { get; set; } = null!;
}