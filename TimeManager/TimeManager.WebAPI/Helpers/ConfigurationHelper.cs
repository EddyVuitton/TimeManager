namespace TimeManager.WebAPI.Helpers;

public static class ConfigurationHelper
{
    public static IConfiguration Config { get; set; } = null!;
    public static string DatabaseConnectionString { get; set; } = null!;
    public static string TempDatabaseConnectionString { get; set; } = null!;
    public static string JWTKey { get; set; } = null!;

    public static void Initialize(IConfiguration Configuration)
    {
        Config = Configuration;

        DatabaseConnectionString = Config.GetConnectionString("DatabaseConnection")
            ?? throw new NullReferenceException("There is no database connection in configuration file");

        TempDatabaseConnectionString = Config.GetConnectionString("TempDatabaseConnection")
            ?? throw new NullReferenceException("There is no temp database connection in configuration file");

        JWTKey = Config.GetSection("JWT:Key").Value
            ?? throw new NullReferenceException("There is no JWT key in configuration file");
    }
}