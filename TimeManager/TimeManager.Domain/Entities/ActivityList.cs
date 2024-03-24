using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeManager.Domain.Entities;

public class ActivityList
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime Created { get; set; } = DateTime.Now;
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }

    public virtual UserAccount User { get; set; } = null!;
}