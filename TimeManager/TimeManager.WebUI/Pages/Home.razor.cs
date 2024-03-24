using Microsoft.AspNetCore.Components;
using MudBlazor;
using TimeManager.WebUI.Auth;
using TimeManager.WebUI.Services.Account;
using TimeManager.WebUI.Services.Snackbar;

namespace TimeManager.WebUI.Pages;

public partial class Home
{
    [Inject] public ILoginService LoginService { get; set; } = null!;
    [Inject] public IAccountService AccountService { get; set; } = null!;
    [Inject] public ISnackbarService SnackbarService { get; set; } = null!;

    private int _userId;

    private bool _isInitialized = false;

    protected override async Task OnInitializedAsync()
    {
        await LoginService.LogoutIfExpiredTokenAsync();
        var userEmail = await LoginService.IsLoggedInAsync();

        if (!string.IsNullOrEmpty(userEmail))
        {
            try
            {
                var result = await AccountService.GetUserByEmailAsync(userEmail);

                if (!result.IsSuccess)
                {
                    throw new Exception(result.Message ?? "Błąd w pobraniu użytkownika...");
                }

                if (result.Data is not null)
                    _userId = result.Data.Id;
            }
            catch (Exception ex)
            {
                SnackbarService.Show(ex.Message, Severity.Warning, true, false);
            }
        }
        else
        {
            _userId = 0;
        }

        _isInitialized = true;
    }

    public int GetUserId() => _userId;
}