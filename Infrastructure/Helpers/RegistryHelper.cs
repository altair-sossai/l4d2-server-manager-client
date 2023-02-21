using Microsoft.Win32;

namespace L4D2AntiCheat.Infrastructure.Helpers;

public static class RegistryHelper
{
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