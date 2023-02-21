using L4D2AntiCheat.DependencyInjection;
using L4D2AntiCheat.Modules.Steam;
using L4D2AntiCheat.ProcessInfo;
using Microsoft.Extensions.DependencyInjection;

namespace L4D2AntiCheat.Forms;

public partial class StartupForm : Form
{
    public StartupForm()
    {
        InitializeComponent();
    }

    private void OpenSteamButton_Click(object sender, EventArgs e)
    {
        using var serviceProvider = ServiceProviderFactory.New();

        var steamProcessInfo = serviceProvider.GetRequiredService<ISteamProcessInfo>();
        if (steamProcessInfo.IsRunning)
        {
            MessageBox.Show(@"Por favor, para prosseguir feche a Steam", @"Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var left4Dead2ProcessInfo = serviceProvider.GetRequiredService<ILeft4Dead2ProcessInfo>();
        if (left4Dead2ProcessInfo.IsRunning)
        {
            MessageBox.Show(@"Por favor, para prosseguir feche o jogo (Left 4 Dead 2)", @"Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var steamInfo = serviceProvider.GetRequiredService<ISteamInfo>();
        if (string.IsNullOrEmpty(steamInfo.SteamPath))
        {
            MessageBox.Show(@"Os arquivos da Steam não foram localizados", @"Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var process = steamProcessInfo.Start(steamInfo.SteamPath);
        if (process == null)
        {
            MessageBox.Show(@"Não foi possivel iniciar a Steam", @"Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (!left4Dead2ProcessInfo.AttachProcess())
        {
            steamProcessInfo.CurrentProcess = null;
            left4Dead2ProcessInfo.CurrentProcess = null;

            MessageBox.Show(@"Falha ao iniciar o jogo, tente novamente", @"Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var mainForm = serviceProvider.GetRequiredService<MainForm>();
        mainForm.Closed += (_, _) => Application.Exit();

        Hide();

        mainForm.ShowDialog(this);
    }
}