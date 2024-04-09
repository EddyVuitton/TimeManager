using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeManager.Domain.Entities;

public class RepetitionType
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    [Column(TypeName = "datetime")]
    public DateTime CreatedOn { get; set; } = DateTime.Now;
}