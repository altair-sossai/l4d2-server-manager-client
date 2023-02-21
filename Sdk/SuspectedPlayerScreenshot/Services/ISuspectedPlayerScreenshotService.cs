using L4D2AntiCheat.Sdk.SuspectedPlayerScreenshot.Results;
using Refit;

namespace L4D2AntiCheat.Sdk.SuspectedPlayerScreenshot.Services;

public interface ISuspectedPlayerScreenshotService
{
    [Get("/api/suspected-players-screenshot/generate-upload-url")]
    Task<GenerateUploadUrlResult> GenerateUploadUrlAsync();
}