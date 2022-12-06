namespace L4D2AntiCheat.App.CurrentUser;

public interface ICurrentUser
{
    bool Authenticated { get; }
    long? CommunityId { get; }
    string? Secret { get; }
    string? AccessToken { get; }

    void LogIn(long communityId, string secret);
    void LogOut();
}