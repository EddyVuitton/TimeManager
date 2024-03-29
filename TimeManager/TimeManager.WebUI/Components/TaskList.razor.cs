using Microsoft.AspNetCore.Components;
using TimeManager.Domain.DTOs;

namespace TimeManager.WebUI.Components;

public partial class TaskList
{
    [Parameter] public ListDto CustomList { get; init; } = null!;

    private void AddTask()
    {
        CustomList.Tasks.Add("Zadanie");
    }
}