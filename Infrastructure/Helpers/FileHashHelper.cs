using System.Diagnostics;

namespace L4D2AntiCheat.Infrastructure.Helpers;

public static class FileHashHelper
{
	private static readonly string FilesHashContent = EmbeddedResourceHelper.ReadAllText("L4D2AntiCheat.Resources.FilesHash.txt")!;
	private static readonly string[] Lines = FilesHashContent.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

	public static bool IsValid()
	{
		var process = Left4Dead2ProcessHelper.GetProcess();
		if (string.IsNullOrEmpty(process?.MainModule?.FileName))
			return false;

		var fileInfo = new FileInfo(process.MainModule.FileName);
		var directoryInfo = fileInfo.Directory;

		return directoryInfo != null && Lines.All(path => IsValid(process, directoryInfo.FullName, path));
	}

	public static IEnumerable<string> InvalidFiles()
	{
		var process = Left4Dead2ProcessHelper.GetProcess();
		if (string.IsNullOrEmpty(process?.MainModule?.FileName))
			return Array.Empty<string>();

		var fileInfo = new FileInfo(process.MainModule.FileName);
		var directoryInfo = fileInfo.Directory;

		if (directoryInfo == null)
			return Array.Empty<string>();

		return Lines
			.Where(path => !IsValid(process, directoryInfo.FullName, path))
			.Select(line => line.Split(' ', 3).Last());
	}

	private static bool IsValid(Process process, string folder, string line)
	{
		var segments = line.Split(' ', 3);
		var md5 = segments[0];
		var length = long.Parse(segments[1]);
		var relativePath = segments[2];

		var filePath = Path.Combine(folder, relativePath);
		var fileInfo = new FileInfo(filePath);
		var startTime = process.StartTime;

		var valid = fileInfo.Exists
		            && fileInfo.Length == length
		            && startTime > fileInfo.CreationTime
		            && startTime > fileInfo.LastWriteTime
		            && Md5Helper.Md5(filePath) == md5;

		return valid;
	}
}