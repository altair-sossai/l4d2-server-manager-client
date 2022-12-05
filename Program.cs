using L4D2AntiCheat.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace L4D2AntiCheat;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        using var serviceProvider = ServiceProviderFactory.New();
        var mainForm = serviceProvider.GetRequiredService<MainForm>();

        Application.Run(mainForm);
    }
}