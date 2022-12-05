using L4D2AntiCheat.App.UserSecret.Repositories;
using L4D2AntiCheat.App.UserSecret.Services;
using L4D2AntiCheat.DependencyInjection;
using L4D2AntiCheat.Infrastructure.Helpers;
using L4D2AntiCheat.Sdk.SuspectedPlayer.Results;
using L4D2AntiCheat.Sdk.SuspectedPlayer.Services;
using L4D2AntiCheat.Sdk.SuspectedPlayerSecret.Commands;
using L4D2AntiCheat.Sdk.SuspectedPlayerSecret.Services;
using L4D2AntiCheat.Sdk.VirtualMachine.Services;
using Microsoft.Extensions.DependencyInjection;
using Timer = System.Windows.Forms.Timer;

namespace L4D2AntiCheat;

public partial class MainForm : Form
{
    private readonly ServiceProvider _serviceProvider = ServiceProviderFactory.New();

    private readonly Timer _timer = new()
    {
        Enabled = false,
        Interval = 10 * 1000
    };

    public MainForm()
    {
        InitializeComponent();

        _timer.Tick += Timer_Tick;
    }

    private ISuspectedPlayerService SuspectedPlayerService => _serviceProvider.GetRequiredService<ISuspectedPlayerService>();
    private ISuspectedPlayerSecretService SuspectedPlayerSecretService => _serviceProvider.GetRequiredService<ISuspectedPlayerSecretService>();
    private IUserSecretService UserSecretService => _serviceProvider.GetRequiredService<IUserSecretService>();
    private IUserSecretRepository UserSecretRepository => _serviceProvider.GetRequiredService<IUserSecretRepository>();
    private IVirtualMachineService VirtualMachineService => _serviceProvider.GetRequiredService<IVirtualMachineService>();

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        RefreshSteamAccounts();
    }

    private void SteamAccountComboBox_DataSourceChanged(object sender, EventArgs e)
    {
        var comboBox = sender as ComboBox;

        if (comboBox?.DataSource is List<SuspectedPlayerResult> { Count: 0 })
            SteamAccountEmpty();
    }

    private void SteamAccountComboBox_SelectedValueChanged(object? sender, EventArgs e)
    {
        var comboBox = sender as ComboBox;

        if (comboBox?.SelectedValue is not SuspectedPlayerResult suspectedPlayer)
        {
            ClearSteamAccount();
            return;
        }

        SteamAccountSelected(suspectedPlayer);
    }

    private void RefreshButton_Click(object sender, EventArgs e)
    {
        RefreshSteamAccounts();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        AntiCheatTick();
    }

    private void RefreshSteamAccounts()
    {
        _timer.Enabled = false;

        StatusTextBox.Text = @"Atualizando contas...";
        SteamAccountComboBox.SelectedItem = null;
        SteamAccountComboBox.DataSource = SuspectedPlayerService.SteamUsers();
    }

    private void SteamAccountEmpty()
    {
        _timer.Enabled = false;

        StatusTextBox.Text = @"Nenhuma conta encontrada";
    }

    private void ClearSteamAccount()
    {
        _timer.Enabled = false;

        StatusTextBox.Text = @"Selecione uma conta";
        SteamAccountPicture.Image = null;
    }

    private void SteamAccountSelected(SuspectedPlayerResult suspectedPlayer)
    {
        StatusTextBox.Text = @"Validando conta...";
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
        StatusTextBox.Text = @"Conta já registrado em outro dispositivo";
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

            CheckConnectionWithTheServer();
        }
        catch (Exception)
        {
            SecretInvalid();
        }
    }

    private void SecretInvalid()
    {
        StatusTextBox.Text = @"Não foi possível validar o dispositivo atual";
    }

    private void CheckConnectionWithTheServer()
    {
        StatusTextBox.Text = @"Verificando conexão com o servidor...";

        try
        {
            var virtualMachine = VirtualMachineService.InfoAsync().Result;
            if (virtualMachine.IsOff)
            {
                ServerOff();
                return;
            }

            AllReady();
        }
        catch (Exception)
        {
            ServerOff();
        }
    }

    private void ServerOff()
    {
        StatusTextBox.Text = @"Servidor desligado";
    }

    private void AllReady()
    {
        AntiCheatTick();

        _timer.Enabled = true;
    }

    private void AntiCheatTick()
    {
        if (!ProcessHelper.Left4Dead2IsRunning())
        {
            Left4Dead2IsNotRunning();
            return;
        }
    }

    private void Left4Dead2IsNotRunning()
    {
        StatusTextBox.Text = @"Left 4 Dead 2 não esta em execução";
    }
}