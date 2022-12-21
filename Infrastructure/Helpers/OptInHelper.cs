using Microsoft.Win32;

namespace L4D2AntiCheat.Infrastructure.Helpers;

public static class OptInHelper
{
	private const string RegistryName = @"SOFTWARE\L4D2AntiCheat\OptIn";
	private static readonly RegistryKey RegistryKey;

	static OptInHelper()
	{
		RegistryHelper.CreateIfDoesNotExist(RegistryName);
		RegistryKey = Registry.CurrentUser.OpenSubKey(RegistryName, true)!;
	}

	public static bool Accepted()
	{
		return RegistryKey.GetValue("Accepted")?.ToString() == "true";
	}

	public static void Accept()
	{
		RegistryKey.SetValue("Accepted", "true");
		RegistryKey.SetValue("AcceptedIn", DateTime.UtcNow.ToString("u"));
	}
}