using L4D2AntiCheat.Context;
using L4D2AntiCheat.ProcessInfo;
using L4D2AntiCheat.Tasks.Infrastructure;

namespace L4D2AntiCheat.Tasks;

public class SteamWasClosedTask : IntervalTask
{
	private readonly ISteamProcessInfo _processInfo;

	public SteamWasClosedTask(ISteamProcessInfo processInfo)
		: base(TimeSpan.FromSeconds(30))
	{
		_processInfo = processInfo;
	}

	protected override bool CanRun(AntiCheatContext context)
	{
		return !context.SteamWasClosed && _processInfo.IsRunning;
	}

	protected override void Run(AntiCheatContext context)
	{
		context.SteamWasClosed = _processInfo.WasClosed;
	}
}