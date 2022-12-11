using System.Diagnostics;

namespace L4D2AntiCheat.Sdk.SuspectedPlayerProcess.Commands;

public class ProcessCommand
{
    public ProcessCommand(Process process)
    {
        ProcessName = process.ProcessName;
        WindowTitle = process.MainWindowTitle;

        var mainModule = TryGetMainModule(process);

        FileName = mainModule?.FileName;
    }

    public string? ProcessName { get; }
    public string? WindowTitle { get; }
    public string? FileName { get; }

    private static ProcessModule? TryGetMainModule(Process process)
    {
        try
        {
            return process.MainWindowHandle == IntPtr.Zero ? null : process.MainModule;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            return null;
        }
    }
}