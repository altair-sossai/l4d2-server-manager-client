namespace L4D2AntiCheat.Sdk.SuspectedPlayerPing.Commands;

public class PingCommand
{
    public PingCommand(bool focused)
    {
        Focused = focused;
    }

    public bool Focused { get; }
}