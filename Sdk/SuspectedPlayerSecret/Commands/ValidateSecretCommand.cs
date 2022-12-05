namespace L4D2AntiCheat.Sdk.SuspectedPlayerSecret.Commands;

public class ValidateSecretCommand
{
    public ValidateSecretCommand(string? steam3, string? secret)
    {
        Steam3 = steam3;
        Secret = secret;
    }

    public string? Steam3 { get; }
    public string? Secret { get; }
}