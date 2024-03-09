using MudBlazor;

namespace TimeManager.WebUI.Services.Snackbar;

public interface ISnackbarService
{
    void Show(string message, Severity s, bool hide = false, bool showDate = true);
}