using Azure.Storage.Blobs;

namespace L4D2AntiCheat.Modules.Screenshot.Services;

public class ScreenshotService : IScreenshotService
{
    public void Upload(string url, Stream stream)
    {
        var blobClient = new BlobClient(new Uri(url));
        blobClient.Upload(stream);
    }
}