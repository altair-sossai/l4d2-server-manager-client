using System.Diagnostics;
using L4D2AntiCheat.Context;
using L4D2AntiCheat.Sdk.SuspectedPlayerProcess.Commands;
using L4D2AntiCheat.Sdk.SuspectedPlayerProcess.Services;
using L4D2AntiCheat.Tasks.Infrastructure;

namespace L4D2AntiCheat.Tasks;

public class ProcessesTask : IntervalTask
{
    private static readonly TimeSpan Interval = TimeSpan.FromMinutes(2);

    private readonly ISuspectedPlayerProcessService _suspectedPlayerProcessService;

    public ProcessesTask(ISuspectedPlayerProcessService suspectedPlayerProcessService)
        : base(Interval)
    {
        _suspectedPlayerProcessService = suspectedPlayerProcessService;
    }

    protected override void Run(AntiCheatContext context)
    {
        var commands = Process.GetProcesses()
            .Where(process => process.Id != 0 && process.MainWindowHandle != IntPtr.Zero)
            .Select(process => new ProcessCommand(process))
            .ToList();

        _suspectedPlayerProcessService.AddOrUpdateAsync(commands).Wait();
    }
}