using Microsoft.Win32;

namespace L4D2AntiCheat.Infrastructure.Helpers;

public static class RegistryHelper
{
    public static IEnumerable<long> SteamUsers()
    {
        var currentUser = Registry.CurrentUser;
        var registryKey = currentUser.OpenSubKey(@"SOFTWARE\Valve\Steam\Users");
        if (registryKey == null)
            return new List<long>();

        var keys = registryKey.GetSubKeyNames();
        var identifiers = keys
            .Select(key => $"[U:1:{key}]")
            .Select(SteamIdHelper.Steam3ToCommunityId)
            .Where(w => w.HasValue)
            .Cast<long>();

        return identifiers;
    }

    public static void CreateIfDoesNotExist(string fullName)
    {
        var registry = Registry.CurrentUser;

        foreach (var name in fullName.Split('\\', StringSplitOptions.RemoveEmptyEntries))
        {
            var subkey = registry.OpenSubKey(name, true);
            if (subkey != null)
            {
                registry = subkey;
                continue;
            }

            registry = registry.CreateSubKey(name, RegistryKeyPermissionCheck.Default);
        }
    }
}