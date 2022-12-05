using System.Text.Json;
using L4D2AntiCheat.Sdk.SuspectedPlayer.Services;
using L4D2AntiCheat.Sdk.SuspectedPlayerSecret.Services;
using L4D2AntiCheat.Sdk.VirtualMachine.Services;
using Refit;

namespace L4D2AntiCheat.Sdk;

public class ServerManagerContext : IServerManagerContext
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

    public ISuspectedPlayerService SuspectedPlayerService => CreateService<ISuspectedPlayerService>();
    public ISuspectedPlayerSecretService SuspectedPlayerSecretService => CreateService<ISuspectedPlayerSecretService>();
    public IVirtualMachineService VirtualMachineService => CreateService<IVirtualMachineService>();

    private static T CreateService<T>()
    {
        return RestService.For<T>(BaseUrl, Settings);
    }
}