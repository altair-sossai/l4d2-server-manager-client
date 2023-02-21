using L4D2AntiCheat.DependencyInjection;
using L4D2AntiCheat.Modules.OptIn;
using Microsoft.Extensions.DependencyInjection;

namespace L4D2AntiCheat.Forms;

public partial class OptInForm : Form
{
    public OptInForm()
    {
        InitializeComponent();
    }

    private void NoButton_Click(object sender, EventArgs e)
    {
        Application.Exit();
    }

    private void YesButton_Click(object sender, EventArgs e)
    {
        OptIn.Accept();

        using var serviceProvider = ServiceProviderFactory.New();
        var startupForm = serviceProvider.GetRequiredService<StartupForm>();
        startupForm.Closed += (_, _) => Application.Exit();

        Hide();

        startupForm.ShowDialog(this);
    }
}