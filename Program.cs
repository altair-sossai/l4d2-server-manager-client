using L4D2AntiCheat.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace L4D2AntiCheat;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        AppDomain.CurrentDomain.UnhandledException += UnhandledException;

        using var serviceProvider = ServiceProviderFactory.New();
        var mainForm = serviceProvider.GetRequiredService<MainForm>();

        Application.Run(mainForm);
    }

    private static void UnhandledException(object sender, UnhandledExceptionEventArgs args)
    {
        var exception = (Exception)args.ExceptionObject;
        Console.WriteLine(exception.Message);
    }
}