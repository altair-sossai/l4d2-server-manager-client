using System.Diagnostics;
using System.Runtime.InteropServices;
using L4D2AntiCheat.Infrastructure.Extensions;

namespace L4D2AntiCheat.Infrastructure.Helpers;

public static class Left4Dead2ProcessHelper
{
	private const string ProcessName = "left4dead2";
	private static Process? _currentProcess;

	public static Process? CurrentProcess => _currentProcess?.HasExited ?? false ? null : _currentProcess;

	public static bool IsRunning()
	{
		return Process.GetProcessesByName(ProcessName).Any();
	}

	public static bool WasClosed()
	{
		var currentProcess = CurrentProcess;
		if (currentProcess == null || currentProcess.HasExited)
			return true;

		var process = FindProcess();
		if (process == null)
			return true;

		return currentProcess.Id != process.Id;
	}

	public static bool CatchCurrentProcess()
	{
		for (var i = 0; i < 60; i++)
		{
			Thread.Sleep(1000);

			var process = FindProcess();
			if (process == null)
				continue;

			var parent = process.Parent();
			if (parent == null || SteamProcessHelper.CurrentProcess == null || SteamProcessHelper.CurrentProcess.Id != parent.Id)
				return false;

			_currentProcess = process;

			return true;
		}

		return false;
	}

	public static void Clear()
	{
		_currentProcess = null;
	}

	public static bool IsFocused()
	{
		if (CurrentProcess == null)
			return false;

		var foregroundWindow = GetForegroundWindow();

		_ = GetWindowThreadProcessId(foregroundWindow, out var foregroundWindowProcessId);

		return CurrentProcess.Id == foregroundWindowProcessId;
	}

	private static Process? FindProcess()
	{
		var processes = Process.GetProcessesByName(ProcessName);
		var process = processes.FirstOrDefault();

		return process;
	}

	[DllImport("user32.dll")]
	private static extern IntPtr GetForegroundWindow();

	[DllImport("user32.dll")]
	private static extern int GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
}