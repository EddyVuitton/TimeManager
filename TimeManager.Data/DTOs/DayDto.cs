namespace TimeManager.Data.DTOs;

public class DayDto
{
    public DateTime Day { get; set; }
    public List<ActivityDto> Activities { get; set; } = new();
}