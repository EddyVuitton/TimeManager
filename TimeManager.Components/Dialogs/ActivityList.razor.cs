using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace TimeManager.Components.Dialogs;

public partial class ActivityList
{
    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }
    [Parameter] public string Title { get; set; } = string.Empty;

    readonly List<int> _hours = new();

    protected override void OnInitialized()
    {
        for (int i = 0; i < 24; i++)
        {
            _hours.Add(i);
        }
    }

    void Submit() => MudDialog?.Close(DialogResult.Ok(true));
    void Cancel() => MudDialog?.Cancel();
}