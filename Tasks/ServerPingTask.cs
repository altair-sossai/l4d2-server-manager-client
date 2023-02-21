using L4D2AntiCheat.Context;
using L4D2AntiCheat.Sdk.ServerPing.Services;
using L4D2AntiCheat.Tasks.Infrastructure;

namespace L4D2AntiCheat.Tasks;

public class ServerPingTask : IntervalTask
{
    private readonly IServerPingService _serverPingService;

    public ServerPingTask(IServerPingService serverPingService)
        : base(TimeSpan.FromSeconds(30))
    {
        _serverPingService = serverPingService;
    }

    protected override void Run(AntiCheatContext context)
    {
        var result = _serverPingService.GetAsync().Result;

        context.ServerIsOn = result.IsOn;
    }
}