namespace TimeManager.Domain.DTOs;

public class RepetitionDto
{
    public int RepetitionId { get; set; }
    public int RepetitionTypeId { get; set; }
    public string RepetitionName { get; set; } = null!;
    public string? InitialTitle { get; set; }
    public int ActivityListId { get; set; }
}