using System.ComponentModel.DataAnnotations.Schema;

namespace TimeManager.Domain.DTOs;

public class ActivityDto
{
    public int ActivityId { get; set; }
    public int RepetitionId { get; set; }
    public int ActivityListId { get; set; }
    [Column(TypeName = "date")]
    public DateTime Day { get; set; }
    public string? Title { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Task { get; set; } = string.Empty;
    public int HourTypeId { get; set; }
    public int RepetitionTypeId { get; set; }
    public bool IsOpen { get; set; } = false;
    public int UserId { get; set; }
}