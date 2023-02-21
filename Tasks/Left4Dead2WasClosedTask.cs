using L4D2AntiCheat.Context;
using L4D2AntiCheat.ProcessInfo;
using L4D2AntiCheat.Tasks.Infrastructure;

namespace L4D2AntiCheat.Tasks;

public class Left4Dead2WasClosedTask : IntervalTask
{
    private readonly ILeft4Dead2ProcessInfo _processInfo;

    public Left4Dead2WasClosedTask(ILeft4Dead2ProcessInfo processInfo)
        : base(TimeSpan.FromSeconds(30))
    {
        _processInfo = processInfo;
    }

    protected override bool CanRun(AntiCheatContext context)
    {
        return !context.Left4Dead2WasClosed && _processInfo.IsRunning;
    }

    protected override void Run(AntiCheatContext context)
    {
        context.Left4Dead2WasClosed = _processInfo.WasClosed;
    }
}