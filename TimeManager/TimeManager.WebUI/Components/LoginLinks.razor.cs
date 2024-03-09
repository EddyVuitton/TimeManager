using Microsoft.AspNetCore.Components;
using MudBlazor;
using TimeManager.WebUI.Auth;
using TimeManager.WebUI.Dialogs.Auth;

namespace TimeManager.WebUI.Components;

public partial class LoginLinks
{
    [Inject] public ILoginService LoginService { get; set; } = null!;
    [Inject] public IDialogService DialogService { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    private async Task LogOut()
    {
        await LoginService.LogoutAsync();
        NavigationManager.NavigateTo("/");
    }

    private async Task OpenLoginDialog()
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            NoHeader = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        };

        await DialogService.ShowAsync<LoginDialog>(string.Empty, options);
    }
}