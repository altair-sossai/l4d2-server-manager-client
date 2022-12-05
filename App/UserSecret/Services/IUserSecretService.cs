namespace L4D2AntiCheat.App.UserSecret.Services;

public interface IUserSecretService
{
    string? GetOrCreatUserSecret(long communityId);
}