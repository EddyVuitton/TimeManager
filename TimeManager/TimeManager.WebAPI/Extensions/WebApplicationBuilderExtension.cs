using TimeManager.WebAPI.Helpers;

namespace TimeManager.WebAPI.Extensions;

public static class WebApplicationBuilderExtension
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        ConfigurationHelper.Initialize(builder.Configuration);
        builder.Services.AddContextFactory();
        builder.Services.AddBusinessLogics();
    }
}