namespace TimeManager.Domain.DTOs;

public class ActivityListDto
{
    public int ID { get; init; }
    public string Name { get; set; } = string.Empty;
    public bool IsChecked { get; set; }

    public List<ActivityDto> Tasks { get; set; } = [];
}