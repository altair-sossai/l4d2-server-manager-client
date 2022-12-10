using System.Diagnostics;
using System.Runtime.InteropServices;

namespace L4D2AntiCheat.Infrastructure.Helpers;

public static class Left4Dead2ProcessHelper
{
    private const string ProcessName = "left4dead2";

    public static Process? GetProcess()
    {
        return Process.GetProcessesByName(ProcessName).FirstOrDefault();
    }

    public static bool IsRunning()
    {
        return Process.GetProcessesByName(ProcessName).Any();
    }

    public static bool IsFocused()
    {
        var process = GetProcess();
        var foregroundWindow = GetForegroundWindow();

        GetWindowThreadProcessId(foregroundWindow, out var foregroundWindowProcessId);

        return process?.Id == foregroundWindowProcessId;
    }

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern int GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
}