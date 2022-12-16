﻿using System.Security.Cryptography;

namespace L4D2AntiCheat.Infrastructure.Helpers;

public static class FileHashHelper
{
	public static bool IsValid()
	{
		var process = Left4Dead2ProcessHelper.GetProcess();
		if (string.IsNullOrEmpty(process?.MainModule?.FileName))
			return false;

		var fileInfo = new FileInfo(process.MainModule.FileName);
		var directoryInfo = fileInfo.Directory;
		if (directoryInfo == null)
			return false;

		var content = EmbeddedResourceHelper.ReadAllText("L4D2AntiCheat.Resources.FilesHash.txt");

		return !string.IsNullOrEmpty(content) 
		       && content.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).All(path => IsValid(directoryInfo.FullName, path));
	}

	private static bool IsValid(string gameFolder, string item)
	{
		var segments = item.Split(' ', 3);
		var md5 = segments[0];
		var length = long.Parse(segments[1]);
		var relativePath = segments[2];

		var filePath = Path.Combine(gameFolder, relativePath);
		var fileInfo = new FileInfo(filePath);

		return fileInfo.Exists && fileInfo.Length == length && Md5(filePath) == md5;
	}

	private static string Md5(string filename)
	{
		using var md5 = MD5.Create();
		using var stream = new FileStream(filename, FileMode.Open, FileAccess.Read);

		var buffer = new byte[1024 * 1000];
		_ = stream.Read(buffer, 0, buffer.Length);

		var hash = md5.ComputeHash(buffer);

		return BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();
	}
}