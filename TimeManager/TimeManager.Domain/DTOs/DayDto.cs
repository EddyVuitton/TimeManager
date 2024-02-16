namespace TimeManager.Domain.DTOs;

public class DayDto
{
    public DateTime Day { get; set; }
    public List<ActivityDto> Activities { get; set; } = [];
}