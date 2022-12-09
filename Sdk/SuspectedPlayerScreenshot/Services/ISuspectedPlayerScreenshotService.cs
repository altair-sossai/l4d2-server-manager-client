using Azure.Storage.Blobs;
using L4D2AntiCheat.Infrastructure.Extensions;
using L4D2AntiCheat.Sdk.SuspectedPlayerScreenshot.Results;
using Refit;

namespace L4D2AntiCheat.Sdk.SuspectedPlayerScreenshot.Services;

public interface ISuspectedPlayerScreenshotService
{
    [Get("/api/suspected-players-screenshot/generate-upload-url")]
    Task<GenerateUploadUrlResult> GenerateUploadUrlAsync();

    public void Upload(string url, Bitmap screenshot)
    {
        var width = Math.Min(screenshot.Width, 1600);
        var height = Math.Min(screenshot.Height, 900);

        using var bitmap = new Bitmap(screenshot, width, height);
        using var memoryStream = bitmap.Compress();

        memoryStream.Position = 0;

        var blobClient = new BlobClient(new Uri(url));
        blobClient.Upload(memoryStream);
    }
}