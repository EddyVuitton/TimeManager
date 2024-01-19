using MudBlazor.Services;

namespace TimeManager.Server.Helpers;

public static class BasicHelper
{
    public static void AddServices(WebApplicationBuilder builder)
    {
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddMudServices();
    }
}