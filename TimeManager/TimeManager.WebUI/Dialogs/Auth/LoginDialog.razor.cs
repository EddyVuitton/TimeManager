using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using TimeManager.Domain.Forms;
using TimeManager.WebUI.Auth;
using TimeManager.WebUI.Services.Account;
using TimeManager.WebUI.Services.Snackbar;

namespace TimeManager.WebUI.Dialogs.Auth;

public partial class LoginDialog
{
    [Inject] public IAccountService AccountService { get; set; } = null!;
    [Inject] public ISnackbarService SnackbarService { get; set; } = null!;
    [Inject] public ILoginService LoginService { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;

    private readonly LoginAccountForm _model = new();

    private async void OnValidSubmit(EditContext context)
    {
        try
        {
            var loginForm = context.Model as LoginAccountForm ?? new();
            var response = await AccountService.LoginAsync(loginForm);

            if (!response.IsSuccess)
            {
                throw new Exception(response.Message ?? "Nieudana próba logowania");
            }

            if (response.Data is not null)
            {
                await LoginService.LoginAsync(response.Data);
                NavigationManager.NavigateTo("/");
            }

            SnackbarService.Show("Poprawnie zalogowano", Severity.Info, true, false, Defaults.Classes.Position.TopStart);
        }
        catch (Exception ex)
        {
            SnackbarService.Show(ex.Message, Severity.Error, true, false);
        }
    }

    private void Cancel() => MudDialog.Cancel();
}