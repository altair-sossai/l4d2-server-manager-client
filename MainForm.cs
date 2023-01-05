using L4D2AntiCheat.App.CurrentUser;
using L4D2AntiCheat.App.UserSecret.Repositories;
using L4D2AntiCheat.App.UserSecret.Services;
using L4D2AntiCheat.Context;
using L4D2AntiCheat.DependencyInjection;
using L4D2AntiCheat.Modules.Player.Services;
using L4D2AntiCheat.Sdk.SuspectedPlayer.Results;
using L4D2AntiCheat.Sdk.SuspectedPlayerSecret.Commands;
using L4D2AntiCheat.Sdk.SuspectedPlayerSecret.Services;
using L4D2AntiCheat.Tasks;
using L4D2AntiCheat.Tasks.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Timer = System.Windows.Forms.Timer;

namespace L4D2AntiCheat;

public partial class MainForm : Form
{
	private readonly AntiCheatContext _context = new();
	private readonly ServiceProvider _serviceProvider = ServiceProviderFactory.New();

	private readonly Timer _tasksTimer = new()
	{
		Interval = 1 * 1000
	};

	private bool _tasksRunning;

	public MainForm()
	{
		InitializeComponent();

		_tasksTimer.Tick += (_, _) => TasksTick();
		_tasksTimer.Enabled = true;
	}

	private ILogger Logger => _serviceProvider.GetRequiredService<ILogger>();
	private ICurrentUser CurrentUser => _serviceProvider.GetRequiredService<ICurrentUser>();
	private IUserSecretService UserSecretService => _serviceProvider.GetRequiredService<IUserSecretService>();
	private IUserSecretRepository UserSecretRepository => _serviceProvider.GetRequiredService<IUserSecretRepository>();
	private ISuspectedPlayerSecretService SuspectedPlayerSecretService => _serviceProvider.GetRequiredService<ISuspectedPlayerSecretService>();
	private IPlayerService PlayerService => _serviceProvider.GetRequiredService<IPlayerService>();

	private IEnumerable<IIntervalTask> Tasks => new IIntervalTask[]
	{
		_serviceProvider.GetRequiredService<ServerPingTask>(),
		_serviceProvider.GetRequiredService<FileConsistencyTask>(),
		_serviceProvider.GetRequiredService<SteamWasClosedTask>(),
		_serviceProvider.GetRequiredService<Left4Dead2WasClosedTask>(),
		_serviceProvider.GetRequiredService<PingTask>(),
		_serviceProvider.GetRequiredService<ScreenshotTask>(),
		_serviceProvider.GetRequiredService<ProcessesTask>()
	};

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);

		try
		{
			RefreshSteamAccounts();
		}
		catch (Exception exception)
		{
			Logger.Error(exception, nameof(OnLoad));
		}
	}

	private void SteamAccountComboBox_DataSourceChanged(object sender, EventArgs e)
	{
		var comboBox = sender as ComboBox;

		if (comboBox?.DataSource is List<SuspectedPlayerResult> { Count: 0 })
			ShowError(@"Nenhuma conta encontrada", "O dispositivo atual não possui conta Steam registrada como suspeita, clique em 'Atualizar' para tentar novamente");
	}

	private void SteamAccountComboBox_SelectedValueChanged(object? sender, EventArgs e)
	{
		_context.Clear();

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
			ShowError("Erro ao validar a conta", "Clique em 'Atualizar' para tentar novamente");
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
		_context.Clear();

		ShowInfo(@"Atualizando contas...");
		SteamAccountComboBox.SelectedItem = null;
		SteamAccountComboBox.DataSource = PlayerService.Accounts();
	}

	private void SteamAccountSelected(SuspectedPlayerResult suspectedPlayer)
	{
		ShowInfo(@"Validando conta, aguarde...");
		SteamAccountPicture.Load(suspectedPlayer.PictureUrl);

		var communityId = long.Parse(suspectedPlayer.CommunityId!);
		var secret = UserSecretService.GetOrCreatUserSecret(communityId);

		if (string.IsNullOrEmpty(secret))
		{
			ShowError(@"Conta já registrada em outro dispositivo", "A conta atual já está registrada em outro dispositivo, solicite ao administrador que resete seus dados de cadastro e tente novamente");
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

			_context.SuspectedPlayer = suspectedPlayer;
		}
		catch (Exception exception)
		{
			Logger.Error(exception, nameof(ValidateSecret));
			SecretInvalid();
		}
	}

	private void SecretInvalid()
	{
		ShowError(@"Não foi possível validar o dispositivo atual", "Clique em 'Atualizar' para tentar novamente");
	}

	private void TasksTick()
	{
		if (_tasksRunning || _context.SuspectedPlayer == null)
			return;

		if (_context.InconsistentFiles || _context.SteamWasClosed || _context.Left4Dead2WasClosed)
		{
			ShowMessage();
			return;
		}

		try
		{
			_tasksRunning = true;

			foreach (var task in Tasks)
				task.Execute(_context);

			ShowMessage();
		}
		catch (Exception exception)
		{
			Logger.Error(exception, nameof(TasksTick));
		}
		finally
		{
			_tasksRunning = false;
		}
	}

	private void ShowMessage()
	{
		if (!_context.ServerIsOn)
		{
			ShowError(@"Servidor desligado");
			return;
		}

		if (_context.InconsistentFiles)
		{
			ShowError(@"Os arquivos do jogo foram modificados", "Feche o jogo, restaure os arquivos do game e tente novamente");
			return;
		}

		if (_context.SteamWasClosed)
		{
			ShowError(@"A Steam foi fechada", "Por favor, feche o jogo (Left 4 Dead 2), feche a Steam, feche o Anti-cheat e inicie tudo outra vez");
			return;
		}

		if (_context.Left4Dead2WasClosed)
		{
			ShowError(@"Left 4 Dead 2 foi fechado", "Por favor, feche o jogo (Left 4 Dead 2), feche a Steam, feche o Anti-cheat e inicie tudo outra vez");
			return;
		}

		ShowSuccess(@"Anti-cheat em execução");
	}

	private void ShowInfo(string message, string? details = null)
	{
		StatusTextBox.ForeColor = Color.White;
		StatusTextBox.Font = new Font(StatusTextBox.Font, FontStyle.Regular);
		StatusTextBox.Text = message;
		DetailsLabel.Text = details;
	}

	private void ShowError(string message, string? details = null)
	{
		StatusTextBox.ForeColor = Color.Red;
		StatusTextBox.Font = new Font(StatusTextBox.Font, FontStyle.Bold);
		StatusTextBox.Text = message;
		DetailsLabel.Text = details;
	}

	private void ShowSuccess(string message, string? details = null)
	{
		StatusTextBox.ForeColor = Color.LimeGreen;
		StatusTextBox.Font = new Font(StatusTextBox.Font, FontStyle.Regular);
		StatusTextBox.Text = message;
		DetailsLabel.Text = details;
	}
}