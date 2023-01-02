using System.Diagnostics;
using L4D2AntiCheat.DependencyInjection;
using L4D2AntiCheat.Forms;
using L4D2AntiCheat.Infrastructure.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace L4D2AntiCheat;

internal static class Program
{
	static Program()
	{
#if DEBUG
		var processes = Process.GetProcessesByName("steam");
		var process = processes.FirstOrDefault();
		if (process != null)
			SteamProcessHelper.SetCurrentProcess(process);
#endif
	}

	[STAThread]
	private static void Main()
	{
		ApplicationConfiguration.Initialize();

		using var serviceProvider = ServiceProviderFactory.New();

		AppDomain.CurrentDomain.UnhandledException += UnhandledException;

		Form form = OptInHelper.Accepted() ? serviceProvider.GetRequiredService<StartupForm>() : serviceProvider.GetRequiredService<OptInForm>();

#if DEBUG
		form = serviceProvider.GetRequiredService<StartupForm>();
#endif

		Application.Run(form);
	}

	private static void UnhandledException(object sender, UnhandledExceptionEventArgs args)
	{
		var exception = (Exception)args.ExceptionObject;
		Log.Logger.Error(exception, nameof(UnhandledException));
	}
}