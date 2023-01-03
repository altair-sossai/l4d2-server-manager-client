using System.Runtime.InteropServices;
using L4D2AntiCheat.Infrastructure.Extensions;

namespace L4D2AntiCheat.ProcessInfo;

public class Left4Dead2ProcessInfo : Infrastructure.ProcessInfo, ILeft4Dead2ProcessInfo
{
	private readonly ISteamProcessInfo _steamProcessInfo;

	public Left4Dead2ProcessInfo(ISteamProcessInfo steamProcessInfo)
		: base("left4dead2")
	{
		_steamProcessInfo = steamProcessInfo;
	}

	public bool IsFocused
	{
		get
		{
			var currentProcess = CurrentProcess;
			if (currentProcess == null)
				return false;

			var foregroundWindow = GetForegroundWindow();

			_ = GetWindowThreadProcessId(foregroundWindow, out var foregroundWindowProcessId);

			return currentProcess.Id == foregroundWindowProcessId;
		}
	}

	public string? RootFolder
	{
		get
		{
			var path = CurrentProcess?.MainModule?.FileName;
			if (string.IsNullOrEmpty(path))
				return null;

			var fileInfo = new FileInfo(path);
			var directoryInfo = fileInfo.Directory;

			return directoryInfo?.FullName;
		}
	}

	public bool AttachProcess()
	{
		var currentProcess = _steamProcessInfo.CurrentProcess;
		if (currentProcess == null)
			return false;

		for (var i = 0; i < 60; i++)
		{
			Thread.Sleep(1000);

			var process = GetProcess();
			if (process == null)
				continue;

			var parent = process.Parent();
			if (parent == null || currentProcess.Id != parent.Id)
				return false;

			CurrentProcess = process;

			return true;
		}

		return false;
	}

	[DllImport("user32.dll")]
	private static extern IntPtr GetForegroundWindow();

	[DllImport("user32.dll")]
	private static extern int GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
}