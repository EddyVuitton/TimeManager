using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace TimeManager.WebUI.Components;

public partial class MenuList
{
    [CascadingParameter] public MudMenu MudMenu { get; set; } = null!;

    private void CloseMenu()
    {
        MudMenu.CloseMenu();
    }
}