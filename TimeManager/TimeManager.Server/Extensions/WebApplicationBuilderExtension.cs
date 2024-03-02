using MudBlazor.Services;
using TimeManager.Server.Helpers;

namespace TimeManager.Server.Extensions;

public static class WebApplicationBuilderExtension
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        builder.Services.AddMudServices();
        ConfigurationHelper.Initialize(builder.Configuration);
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(ConfigurationHelper.WebAPIHostAddress) });
        builder.Services.AddApiServices();
    }
}