﻿namespace TimeManager.Data.DTOs;

public class ActivityDto
{
    public DateTime Day { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Task { get; set; } = string.Empty;
    public string Hour { get; set; } = string.Empty;
    public string RepetitionType { get; set; } = string.Empty;
    public int RepetitionDay { get; set; } //todo
}