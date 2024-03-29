namespace TimeManager.Domain.DTOs;

public class ListDto
{
    public int ID { get; init; }
    public string Name { get; set; } = string.Empty;
    public bool IsChecked { get; set; }

    public List<string> Tasks { get; set; } = [];
}
