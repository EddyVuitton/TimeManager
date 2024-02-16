namespace TimeManager.Domain.DTOs;

public class MonthDto
{
    public int Month { get; set; }
    public int Year { get; set; }
    public List<DayDto> Days { get; set; } = [];
}