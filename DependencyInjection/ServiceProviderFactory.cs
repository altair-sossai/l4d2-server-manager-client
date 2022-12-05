using Microsoft.Extensions.DependencyInjection;

namespace L4D2AntiCheat.DependencyInjection;

public static class ServiceProviderFactory
{
    public static ServiceProvider New()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddApp();

        return serviceCollection.BuildServiceProvider();
    }
}