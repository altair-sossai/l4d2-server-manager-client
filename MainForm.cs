using System.Diagnostics;
using L4D2AntiCheat.App.CurrentUser;
using L4D2AntiCheat.App.UserSecret.Repositories;
using L4D2AntiCheat.App.UserSecret.Services;
using L4D2AntiCheat.DependencyInjection;
using L4D2AntiCheat.Infrastructure.Helpers;
using L4D2AntiCheat.Sdk.SuspectedPlayer.Results;
using L4D2AntiCheat.Sdk.SuspectedPlayer.Services;
using L4D2AntiCheat.Sdk.SuspectedPlayerPing.Commands;
using L4D2AntiCheat.Sdk.SuspectedPlayerPing.Services;
using L4D2AntiCheat.Sdk.SuspectedPlayerProcess.Commands;
using L4D2AntiCheat.Sdk.SuspectedPlayerProcess.Services;
using L4D2AntiCheat.Sdk.SuspectedPlayerScreenshot.Services;
using L4D2AntiCheat.Sdk.SuspectedPlayerSecret.Commands;
using L4D2AntiCheat.Sdk.SuspectedPlayerSecret.Services;
using L4D2AntiCheat.Sdk.VirtualMachine.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Timer = System.Windows.Forms.Timer;

namespace L4D2AntiCheat;

public partial class MainForm : Form
{
	private static bool _serverTickRunning;
	private static bool _pingTickRunning;
	private static bool _screenTickRunning;
	private static bool _processesTickRunning;

	private readonly Timer _pingTimer = new()
	{
		Enabled = false,
		Interval = 15 * 1000
	};

	private readonly Timer _processesTimer = new()
	{
		Enabled = false,
		Interval = 60 * 1000
	};

	private readonly Timer _screenshotTimer = new()
	{
		Enabled = false,
		Interval = 20 * 1000
	};

	private readonly Timer _serverTimer = new()
	{
		Enabled = true,
		Interval = 30 * 1000
	};

	private readonly ServiceProvider _serviceProvider = ServiceProviderFactory.New();
	private bool _serverIsOn;

	public MainForm()
	{
		InitializeComponent();

		_pingTimer.Tick += (_, _) => PingTick();
		_screenshotTimer.Tick += (_, _) => ScreenshotTick();
		_processesTimer.Tick += (_, _) => ProcessesTick();
		_serverTimer.Tick += (_, _) => ServerTick();
	}

	private ILogger Logger => _serviceProvider.GetRequiredService<ILogger>();
	private ICurrentUser CurrentUser => _serviceProvider.GetRequiredService<ICurrentUser>();
	private IVirtualMachineService VirtualMachineService => _serviceProvider.GetRequiredService<IVirtualMachineService>();
	private IUserSecretService UserSecretService => _serviceProvider.GetRequiredService<IUserSecretService>();
	private IUserSecretRepository UserSecretRepository => _serviceProvider.GetRequiredService<IUserSecretRepository>();
	private ISuspectedPlayerService SuspectedPlayerService => _serviceProvider.GetRequiredService<ISuspectedPlayerService>();
	private ISuspectedPlayerSecretService SuspectedPlayerSecretService => _serviceProvider.GetRequiredService<ISuspectedPlayerSecretService>();
	private ISuspectedPlayerPingService SuspectedPlayerPingService => _serviceProvider.GetRequiredService<ISuspectedPlayerPingService>();
	private ISuspectedPlayerScreenshotService SuspectedPlayerScreenshotService => _serviceProvider.GetRequiredService<ISuspectedPlayerScreenshotService>();
	private ISuspectedPlayerProcessService SuspectedPlayerProcessService => _serviceProvider.GetRequiredService<ISuspectedPlayerProcessService>();

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);

		try
		{
			RefreshSteamAccounts();
			Task.Factory.StartNew(ServerTick);
		}
		catch (Exception exception)
		{
			Logger.Error(exception, nameof(OnLoad));
		}
	}

	private void SteamAccountComboBox_DataSourceChanged(object sender, EventArgs e)
	{
		DisableAllTimers();

		var comboBox = sender as ComboBox;

		if (comboBox?.DataSource is List<SuspectedPlayerResult> { Count: 0 })
			ShowError(@"Nenhuma conta encontrada");
	}

	private void SteamAccountComboBox_SelectedValueChanged(object? sender, EventArgs e)
	{
		DisableAllTimers();

		var comboBox = sender as ComboBox;

		if (comboBox?.SelectedValue is not SuspectedPlayerResult suspectedPlayer)
		{
			CurrentUser.LogOut();

			ShowInfo(@"Selecione uma conta");
			SteamAccountPicture.Image = null;
			return;
		}

		try
		{
			SteamAccountSelected(suspectedPlayer);
		}
		catch (Exception exception)
		{
			Logger.Error(exception, nameof(SteamAccountComboBox_SelectedValueChanged));
		}
	}

	private void RefreshButton_Click(object sender, EventArgs e)
	{
		try
		{
			RefreshSteamAccounts();
		}
		catch (Exception exception)
		{
			Logger.Error(exception, nameof(RefreshButton_Click));
		}
	}

	private void RefreshSteamAccounts()
	{
		DisableAllTimers();

		ShowInfo(@"Atualizando contas...");
		SteamAccountComboBox.SelectedItem = null;
		SteamAccountComboBox.DataSource = SuspectedPlayerService.SteamUsers();
	}

	private void SteamAccountSelected(SuspectedPlayerResult suspectedPlayer)
	{
		ShowInfo(@"Validando conta...");
		SteamAccountPicture.Load(suspectedPlayer.PictureUrl);

		var communityId = long.Parse(suspectedPlayer.CommunityId!);
		var secret = UserSecretService.GetOrCreatUserSecret(communityId);

		if (string.IsNullOrEmpty(secret))
		{
			ShowError(@"Conta já registrada em outro dispositivo");
			return;
		}

		ValidateSecret(suspectedPlayer, secret);
	}

	private void ValidateSecret(SuspectedPlayerResult suspectedPlayer, string secret)
	{
		try
		{
			var command = new ValidateSecretCommand(suspectedPlayer.Steam3, secret);
			var result = SuspectedPlayerSecretService.ValidateAsync(command).Result;

			if (!result.Valid)
			{
				UserSecretRepository.Delete(long.Parse(suspectedPlayer.CommunityId!));
				SecretInvalid();
				return;
			}

			CurrentUser.LogIn(long.Parse(suspectedPlayer.CommunityId!), secret);

			EnableAllTimers();
			ProcessesTick();
		}
		catch (Exception exception)
		{
			Logger.Error(exception, nameof(ValidateSecret));
			SecretInvalid();
		}
	}

	private void SecretInvalid()
	{
		ShowError(@"Não foi possível validar o dispositivo atual");
	}

	private void DisableAllTimers()
	{
		_pingTimer.Enabled = false;
		_screenshotTimer.Enabled = false;
		_processesTimer.Enabled = false;
	}

	private void EnableAllTimers()
	{
		_pingTimer.Enabled = true;
		_screenshotTimer.Enabled = true;
		_processesTimer.Enabled = true;
	}

	private void ServerTick()
	{
		if (_serverTickRunning)
			return;

		try
		{
			_serverTickRunning = true;

			var virtualMachine = VirtualMachineService.InfoAsync().Result;

			_serverIsOn = virtualMachine.IsOn;
		}
		catch (Exception exception)
		{
			Logger.Error(exception, nameof(ServerTick));
			_serverIsOn = false;
		}
		finally
		{
			_serverTickRunning = false;
		}
	}

	private void PingTick()
	{
		if (_pingTickRunning || !AntiCheatIsRunning())
			return;

		try
		{
			_pingTickRunning = true;

			SuspectedPlayerPingService.PingAsync(new PingCommand()).Wait();
		}
		catch (Exception exception)
		{
			Logger.Error(exception, nameof(PingTick));
		}
		finally
		{
			_pingTickRunning = false;
		}
	}

	private void ScreenshotTick()
	{
		if (_screenTickRunning || !AntiCheatIsRunning() || !Left4Dead2ProcessHelper.IsFocused())
			return;

		try
		{
			_screenTickRunning = true;

			var result = SuspectedPlayerScreenshotService.GenerateUploadUrlAsync().Result;
			if (string.IsNullOrEmpty(result.Url))
				return;

			var process = Left4Dead2ProcessHelper.GetProcess();
			if (process == null)
				return;

			using var screenshot = ScreenshotHelper.TakeScreenshot(process);

			SuspectedPlayerScreenshotService.Upload(result.Url, screenshot);
		}
		catch (Exception exception)
		{
			Logger.Error(exception, nameof(ScreenshotTick));
		}
		finally
		{
			_screenTickRunning = false;
		}
	}

	private void ProcessesTick()
	{
		if (_processesTickRunning || !AntiCheatIsRunning())
			return;

		try
		{
			_processesTickRunning = true;

			var commands = Process.GetProcesses()
				.Where(process => process.Id != 0 && process.MainWindowHandle != IntPtr.Zero)
				.Select(process => new ProcessCommand(process))
				.ToList();

			SuspectedPlayerProcessService.AddOrUpdateAsync(commands).Wait();
		}
		catch (Exception exception)
		{
			Logger.Error(exception, nameof(ProcessesTick));
		}
		finally
		{
			_processesTickRunning = false;
		}
	}

	private bool AntiCheatIsRunning()
	{
		try
		{
			if (!_serverIsOn)
			{
				ShowError(@"Servidor desligado");
				return false;
			}

			if (!Left4Dead2ProcessHelper.IsRunning())
			{
				ShowError(@"Left 4 Dead 2 não esta em execução");
				return false;
			}

			ShowSuccess(@"Anti-cheat em execução");

			return true;
		}
		catch (Exception exception)
		{
			Logger.Error(exception, nameof(AntiCheatIsRunning));
			return false;
		}
	}

	private void ShowInfo(string message)
	{
		StatusTextBox.ForeColor = Color.White;
		StatusTextBox.Font = new Font(StatusTextBox.Font, FontStyle.Regular);
		StatusTextBox.Text = message;
	}

	private void ShowError(string message)
	{
		StatusTextBox.ForeColor = Color.Red;
		StatusTextBox.Font = new Font(StatusTextBox.Font, FontStyle.Bold);
		StatusTextBox.Text = message;
	}

	private void ShowSuccess(string message)
	{
		StatusTextBox.ForeColor = Color.LimeGreen;
		StatusTextBox.Font = new Font(StatusTextBox.Font, FontStyle.Regular);
		StatusTextBox.Text = message;
	}
}