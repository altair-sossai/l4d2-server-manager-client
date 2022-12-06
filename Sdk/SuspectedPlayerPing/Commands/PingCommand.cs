using System.Runtime.InteropServices;
using L4D2AntiCheat.Infrastructure.Helpers;

namespace L4D2AntiCheat.Sdk.SuspectedPlayerPing.Commands;

public class PingCommand
{
    public PingCommand()
    {
        var left4Dead2Process = ProcessHelper.Left4Dead2();
        var foregroundWindow = GetForegroundWindow();

        GetWindowThreadProcessId(foregroundWindow, out var foregroundWindowProcessId);

        Focused = left4Dead2Process?.Id == foregroundWindowProcessId;
    }

    public bool Focused { get; }

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern int GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
}