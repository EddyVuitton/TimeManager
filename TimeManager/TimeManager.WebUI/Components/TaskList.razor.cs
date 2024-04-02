using Microsoft.AspNetCore.Components;
using TimeManager.Domain.DTOs;
using TimeManager.WebUI.Pages;

namespace TimeManager.WebUI.Components;

public partial class TaskList
{
    [Parameter] public ActivityListDto ActivityList { get; init; } = null!;
    [Parameter] public Tasks TasksRef { get; init; } = null!;

    private void AddTask()
    {
        //CustomList.Tasks.Add("Zadanie");
    }
}