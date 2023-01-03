using System.Diagnostics;

namespace L4D2AntiCheat.ProcessInfo;

public class SteamProcessInfo : Infrastructure.ProcessInfo, ISteamProcessInfo
{
    public SteamProcessInfo()
        : base("steam")
    {
    }

    public Process? Start(string steamPath)
    {
        var fileInfo = new FileInfo(steamPath);
        var processStartInfo = new ProcessStartInfo
        {
            FileName = fileInfo.FullName,
            WorkingDirectory = fileInfo.DirectoryName!,
            Arguments = "-applaunch 550"
        };

        return CurrentProcess = Process.Start(processStartInfo);
    }
}