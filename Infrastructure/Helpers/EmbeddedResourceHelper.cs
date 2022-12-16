using System.Reflection;

namespace L4D2AntiCheat.Infrastructure.Helpers;

public static class EmbeddedResourceHelper
{
	private static readonly Assembly Assembly = Assembly.Load("L4D2AntiCheat");

	public static string? ReadAllText(string resourceName)
	{
		using var stream = Assembly.GetManifestResourceStream(resourceName);

		if (stream == null)
			return null;

		using var streamReader = new StreamReader(stream);

		return streamReader.ReadToEnd();
	}
}