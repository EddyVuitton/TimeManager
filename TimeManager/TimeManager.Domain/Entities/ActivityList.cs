using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeManager.Domain.Entities;

public class ActivityList
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public bool IsDefault { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    [ForeignKey(nameof(UserAccount))]
    public int UserId { get; set; }

    public virtual UserAccount UserAccount { get; set; } = null!;
}