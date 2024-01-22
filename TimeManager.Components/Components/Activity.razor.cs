using Microsoft.AspNetCore.Components;
using MudBlazor;
using TimeManager.Components.Dialogs;
using TimeManager.Data.DTOs;

namespace TimeManager.Components.Components;

public partial class Activity
{
    [Inject] public IDialogService? DialogService { get; set; }

    [Parameter] public List<ActivityDto>? ActivitiesDto { get; set; }

    protected override void OnInitialized()
    {
    }

    private void OpenDialog()
    {
        //To do...
    }
}