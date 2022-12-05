using L4D2AntiCheat.Infrastructure.Helpers;
using L4D2AntiCheat.Sdk.SuspectedPlayer.Results;
using Refit;

namespace L4D2AntiCheat.Sdk.SuspectedPlayer.Services;

public interface ISuspectedPlayerService
{
    [Get("/api/suspected-players/{communityId}")]
    Task<SuspectedPlayerResult> Find(long communityId);

    List<SuspectedPlayerResult> SteamUsers()
    {
        var steamUsers = RegistryHelper.SteamUsers();
        var suspecteds = steamUsers
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
            return Find(communityId).Result;
        }
        catch (Exception)
        {
            return null;
        }
    }
}