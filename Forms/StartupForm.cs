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
			MessageBox.Show(@"Por favor, para prosseguir feche o jogo (Left 4 Dead 2)", @"Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Error);
			return;
		}

		if (SteamProcessHelper.IsRunning())
		{
			MessageBox.Show(@"Por favor, para prosseguir feche a Steam", @"Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
			WorkingDirectory = fileInfo.DirectoryName!,
			Arguments = "-applaunch 550"
		};

		var process = Process.Start(processStartInfo);
		if (process == null)
		{
			MessageBox.Show(@"Não foi possivel iniciar a Steam", @"Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Error);
			return;
		}

		SteamProcessHelper.SetCurrentProcess(process);
		Thread.Sleep(3000);

		if (!Left4Dead2ProcessHelper.CatchCurrentProcess())
		{
			SteamProcessHelper.Clear();
			Left4Dead2ProcessHelper.Clear();

			MessageBox.Show(@"Falha ao iniciar o jogo, tente novamente", @"Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Error);
			return;
		}

		using var serviceProvider = ServiceProviderFactory.New();
		var mainForm = serviceProvider.GetRequiredService<MainForm>();
		mainForm.Closed += (_, _) => Application.Exit();

		Hide();

		mainForm.ShowDialog(this);
	}
}