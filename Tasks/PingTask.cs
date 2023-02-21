using L4D2AntiCheat.Context;
using L4D2AntiCheat.ProcessInfo;
using L4D2AntiCheat.Sdk.SuspectedPlayerPing.Commands;
using L4D2AntiCheat.Sdk.SuspectedPlayerPing.Services;
using L4D2AntiCheat.Tasks.Infrastructure;

namespace L4D2AntiCheat.Tasks;

public class PingTask : IntervalTask
{
    private static readonly TimeSpan Interval = TimeSpan.FromSeconds(30);

    private readonly ILeft4Dead2ProcessInfo _processInfo;
    private readonly ISuspectedPlayerPingService _suspectedPlayerPingService;

    public PingTask(ILeft4Dead2ProcessInfo processInfo,
        ISuspectedPlayerPingService suspectedPlayerPingService)
        : base(Interval)
    {
        _processInfo = processInfo;
        _suspectedPlayerPingService = suspectedPlayerPingService;
    }

    protected override void Run(AntiCheatContext context)
    {
        var focused = _processInfo.IsFocused;
        var command = new PingCommand(focused);

        _suspectedPlayerPingService.PingAsync(command).Wait();
    }
}