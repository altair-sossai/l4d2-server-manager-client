namespace L4D2AntiCheat.App.CurrentUser;

public interface ICurrentUser
{
    string? AccessToken { get; }
    void LogIn(long communityId, string secret);
    void LogOut();
}