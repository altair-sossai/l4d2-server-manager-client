namespace L4D2AntiCheat.Modules.Steam;

public interface ISteamInfo
{
    string? SteamPath { get; }
    IEnumerable<long> Accounts { get; }
}