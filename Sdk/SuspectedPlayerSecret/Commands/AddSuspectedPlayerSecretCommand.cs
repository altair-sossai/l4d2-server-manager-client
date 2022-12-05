namespace L4D2AntiCheat.Sdk.SuspectedPlayerSecret.Commands;

public class AddSuspectedPlayerSecretCommand
{
    public AddSuspectedPlayerSecretCommand(string? steam3)
    {
        Steam3 = steam3;
    }

    public string? Steam3 { get; }
}