namespace L4D2AntiCheat.Modules.Screenshot.Services;

public interface IScreenshotService
{
	void Upload(string url, Stream stream);
}