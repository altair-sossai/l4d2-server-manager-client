using System.Diagnostics;
using L4D2AntiCheat.DependencyInjection;
using L4D2AntiCheat.Forms;
using L4D2AntiCheat.Modules.OptIn;
using L4D2AntiCheat.ProcessInfo;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace L4D2AntiCheat;

internal static class Program
{
	[STAThread]
	private static void Main()
	{
		ApplicationConfiguration.Initialize();

		using var serviceProvider = ServiceProviderFactory.New();

#if DEBUG
		AttachSteamProcess(serviceProvider);
		AttachLeft4Dead2Process(serviceProvider);
#endif

		AppDomain.CurrentDomain.UnhandledException += UnhandledException;

		Form form = OptIn.Accepted ? serviceProvider.GetRequiredService<StartupForm>() : serviceProvider.GetRequiredService<OptInForm>();

#if DEBUG
		form = serviceProvider.GetRequiredService<MainForm>();
#endif

		Application.Run(form);
	}

	private static void AttachSteamProcess(IServiceProvider serviceProvider)
	{
		var processes = Process.GetProcessesByName("steam");
		var process = processes.FirstOrDefault();
		if (process == null)
			return;

		var processInfo = serviceProvider.GetRequiredService<ISteamProcessInfo>();

		processInfo.CurrentProcess = process;
	}

	private static void AttachLeft4Dead2Process(IServiceProvider serviceProvider)
	{
		var processes = Process.GetProcessesByName("left4dead2");
		var process = processes.FirstOrDefault();
		if (process == null)
			return;

		var processInfo = serviceProvider.GetRequiredService<ILeft4Dead2ProcessInfo>();

		processInfo.CurrentProcess = process;
	}

	private static void UnhandledException(object sender, UnhandledExceptionEventArgs args)
	{
		var exception = (Exception)args.ExceptionObject;
		Log.Logger.Error(exception, nameof(UnhandledException));
	}
}