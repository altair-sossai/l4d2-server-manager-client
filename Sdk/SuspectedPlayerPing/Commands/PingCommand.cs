using L4D2AntiCheat.Infrastructure.Helpers;

namespace L4D2AntiCheat.Sdk.SuspectedPlayerPing.Commands;

public class PingCommand
{
    public PingCommand()
    {
        Focused = Left4Dead2ProcessHelper.IsFocused();
    }

    public bool Focused { get; }
}