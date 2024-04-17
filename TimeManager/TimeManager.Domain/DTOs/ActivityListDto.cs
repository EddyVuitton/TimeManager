namespace TimeManager.Domain.DTOs;

public class ActivityListDto
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsChecked { get; set; }
    public bool IsDefault { get; set; }
    public int UserId { get; set; }

    public List<RepetitionDto> Repetitions { get; set; } = [];
}