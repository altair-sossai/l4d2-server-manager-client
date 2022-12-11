using System.Diagnostics;

namespace L4D2AntiCheat.Sdk.SuspectedPlayerProcess.Commands;

public class ProcessCommand
{
    public ProcessCommand(Process process)
    {
        ProcessName = process.ProcessName;
        WindowTitle = process.MainWindowTitle;

        var module = process.MainModule;
        FileName = module?.FileName;
        Module = module?.ModuleName;

        var fileVersionInfo = module?.FileVersionInfo;
        CompanyName = fileVersionInfo?.CompanyName;
        FileDescription = fileVersionInfo?.FileDescription;
        FileVersion = fileVersionInfo?.FileVersion;
        OriginalFilename = fileVersionInfo?.OriginalFilename;
        ProductName = fileVersionInfo?.ProductName;
    }

    public string? ProcessName { get; }
    public string? WindowTitle { get; }
    public string? FileName { get; }
    public string? Module { get; }
    public string? CompanyName { get; }
    public string? FileDescription { get; }
    public string? FileVersion { get; }
    public string? OriginalFilename { get; }
    public string? ProductName { get; }
}