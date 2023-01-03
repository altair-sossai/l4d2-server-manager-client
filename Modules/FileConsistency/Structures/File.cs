using L4D2AntiCheat.Infrastructure.Helpers;

namespace L4D2AntiCheat.Modules.FileConsistency.Structures;

public struct File
{
	public File(string md5, long length, string relativePath)
	{
		Md5 = md5;
		Length = length;
		RelativePath = relativePath;
	}

	public string Md5 { get; }
	public long Length { get; }
	public string RelativePath { get; }

	public bool Consistent(string folder, DateTime startTime)
	{
		var filePath = Path.Combine(folder, RelativePath);
		var fileInfo = new FileInfo(filePath);

		var valid = fileInfo.Exists
		            && fileInfo.Length == Length
		            && startTime > fileInfo.CreationTime
		            && startTime > fileInfo.LastWriteTime
		            && Md5Helper.Md5(filePath) == Md5;

		return valid;
	}

	public static File Parse(string line)
	{
		var segments = line.Split(' ', 3);
		var md5 = segments[0];
		var length = long.Parse(segments[1]);
		var relativePath = segments[2];

		return new File(md5, length, relativePath);
	}
}