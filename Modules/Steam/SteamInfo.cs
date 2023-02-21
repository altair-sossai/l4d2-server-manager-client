using L4D2AntiCheat.Infrastructure.Helpers;
using Microsoft.Win32;

namespace L4D2AntiCheat.Modules.Steam;

public class SteamInfo : ISteamInfo
{
    private static readonly RegistryKey? SteamRegistryKey;
    private static readonly RegistryKey? UsersRegistryKey;
    private static readonly RegistryKey? ActiveProcessRegistryKey;
    private static string? _steamExePath;

    static SteamInfo()
    {
        var currentUser = Registry.CurrentUser;

        SteamRegistryKey = currentUser.OpenSubKey(@"SOFTWARE\Valve\Steam");
        UsersRegistryKey = currentUser.OpenSubKey(@"SOFTWARE\Valve\Steam\Users");
        ActiveProcessRegistryKey = currentUser.OpenSubKey(@"SOFTWARE\Valve\Steam\ActiveProcess");
    }

    public string? SteamPath => _steamExePath ??= SteamRegistryKey?.GetValue("SteamExe")?.ToString()?.Trim();

    public IEnumerable<long> Accounts
    {
        get
        {
            var accounts = new HashSet<long>();

            foreach (var user in Users())
                accounts.Add(user);

            var activeUser = ActiveUser();
            if (activeUser.HasValue)
                accounts.Add(activeUser.Value);

            return accounts;
        }
    }

    private static IEnumerable<long> Users()
    {
        if (UsersRegistryKey == null)
            return Array.Empty<long>();

        var keys = UsersRegistryKey.GetSubKeyNames();
        var identifiers = keys
            .Select(SteamIdHelper.UserToCommunityId)
            .Where(w => w.HasValue)
            .Cast<long>();

        return identifiers;
    }

    private static long? ActiveUser()
    {
        if (ActiveProcessRegistryKey == null)
            return null;

        var activeUser = ActiveProcessRegistryKey.GetValue("ActiveUser")?.ToString()?.Trim();
        if (string.IsNullOrEmpty(activeUser) || activeUser == "0")
            return null;

        return SteamIdHelper.UserToCommunityId(activeUser);
    }
}