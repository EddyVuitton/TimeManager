using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeManager.Domain.Entities;

public class ActivityList
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public bool IsDefault { get; set; }
    [ForeignKey(nameof(UserAccount))]
    public int UserId { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime CreatedOn { get; set; } = DateTime.Now;

    public virtual UserAccount UserAccount { get; set; } = null!;
}