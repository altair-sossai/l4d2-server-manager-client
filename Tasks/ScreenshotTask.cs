using L4D2AntiCheat.Context;
using L4D2AntiCheat.Infrastructure.Extensions;
using L4D2AntiCheat.Infrastructure.Helpers;
using L4D2AntiCheat.Modules.Screenshot.Services;
using L4D2AntiCheat.ProcessInfo;
using L4D2AntiCheat.Sdk.SuspectedPlayerScreenshot.Services;
using L4D2AntiCheat.Tasks.Infrastructure;

namespace L4D2AntiCheat.Tasks;

public class ScreenshotTask : IntervalTask
{
	private static readonly HashSet<string> Md5S = new();
	private readonly ILeft4Dead2ProcessInfo _processInfo;
	private readonly IScreenshotService _screenshotService;
	private readonly ISuspectedPlayerScreenshotService _suspectedPlayerScreenshotService;

	public ScreenshotTask(ILeft4Dead2ProcessInfo processInfo,
		ISuspectedPlayerScreenshotService suspectedPlayerScreenshotService,
		IScreenshotService screenshotService)
		: base(TimeSpan.FromSeconds(45))
	{
		_processInfo = processInfo;
		_suspectedPlayerScreenshotService = suspectedPlayerScreenshotService;
		_screenshotService = screenshotService;
	}

	protected override bool CanRun(AntiCheatContext context)
	{
		return _processInfo.IsFocused;
	}

	protected override void Run(AntiCheatContext context)
	{
		var process = _processInfo.CurrentProcess;
		if (process == null)
			return;

		using var bitmap = ScreenshotHelper.TakeScreenshot(process);
		using var memoryStream = bitmap.Compress();
		
		memoryStream.Position = 0;

		var md5 = Md5Helper.Md5(memoryStream);
		if (Md5S.Contains(md5))
			return;

		Md5S.Add(md5);

		var result = _suspectedPlayerScreenshotService.GenerateUploadUrlAsync().Result;
		if (string.IsNullOrEmpty(result.Url))
			return;

		_screenshotService.Upload(result.Url, memoryStream);
	}
}