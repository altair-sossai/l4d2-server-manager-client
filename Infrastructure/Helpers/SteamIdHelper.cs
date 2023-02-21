using System.Text.RegularExpressions;

namespace L4D2AntiCheat.Infrastructure.Helpers;

public static class SteamIdHelper
{
    private const string Steam3Pattern = @"^\[?U:\d:(\d+)]?$";

    private const long MagicNumber = 76561197960265728;

    private static readonly Regex Steam3Regex = new(Steam3Pattern);

    public static long? UserToCommunityId(string user)
    {
        var steam3 = $"[U:1:{user}]";
        var communityId = Steam3ToCommunityId(steam3);

        return communityId;
    }

    public static string? CommunityIdToSteam3(long communityId)
    {
        if (communityId <= 0)
            return null;

        var authserver = communityId - MagicNumber;

        return $"[U:1:{authserver}]";
    }

    private static long? Steam3ToCommunityId(string value)
    {
        var match = Steam3Regex.Match(value);

        if (!match.Success)
            return null;

        var steam3 = long.Parse(match.Groups[1].Value);

        return steam3 + MagicNumber;
    }
}