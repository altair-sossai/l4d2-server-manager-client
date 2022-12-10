namespace L4D2AntiCheat.App.CurrentUser;

public class CurrentUser : ICurrentUser
{
    private static long? _communityId;
    private static string? _secret;

    private static bool Authenticated => _communityId.HasValue && !string.IsNullOrEmpty(_secret);
    public string? AccessToken => Authenticated ? $"{_communityId}:{_secret}" : null;

    public void LogIn(long communityId, string secret)
    {
        _communityId = communityId;
        _secret = secret;
    }

    public void LogOut()
    {
        _communityId = null;
    }
}