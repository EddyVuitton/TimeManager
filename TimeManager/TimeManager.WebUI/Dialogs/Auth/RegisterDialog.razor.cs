using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using TimeManager.Domain.Forms;
using TimeManager.WebUI.Services.Account;
using TimeManager.WebUI.Services.Snackbar;

namespace TimeManager.WebUI.Dialogs.Auth;

public partial class RegisterDialog
{
    [Inject] public IAccountService AccountService {get; init; } = null!;
    [Inject] public ISnackbarService SnackbarService {get; init; } = null!;
    [Inject] public IDialogService DialogService {get; init; } = null!;

    [CascadingParameter] public MudDialogInstance MudDialog {get; private init; } = null!;

    private readonly RegisterAccountForm _model = new();

    private async void OnValidSubmit(EditContext context)
    {
        try
        {
            var registerForm = context.Model as RegisterAccountForm ?? new();
            var response = await AccountService.RegisterAsync(registerForm);

            if (!response.IsSuccess)
            {
                SnackbarService.Show(response.Message!, Severity.Error, true, false);
                return;
            }

            SnackbarService.Show("Konto zostało poprawnie zarejestrowane", Severity.Success, true, false);
            await OpenLoginDialog();
        }
        catch (Exception ex)
        {
            SnackbarService.Show("Konto nie zostało zarejestrowane", Severity.Warning, true, false);
            SnackbarService.Show(ex.Message, Severity.Error);
        }
    }

    private async Task OpenLoginDialog()
    {
        Cancel();
        await Task.Delay(400);

        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            NoHeader = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        };

        var parameters = new DialogParameters
        {
            { "RegisterAccountForm", _model }
        };

        DialogService.Show<LoginDialog>(string.Empty, parameters, options);
    }

    private void Cancel() => MudDialog.Cancel();
}