using System.Diagnostics;

namespace L4D2AntiCheat.ProcessInfo.Infrastructure;

public interface IProcessInfo
{
	Process? CurrentProcess { get; set; }
	bool IsRunning { get; }
	bool WasClosed { get; }
}