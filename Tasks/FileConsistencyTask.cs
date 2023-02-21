using L4D2AntiCheat.Context;
using L4D2AntiCheat.Modules.FileConsistency.Services;
using L4D2AntiCheat.ProcessInfo;
using L4D2AntiCheat.Tasks.Infrastructure;

namespace L4D2AntiCheat.Tasks;

public class FileConsistencyTask : IntervalTask
{
    private static readonly TimeSpan Interval = TimeSpan.FromMinutes(1.5);

    private readonly IFileConsistencyService _fileConsistencyService;
    private readonly ILeft4Dead2ProcessInfo _processInfo;

    public FileConsistencyTask(ILeft4Dead2ProcessInfo processInfo,
        IFileConsistencyService fileConsistencyService)
        : base(Interval)
    {
        _processInfo = processInfo;
        _fileConsistencyService = fileConsistencyService;
    }

    protected override bool CanRun(AntiCheatContext context)
    {
        if (context.InconsistentFiles)
            return false;

        return _processInfo is { IsRunning: true, WasClosed: false, CurrentProcess: { } };
    }

    protected override void Run(AntiCheatContext context)
    {
        var folder = _processInfo.RootFolder;
        if (string.IsNullOrEmpty(folder))
            return;

        var startTime = _processInfo.CurrentProcess?.StartTime;
        if (startTime == null)
            return;

        var consistency = _fileConsistencyService.CheckFilesConsistency(folder, startTime.Value);

        context.InconsistentFiles = consistency.Inconsistent;
    }
}