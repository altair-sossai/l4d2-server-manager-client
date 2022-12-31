using Azure.Storage.Blobs;
using L4D2AntiCheat.Infrastructure.Extensions;
using L4D2AntiCheat.Infrastructure.Helpers;
using L4D2AntiCheat.Sdk.SuspectedPlayerScreenshot.Results;
using Refit;

namespace L4D2AntiCheat.Sdk.SuspectedPlayerScreenshot.Services;

public interface ISuspectedPlayerScreenshotService
{
	private static readonly HashSet<string> Md5S = new();

	[Get("/api/suspected-players-screenshot/generate-upload-url")]
	Task<GenerateUploadUrlResult> GenerateUploadUrlAsync();

	public void Upload(string url, Bitmap screenshot)
	{
		var width = Math.Min(screenshot.Width, 1280);
		var height = Math.Min(screenshot.Height, 720);

		using var bitmap = new Bitmap(screenshot, width, height);
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