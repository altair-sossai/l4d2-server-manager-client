using System.Diagnostics;

namespace L4D2AntiCheat.Infrastructure.Helpers;

public static class ProcessHelper
{
    public static bool Left4Dead2IsRunning()
    {
        return Process.GetProcessesByName("left4dead2").Any();
    }

    public static Process? Left4Dead2()
    {
        return Process.GetProcessesByName("left4dead2").FirstOrDefault();
    }
}