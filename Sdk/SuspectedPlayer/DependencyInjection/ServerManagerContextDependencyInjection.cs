using Microsoft.Extensions.DependencyInjection;

namespace L4D2AntiCheat.Sdk.SuspectedPlayer.DependencyInjection;

public static class ServerManagerContextDependencyInjection
{
    public static void AddServerManagerContext(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped(serviceProvider => serviceProvider.GetRequiredService<IServerManagerContext>().SuspectedPlayerService);
        serviceCollection.AddScoped(serviceProvider => serviceProvider.GetRequiredService<IServerManagerContext>().SuspectedPlayerSecretService);
        serviceCollection.AddScoped(serviceProvider => serviceProvider.GetRequiredService<IServerManagerContext>().VirtualMachineService);
    }
}