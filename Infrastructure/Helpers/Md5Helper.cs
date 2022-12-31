using System.Security.Cryptography;

namespace L4D2AntiCheat.Infrastructure.Helpers;

public static class Md5Helper
{
	public static string Md5(string filename)
	{
		using var stream = new FileStream(filename, FileMode.Open, FileAccess.Read);

		var buffer = new byte[1024 * 1000];
		_ = stream.Read(buffer, 0, buffer.Length);

		return Md5(buffer);
	}

	public static string Md5(MemoryStream memoryStream)
	{
		return Md5(memoryStream.ToArray());
	}

	private static string Md5(byte[] bytes)
	{
		using var md5 = MD5.Create();

		var hash = md5.ComputeHash(bytes);

		return BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();
	}
}