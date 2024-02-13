using TimeManager.Server.Extensions;
using TimeManager.WebUI.EntryPoint;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddServices(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

/*
 * Podczas ka¿dego uruchomienia aplikacji migruj bazê DBContext.cs do lokalnego serwera SQL (localdb)\\MSSQLLocalDB
*/
//if (app.Environment.IsDevelopment()) //Tylko i wy³¹cznie na œrodowisku deweloperskim
//{
//    app.ReMigrateDatabase();
//}

app.Run();