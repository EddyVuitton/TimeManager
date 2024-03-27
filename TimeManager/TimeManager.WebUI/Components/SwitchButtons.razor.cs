using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace TimeManager.WebUI.Components;

public partial class SwitchButtons
{
    [Inject] public NavigationManager NavigationManager {get; init; } = null!;

    private Variant _calendarVariant = Variant.Filled;
    private Variant _listVariant = Variant.Outlined;

    private void SwitchToCalendar()
    {
        _calendarVariant = Variant.Filled;
        _listVariant = Variant.Outlined;
        NavigationManager.NavigateTo("/");
    }

    private void SwitchToTasks()
    {
        _calendarVariant = Variant.Outlined;
        _listVariant = Variant.Filled;
        NavigationManager.NavigateTo("/tasks");
    }
}