using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace TimeManager.WebUI.Services.Snackbar;

public class SnackbarService : ISnackbarService
{
    [Inject] public ISnackbar Snackbar { get; set; }

    public SnackbarService(ISnackbar snackbar)
    {
        Snackbar = snackbar;
        LoadDeafulfConfiguration();
    }

    #region PrivateMethods

    private void LoadDeafulfConfiguration()
    {
        Snackbar.Configuration.PreventDuplicates = false;
        Snackbar.Configuration.RequireInteraction = true;
        Snackbar.Configuration.HideTransitionDuration = 500;
        Snackbar.Configuration.ShowCloseIcon = true;
    }

    private void LoadHideConfiguration(bool hide)
    {
        if (hide)
        {
            Snackbar.Configuration.RequireInteraction = false;
            Snackbar.Configuration.ShowCloseIcon = false;
        }
        else
        {
            Snackbar.Configuration.RequireInteraction = true;
            Snackbar.Configuration.ShowCloseIcon = true;
        }
    }

    #endregion PrivateMethods

    #region PublicMethods

    public void Show(string message, Severity s, bool hide = false, bool showDate = true)
    {
        var now = DateTime.Now;
        var sNow = showDate ? $"[{now:yyyy-MM-dd HH:mm:ss}] " : string.Empty;

        LoadHideConfiguration(hide);

        Snackbar.Add($"{sNow}{message}", s);
    }

    #endregion PublicMethods
}