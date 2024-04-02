using Microsoft.AspNetCore.Components;
using MudBlazor;
using TimeManager.WebUI.Auth;
using TimeManager.WebUI.Services.Account;
using TimeManager.WebUI.Services.Snackbar;

namespace TimeManager.WebUI.Pages;

public partial class Calendar
{
    [Inject] public ILoginService LoginService {get; init; } = null!;
    [Inject] public IAccountService AccountService {get; init; } = null!;
    [Inject] public ISnackbarService SnackbarService {get; init; } = null!;

    private int _userId;
    private bool _isInitialized = false;

    protected override async Task OnInitializedAsync()
    {
        _userId = await LoginService.GetUserIdFromToken();
        _isInitialized = true;
    }

    public int GetUserId() => _userId;
}