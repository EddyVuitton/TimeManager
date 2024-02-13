namespace TimeManager.Server.Extensions;

public static class IServiceCollectionExtension
{
    public static void AddServices(this IServiceCollection service)
    {
        service
            .AddRazorComponents()
            .AddInteractiveServerComponents();
    }
}