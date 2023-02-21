using L4D2AntiCheat.Sdk.SuspectedPlayer.Results;

namespace L4D2AntiCheat.Context;

public class AntiCheatContext
{
    public SuspectedPlayerResult? SuspectedPlayer { get; set; }
    public bool ServerIsOn { get; set; }
    public bool InconsistentFiles { get; set; }
    public bool SteamWasClosed { get; set; }
    public bool Left4Dead2WasClosed { get; set; }

    public void Clear()
    {
        SuspectedPlayer = null;
    }
}