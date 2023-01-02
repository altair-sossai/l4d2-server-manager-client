using System.Diagnostics;

namespace L4D2AntiCheat.Infrastructure.Helpers;

public static class SteamProcessHelper
{
	private const string ProcessName = "steam";
	private static Process? _currentProcess;

	public static Process? CurrentProcess => _currentProcess?.HasExited ?? false ? null : _currentProcess;

	public static void SetCurrentProcess(Process process)
	{
		_currentProcess = process;
	}

	public static bool IsRunning()
	{
		return Process.GetProcessesByName(ProcessName).Any();
	}

	public static bool WasClosed()
	{
		var currentProcess = _currentProcess;
		if (currentProcess == null || currentProcess.HasExited)
			return true;

		var process = FindProcess();
		if (process == null)
			return true;

		return currentProcess.Id != process.Id;
	}

	public static void Clear()
	{
		_currentProcess = null;
	}

	private static Process? FindProcess()
	{
		var processes = Process.GetProcessesByName(ProcessName);
		var process = processes.FirstOrDefault();

		return process;
	}
}