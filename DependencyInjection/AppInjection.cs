using System.Reflection;
using L4D2AntiCheat.Sdk.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace L4D2AntiCheat.DependencyInjection;

public static class AppInjection
{
    public static void AddApp(this IServiceCollection serviceCollection)
    {
        var assemblies = new[]
        {
            Assembly.Load("L4D2AntiCheat")
        };

        serviceCollection.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses()
            .AsImplementedInterfaces(type => assemblies.Contains(type.Assembly)));

        serviceCollection.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo<Form>()));

        serviceCollection.AddServerManagerSdk();
    }
}