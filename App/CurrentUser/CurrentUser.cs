namespace L4D2AntiCheat.App.CurrentUser;

public class CurrentUser : ICurrentUser
{
    private static long? _communityId;
    private static string? _secret;

    public bool Authenticated => CommunityId.HasValue && !string.IsNullOrEmpty(Secret);
    public long? CommunityId => _communityId;
    public string? Secret => _secret;
    public string? AccessToken => Authenticated ? $"{CommunityId}:{Secret}" : null;

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