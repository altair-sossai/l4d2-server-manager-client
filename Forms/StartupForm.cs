using System.Diagnostics;
using L4D2AntiCheat.DependencyInjection;
using L4D2AntiCheat.Infrastructure.Helpers;
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
		if (Left4Dead2ProcessHelper.IsRunning())
		{
			MessageBox.Show(@"Feche o jogo (Left 4 Dead 2) e tenta outra vez", @"Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Error);
			return;
		}

		if (SteamProcessHelper.IsRunning())
		{
			MessageBox.Show(@"Feche a Steam e tenta outra vez", @"Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Error);
			return;
		}

		var steamExe = RegistryHelper.SteamExe();
		if (string.IsNullOrEmpty(steamExe))
		{
			MessageBox.Show(@"Os arquivos da Steam não foram localizados", @"Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Error);
			return;
		}

		var fileInfo = new FileInfo(steamExe);
		var processStartInfo = new ProcessStartInfo
		{
			FileName = fileInfo.FullName,
			WorkingDirectory = fileInfo.DirectoryName!
		};

		var process = Process.Start(processStartInfo);
		if (process == null)
		{
			MessageBox.Show(@"Não foi possivel iniciar a Steam", @"Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Error);
			return;
		}

		SteamProcessHelper.SetCurrentProcess(process);

		using var serviceProvider = ServiceProviderFactory.New();
		var mainForm = serviceProvider.GetRequiredService<MainForm>();
		mainForm.Closed += (_, _) => Application.Exit();

		Hide();

		mainForm.ShowDialog(this);
	}
}