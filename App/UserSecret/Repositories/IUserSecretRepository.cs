namespace L4D2AntiCheat.App.UserSecret.Repositories;

public interface IUserSecretRepository
{
    string? Get(long communityId);
    void Set(long communityId, string secret);
    void Delete(long communityId);
}