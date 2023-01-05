using L4D2AntiCheat.Context;
using Serilog;

namespace L4D2AntiCheat.Tasks.Infrastructure;

public abstract class IntervalTask : IIntervalTask
{
	private readonly TimeSpan _interval;
	private readonly object _lock = new();
	private DateTime _lastRun = DateTime.MinValue;
	private bool _running;

	protected IntervalTask(TimeSpan interval)
	{
		_interval = interval;
	}

	private bool Expired => DateTime.Now > _lastRun.Add(_interval);

	public void Execute(AntiCheatContext context)
	{
		if (_running || !Expired || !CanRun(context))
			return;

		try
		{
			_running = true;

			lock (_lock)
			{
				Run(context);
			}
		}
		catch (Exception exception)
		{
			Log.Logger.Error(exception, nameof(Execute));
		}
		finally
		{
			_running = false;
			_lastRun = DateTime.Now;
		}
	}

	protected virtual bool CanRun(AntiCheatContext context)
	{
		return true;
	}

	protected abstract void Run(AntiCheatContext context);
}