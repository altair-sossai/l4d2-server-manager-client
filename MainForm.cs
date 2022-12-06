using L4D2AntiCheat.App.CurrentUser;
using L4D2AntiCheat.App.UserSecret.Repositories;
using L4D2AntiCheat.App.UserSecret.Services;
using L4D2AntiCheat.DependencyInjection;
using L4D2AntiCheat.Infrastructure.Helpers;
using L4D2AntiCheat.Sdk.SuspectedPlayer.Results;
using L4D2AntiCheat.Sdk.SuspectedPlayer.Services;
using L4D2AntiCheat.Sdk.SuspectedPlayerPing.Commands;
using L4D2AntiCheat.Sdk.SuspectedPlayerPing.Services;
using L4D2AntiCheat.Sdk.SuspectedPlayerSecret.Commands;
using L4D2AntiCheat.Sdk.SuspectedPlayerSecret.Services;
using L4D2AntiCheat.Sdk.VirtualMachine.Services;
using Microsoft.Extensions.DependencyInjection;
using Timer = System.Windows.Forms.Timer;

namespace L4D2AntiCheat;

public partial class MainForm : Form
{
    private readonly Timer _pingTimer = new()
    {
        Enabled = false,
        Interval = 5 * 1000
    };

    private readonly Timer _serverTimer = new()
    {
        Enabled = true,
        Interval = 20 * 1000
    };

    private readonly ServiceProvider _serviceProvider = ServiceProviderFactory.New();
    private bool _serverIsOn;

    public MainForm()
    {
        InitializeComponent();

        _pingTimer.Tick += (_, _) => PingTick();
        _serverTimer.Tick += (_, _) => ServerTick();
    }

    private ISuspectedPlayerService SuspectedPlayerService => _serviceProvider.GetRequiredService<ISuspectedPlayerService>();
    private ISuspectedPlayerSecretService SuspectedPlayerSecretService => _serviceProvider.GetRequiredService<ISuspectedPlayerSecretService>();
    private ISuspectedPlayerPingService SuspectedPlayerPingService => _serviceProvider.GetRequiredService<ISuspectedPlayerPingService>();
    private IUserSecretService UserSecretService => _serviceProvider.GetRequiredService<IUserSecretService>();
    private IUserSecretRepository UserSecretRepository => _serviceProvider.GetRequiredService<IUserSecretRepository>();
    private IVirtualMachineService VirtualMachineService => _serviceProvider.GetRequiredService<IVirtualMachineService>();
    private ICurrentUser CurrentUser => _serviceProvider.GetRequiredService<ICurrentUser>();

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        RefreshSteamAccounts();

        Task.Factory.StartNew(ServerTick);
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

        SteamAccountSelected(suspectedPlayer);
    }

    private void RefreshButton_Click(object sender, EventArgs e)
    {
        RefreshSteamAccounts();
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
            UserAlreadyRegistered();
            return;
        }

        ValidateSecret(suspectedPlayer, secret);
    }

    private void UserAlreadyRegistered()
    {
        ShowError(@"Conta já registrado em outro dispositivo");
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
        }
        catch (Exception)
        {
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
    }

    private void EnableAllTimers()
    {
        _pingTimer.Enabled = true;
    }

    private void ServerTick()
    {
        try
        {
            var virtualMachine = VirtualMachineService.InfoAsync().Result;
            _serverIsOn = virtualMachine.IsOn;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            _serverIsOn = false;
        }
    }

    private void PingTick()
    {
        if (!ServerIsOnAndLeft4Dead2IsRunning())
            return;

        SuspectedPlayerPingService.PingAsync(new PingCommand()).Wait();
    }

    private bool ServerIsOnAndLeft4Dead2IsRunning()
    {
        if (!_serverIsOn)
        {
            ShowError(@"Servidor desligado");
            return false;
        }

        if (!ProcessHelper.Left4Dead2IsRunning())
        {
            ShowError(@"Left 4 Dead 2 não esta em execução");
            return false;
        }

        ShowSuccess(@"Anti-cheat em execução");

        return true;
    }

    private void ShowInfo(string message)
    {
        StatusTextBox.ForeColor = Color.White;
        StatusTextBox.Text = message;
    }

    private void ShowError(string message)
    {
        StatusTextBox.ForeColor = Color.Red;
        StatusTextBox.Text = message;
    }

    private void ShowSuccess(string message)
    {
        StatusTextBox.ForeColor = Color.LimeGreen;
        StatusTextBox.Text = message;
    }
}