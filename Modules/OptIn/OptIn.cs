using L4D2AntiCheat.Infrastructure.Helpers;
using Microsoft.Win32;

namespace L4D2AntiCheat.Modules.OptIn;

public static class OptIn
{
	private const string RegistryName = @"SOFTWARE\L4D2AntiCheat\OptIn";
	private static readonly RegistryKey RegistryKey;

	static OptIn()
	{
		RegistryHelper.CreateIfDoesNotExist(RegistryName);
		RegistryKey = Registry.CurrentUser.OpenSubKey(RegistryName, true)!;
	}

	public static bool Accepted => RegistryKey.GetValue("Accepted")?.ToString() == "true";

	public static void Accept()
	{
		RegistryKey.SetValue("Accepted", "true");
		RegistryKey.SetValue("AcceptedIn", DateTime.UtcNow.ToString("u"));
	}
}