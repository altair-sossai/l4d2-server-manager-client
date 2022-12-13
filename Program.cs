using L4D2AntiCheat.DependencyInjection;
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
		var mainForm = serviceProvider.GetRequiredService<MainForm>();

		AppDomain.CurrentDomain.UnhandledException += UnhandledException;

		Application.Run(mainForm);
	}

	private static void UnhandledException(object sender, UnhandledExceptionEventArgs args)
	{
		var exception = (Exception)args.ExceptionObject;
		Log.Logger.Error(exception, nameof(UnhandledException));
	}
}