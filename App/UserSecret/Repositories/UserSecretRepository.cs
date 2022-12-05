using L4D2AntiCheat.Infrastructure.Helpers;
using Microsoft.Win32;

namespace L4D2AntiCheat.App.UserSecret.Repositories;

public class UserSecretRepository : IUserSecretRepository
{
    private const string RegistryName = @"SOFTWARE\L4D2AntiCheat\Users";
    private readonly RegistryKey _registryKey = Registry.CurrentUser.OpenSubKey(RegistryName, true)!;

    static UserSecretRepository()
    {
        RegistryHelper.CreateIfDoesNotExist(RegistryName);
    }

    public string? Get(long communityId)
    {
        return _registryKey.GetValue(communityId.ToString())?.ToString();
    }

    public void Set(long communityId, string secret)
    {
        _registryKey.SetValue(communityId.ToString(), secret);
    }

    public void Delete(long communityId)
    {
        _registryKey.DeleteValue(communityId.ToString(), false);
    }
}