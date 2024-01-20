using Microsoft.AspNetCore.Components;
using TimeManager.Data.DTOs;

namespace TimeManager.Components.Components;

public partial class Activity
{
    [Parameter] public List<ActivityDto>? ActivitiesDto { get; set; }

    protected override void OnInitialized()
    {
    }
}