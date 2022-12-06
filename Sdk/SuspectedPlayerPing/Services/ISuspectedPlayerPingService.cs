using L4D2AntiCheat.Sdk.SuspectedPlayerPing.Commands;
using Refit;

namespace L4D2AntiCheat.Sdk.SuspectedPlayerPing.Services;

public interface ISuspectedPlayerPingService
{
    [Post("/api/suspected-players-ping")]
    Task PingAsync([Body] PingCommand command);
}