using System.Text.Json;
using L4D2AntiCheat.Sdk.Handlers;
using L4D2AntiCheat.Sdk.SuspectedPlayer.Services;
using L4D2AntiCheat.Sdk.SuspectedPlayerPing.Services;
using L4D2AntiCheat.Sdk.SuspectedPlayerScreenshot.Services;
using L4D2AntiCheat.Sdk.SuspectedPlayerSecret.Services;
using L4D2AntiCheat.Sdk.VirtualMachine.Services;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace L4D2AntiCheat.Sdk.DependencyInjection;

public static class ServerManagerSdkDependencyInjection
{
#if DEBUG
    private const string BaseUrl = "http://localhost:7094";
#else
    private const string BaseUrl = "https://l4d2-server-manager-api.azurewebsites.net";
#endif

    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    private static readonly RefitSettings Settings = new()
    {
        ContentSerializer = new SystemTextJsonContentSerializer(Options)
    };

    public static void AddServerManagerSdk(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<AuthorizationHeaderHandler>();

        serviceCollection.AddRefitService<IVirtualMachineService>();

        serviceCollection.AddRefitService<ISuspectedPlayerService>();
        serviceCollection.AddRefitService<ISuspectedPlayerSecretService>();
        serviceCollection.AddRefitService<ISuspectedPlayerPingService>();
        serviceCollection.AddRefitService<ISuspectedPlayerScreenshotService>();
    }

    private static void AddRefitService<TService>(this IServiceCollection serviceCollection)
        where TService : class
    {
        serviceCollection.AddRefitClient<TService>(Settings)
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(BaseUrl))
            .AddHttpMessageHandler<AuthorizationHeaderHandler>();
    }
}