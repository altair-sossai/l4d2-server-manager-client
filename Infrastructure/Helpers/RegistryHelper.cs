using Microsoft.Win32;

namespace L4D2AntiCheat.Infrastructure.Helpers;

public static class RegistryHelper
{
	public static IEnumerable<long> SteamUsers()
	{
		var users = new HashSet<long>();
		var currentUser = Registry.CurrentUser;

		foreach (var user in SteamUsers(currentUser))
			users.Add(user);

		var activeUser = ActiveUser(currentUser);
		if (activeUser.HasValue)
			users.Add(activeUser.Value);

		return users;
	}

	private static IEnumerable<long> SteamUsers(RegistryKey registryKey)
	{
		var users = registryKey.OpenSubKey(@"SOFTWARE\Valve\Steam\Users");
		if (users == null)
			return Array.Empty<long>();

		var keys = users.GetSubKeyNames();
		var identifiers = keys
			.Select(CommunityId)
			.Where(w => w.HasValue)
			.Cast<long>();

		return identifiers;
	}

	private static long? ActiveUser(RegistryKey registryKey)
	{
		var activeProcess = registryKey.OpenSubKey(@"SOFTWARE\Valve\Steam\ActiveProcess");
		if (activeProcess == null)
			return null;

		var activeUser = activeProcess.GetValue("ActiveUser")?.ToString()?.Trim();
		if (string.IsNullOrEmpty(activeUser) || activeUser == "0")
			return null;

		return CommunityId(activeUser);
	}

	private static long? CommunityId(string user)
	{
		var steam3 = $"[U:1:{user}]";
		var communityId = SteamIdHelper.Steam3ToCommunityId(steam3);

		return communityId;
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