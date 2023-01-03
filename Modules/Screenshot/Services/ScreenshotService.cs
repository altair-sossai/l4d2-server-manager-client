using Azure.Storage.Blobs;
using L4D2AntiCheat.Infrastructure.Extensions;
using L4D2AntiCheat.Infrastructure.Helpers;

namespace L4D2AntiCheat.Modules.Screenshot.Services;

public class ScreenshotService : IScreenshotService
{
	private static readonly HashSet<string> Md5S = new();

	public void Upload(string url, Bitmap bitmap)
	{
		using var memoryStream = bitmap.Compress();
		memoryStream.Position = 0;

		var md5 = Md5Helper.Md5(memoryStream);
		if (Md5S.Contains(md5))
			return;

		Md5S.Add(md5);

		var blobClient = new BlobClient(new Uri(url));
		blobClient.Upload(memoryStream);
	}
}