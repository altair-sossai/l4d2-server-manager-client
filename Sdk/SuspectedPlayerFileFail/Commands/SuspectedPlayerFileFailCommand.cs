using L4D2AntiCheat.Sdk.SuspectedPlayerFileFail.Enums;
using File = L4D2AntiCheat.Modules.FileConsistency.Structures.File;

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

    public static List<SuspectedPlayerFileFailCommand> Parse(string folder, IEnumerable<File> files)
    {
        return files
            .Select(file => new SuspectedPlayerFileFailCommand(folder, file.RelativePath))
            .ToList();
    }
}