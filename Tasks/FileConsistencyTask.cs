using L4D2AntiCheat.Context;
using L4D2AntiCheat.Modules.FileConsistency.Services;
using L4D2AntiCheat.ProcessInfo;
using L4D2AntiCheat.Tasks.Infrastructure;

namespace L4D2AntiCheat.Tasks;

public class FileConsistencyTask : IntervalTask
{
	private readonly IFileConsistencyService _fileConsistencyService;
	private readonly ILeft4Dead2ProcessInfo _processInfo;

	public FileConsistencyTask(ILeft4Dead2ProcessInfo processInfo,
		IFileConsistencyService fileConsistencyService)
		: base(TimeSpan.FromMinutes(1))
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
		var process = _processInfo.CurrentProcess;
		if (process == null)
			return;

		var fileName = process.MainModule?.FileName;
		if (string.IsNullOrEmpty(fileName))
			return;

		var fileInfo = new FileInfo(fileName);
		var directoryInfo = fileInfo.Directory;
		if (directoryInfo == null)
			return;

		var consistency = _fileConsistencyService.CheckFilesConsistency(directoryInfo.FullName, process.StartTime);

		context.InconsistentFiles = !consistency.IsValid;
	}
}