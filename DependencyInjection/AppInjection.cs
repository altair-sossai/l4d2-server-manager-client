using System.Reflection;
using L4D2AntiCheat.Sdk.DependencyInjection;
using L4D2AntiCheat.Tasks.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

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
            .AsImplementedInterfaces(type => assemblies.Contains(type.Assembly))
            .WithScopedLifetime());

        serviceCollection.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo<Form>()));

        serviceCollection.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo<IIntervalTask>())
            .AsSelf()
            .WithScopedLifetime());

        Log.Logger = new LoggerConfiguration()
            .WriteTo.File("logs/l4d2_.txt", rollingInterval: RollingInterval.Day, retainedFileTimeLimit: TimeSpan.FromDays(30))
            .CreateLogger();

        serviceCollection.AddSingleton(Log.Logger);

        serviceCollection.AddServerManagerSdk();
    }
}