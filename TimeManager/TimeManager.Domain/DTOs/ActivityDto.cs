namespace TimeManager.Domain.DTOs;

public class ActivityDto
{
    public DateTime Day { get; set; }
    public string? Title { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Task { get; set; } = string.Empty;
    public string Hour { get; set; } = string.Empty;
    public int RepetitionTypeId { get; set; }
    public string RepetitionTypeName { get; set; } = string.Empty;
    public int? RepetitionDay { get; set; } //todo
    public bool IsOpen { get; set; } = false;
    public int UserId { get; set; }
}