namespace TimeManager.Server.Helpers;

public static class ConfigurationHelper
{
    public static IConfiguration Config { get; set; } = null!;
    public static string WebAPIHostAddress { get; set; } = null!;

    public static void Initialize(IConfiguration Configuration)
    {
        Config = Configuration;

        WebAPIHostAddress = Config.GetConnectionString("WebAPIHostAddress")
            ?? throw new NullReferenceException("There is no WebAPI host address in configuration file");
    }
}