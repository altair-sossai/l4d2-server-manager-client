using L4D2AntiCheat.DependencyInjection;
using L4D2AntiCheat.Forms;
using L4D2AntiCheat.Infrastructure.Helpers;
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

		Form form = OptInHelper.Accepted() ? serviceProvider.GetRequiredService<StartupForm>() : serviceProvider.GetRequiredService<OptInForm>();

		AppDomain.CurrentDomain.UnhandledException += UnhandledException;

		Application.Run(form);
	}

	private static void UnhandledException(object sender, UnhandledExceptionEventArgs args)
	{
		var exception = (Exception)args.ExceptionObject;
		Log.Logger.Error(exception, nameof(UnhandledException));
	}
}