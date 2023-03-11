using System.Text.Json;
using L4D2AntiCheat.Sdk.Handlers;
using L4D2AntiCheat.Sdk.ServerPing.Services;
using L4D2AntiCheat.Sdk.SuspectedPlayer.Services;
using L4D2AntiCheat.Sdk.SuspectedPlayerFileCheck.Services;
using L4D2AntiCheat.Sdk.SuspectedPlayerMetadata.Services;
using L4D2AntiCheat.Sdk.SuspectedPlayerPing.Services;
using L4D2AntiCheat.Sdk.SuspectedPlayerProcess.Services;
using L4D2AntiCheat.Sdk.SuspectedPlayerScreenshot.Services;
using L4D2AntiCheat.Sdk.SuspectedPlayerSecret.Services;
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

        serviceCollection.AddRefitService<IServerPingService>();
        serviceCollection.AddRefitService<ISuspectedPlayerService>();
        serviceCollection.AddRefitService<ISuspectedPlayerFileCheck>();
        serviceCollection.AddRefitService<ISuspectedPlayerPingService>();
        serviceCollection.AddRefitService<ISuspectedPlayerProcessService>();
        serviceCollection.AddRefitService<ISuspectedPlayerScreenshotService>();
        serviceCollection.AddRefitService<ISuspectedPlayerSecretService>();
        serviceCollection.AddRefitService<ISuspectedPlayerMetadataService>();
	}

    private static void AddRefitService<TService>(this IServiceCollection serviceCollection)
        where TService : class
    {
        serviceCollection.AddRefitClient<TService>(Settings)
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { ServerCertificateCustomValidationCallback = (_, _, _, _) => true })
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(BaseUrl))
            .AddHttpMessageHandler<AuthorizationHeaderHandler>();
    }
}