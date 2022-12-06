using System.Diagnostics;
using System.Runtime.InteropServices;

namespace L4D2AntiCheat.Infrastructure.Helpers;

public static class ProcessHelper
{
    public static bool Left4Dead2IsRunning()
    {
        return Process.GetProcessesByName("left4dead2").Any();
    }

    public static Process? Left4Dead2Process()
    {
        return Process.GetProcessesByName("left4dead2").FirstOrDefault();
    }

    public static bool Left4Dead2IsFocused()
    {
        var left4Dead2Process = Left4Dead2Process();
        var foregroundWindow = GetForegroundWindow();

        GetWindowThreadProcessId(foregroundWindow, out var foregroundWindowProcessId);

        return left4Dead2Process?.Id == foregroundWindowProcessId;
    }

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern int GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
}