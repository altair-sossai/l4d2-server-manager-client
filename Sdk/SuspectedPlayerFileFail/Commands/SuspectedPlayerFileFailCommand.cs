using L4D2AntiCheat.Infrastructure.Helpers;
using L4D2AntiCheat.Sdk.SuspectedPlayerFileFail.Enums;

namespace L4D2AntiCheat.Sdk.SuspectedPlayerFileFail.Commands;

public class SuspectedPlayerFileFailCommand
{
	private SuspectedPlayerFileFailCommand(string folder, string file)
	{
		var filePath = Path.Combine(folder, file);

		File = file;
		Reason = System.IO.File.Exists(filePath) ? FailReason.FileChanged : FailReason.FileDeleted;
	}

	public string? File { get; }
	public FailReason Reason { get; }

	public static List<SuspectedPlayerFileFailCommand> Parse(IEnumerable<string> files)
	{
		var process = Left4Dead2ProcessHelper.GetProcess();
		if (string.IsNullOrEmpty(process?.MainModule?.FileName))
			return new List<SuspectedPlayerFileFailCommand>();

		var fileInfo = new FileInfo(process.MainModule.FileName);
		var directoryInfo = fileInfo.Directory;
		if (string.IsNullOrEmpty(directoryInfo?.FullName))
			return new List<SuspectedPlayerFileFailCommand>();

		var folder = directoryInfo.FullName;

		return files
			.Where(file => !string.IsNullOrEmpty(file))
			.Select(file => new SuspectedPlayerFileFailCommand(folder, file))
			.ToList();
	}
}