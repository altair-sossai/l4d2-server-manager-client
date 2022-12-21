using L4D2AntiCheat.Sdk.ServerPing.Results;
using Refit;

namespace L4D2AntiCheat.Sdk.ServerPing.Services;

public interface IServerPingService
{
    [Get("/api/server-ping")]
    Task<ServerPingResult> GetAsync();
}