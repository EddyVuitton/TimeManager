using Microsoft.AspNetCore.Components.Authorization;
using TimeManager.WebAPI.Auth;
using TimeManager.WebUI.Services.AccontService;
using TimeManager.WebUI.Services.ManagementService;
using TimeManager.WebUI.Services.SnackbarService;

namespace TimeManager.Server.Extensions;

public static class IServiceCollectionExtension
{
    public static void AddApiServices(this IServiceCollection service)
    {
        service.AddScoped<IManagementService, ManagementService>();
        service.AddScoped<ISnackbarService, SnackbarService>();
        service.AddScoped<IAccountService, AccountService>();
    }

    public static IServiceCollection AddAuthServices(this IServiceCollection services)
    {
        services.AddScoped<JWTAuthenticationStateProvider>();
        services.AddScoped<AuthenticationStateProvider, JWTAuthenticationStateProvider>(provider => provider.GetRequiredService<JWTAuthenticationStateProvider>());
        services.AddScoped<ILoginService, JWTAuthenticationStateProvider>(provider => provider.GetRequiredService<JWTAuthenticationStateProvider>());

        return services;
    }
}