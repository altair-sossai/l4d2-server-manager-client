using L4D2AntiCheat.Infrastructure.Helpers;

namespace L4D2AntiCheat.Sdk.SuspectedPlayerPing.Commands;

public class PingCommand
{
    public PingCommand()
    {
        Focused = ProcessHelper.Left4Dead2IsFocused();
    }

    public bool Focused { get; }
}