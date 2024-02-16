using Swashbuckle.AspNetCore.SwaggerUI;
using TimeManager.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DefaultModelsExpandDepth(-1);
        c.DocExpansion(docExpansion: DocExpansion.None);
        c.EnableTryItOutByDefault();
    });
}

/*
 * Podczas ka�dego uruchomienia aplikacji migruj baz� DBContext.cs do lokalnego serwera SQL (localdb)\\MSSQLLocalDB
*/
if (app.Environment.IsDevelopment()) //Tylko i wy��cznie na �rodowisku deweloperskim
{
    app.ReMigrateDatabase();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();