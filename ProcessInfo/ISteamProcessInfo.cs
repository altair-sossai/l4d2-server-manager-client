using System.Diagnostics;
using L4D2AntiCheat.ProcessInfo.Infrastructure;

namespace L4D2AntiCheat.ProcessInfo;

public interface ISteamProcessInfo : IProcessInfo
{
    Process? Start(string steamPath);
}