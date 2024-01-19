using System.Reflection;
using TimeManager.Components.Shared;

namespace TimeManager.Server;

public partial class App
{
    private readonly List<Assembly> _additionalAssemblies = new()
    {
        typeof(MainLayout).Assembly
    };
}