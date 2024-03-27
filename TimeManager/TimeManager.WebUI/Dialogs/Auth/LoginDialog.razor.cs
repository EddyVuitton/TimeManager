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
    [Inject] public IAccountService AccountService {get; init; } = null!;
    [Inject] public ISnackbarService SnackbarService {get; init; } = null!;
    [Inject] public ILoginService LoginService {get; init; } = null!;
    [Inject] public IDialogService DialogService {get; init; } = null!;
    [Inject] public NavigationManager NavigationManager {get; init; } = null!;

    [CascadingParameter] public MudDialogInstance MudDialog {get; private init; } = null!;

    [Parameter] public RegisterAccountForm? RegisterAccountForm { get; set; }

    private readonly LoginAccountForm _model = new();
    private readonly string _info = "Na potrzeby demo aplikacji jest stworzone konto \"konto@demo.com\" z hasłem \"demo\"";

    protected override void OnInitialized()
    {
        _model.Email = "konto@demo.com";
        _model.Password = "demo";

        if (RegisterAccountForm is not null)
        {
            _model.Email = RegisterAccountForm.Email;
            _model.Password = string.Empty;
        }
    }

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
                NavigationManager.NavigateTo("/", true);
            }
        }
        catch (Exception ex)
        {
            SnackbarService.Show(ex.Message, Severity.Error, true, false);
        }
    }

    private void Cancel() => MudDialog.Cancel();

    private async Task OpenRegisterDialog()
    {
        Cancel();
        await Task.Delay(250);

        var options = new DialogOptions
        {
            CloseOnEscapeKey = false,
            NoHeader = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        };

        DialogService.Show<RegisterDialog>(string.Empty, options);
    }
}