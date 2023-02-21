using L4D2AntiCheat.Modules.Steam;
using L4D2AntiCheat.Sdk.SuspectedPlayer.Results;
using L4D2AntiCheat.Sdk.SuspectedPlayer.Services;

namespace L4D2AntiCheat.Modules.Player.Services;

public class PlayerService : IPlayerService
{
    private readonly ISteamInfo _steamInfo;
    private readonly ISuspectedPlayerService _suspectedPlayerService;

    public PlayerService(ISuspectedPlayerService suspectedPlayerService,
        ISteamInfo steamInfo)
    {
        _suspectedPlayerService = suspectedPlayerService;
        _steamInfo = steamInfo;
    }

    public List<SuspectedPlayerResult> Accounts()
    {
        var accounts = _steamInfo.Accounts;
        var suspecteds = accounts
            .Select(TryFind)
            .Where(suspectedPlayer => suspectedPlayer != null)
            .Cast<SuspectedPlayerResult>()
            .ToList();

        return suspecteds;
    }

    private SuspectedPlayerResult? TryFind(long communityId)
    {
        try
        {
            return _suspectedPlayerService.Find(communityId).Result;
        }
        catch (Exception)
        {
            return null;
        }
    }
}