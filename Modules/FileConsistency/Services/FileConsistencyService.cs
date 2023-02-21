using L4D2AntiCheat.Infrastructure.Helpers;
using L4D2AntiCheat.Modules.FileConsistency.Results;
using L4D2AntiCheat.Sdk.SuspectedPlayerFileCheck.Services;
using L4D2AntiCheat.Sdk.SuspectedPlayerFileFail.Commands;
using File = L4D2AntiCheat.Modules.FileConsistency.Structures.File;

namespace L4D2AntiCheat.Modules.FileConsistency.Services;

public class FileConsistencyService : IFileConsistencyService
{
    private static readonly File[] Files;
    private readonly ISuspectedPlayerFileCheck _suspectedPlayerFileCheck;

    static FileConsistencyService()
    {
        var filesHash = EmbeddedResourceHelper.ReadAllText("L4D2AntiCheat.Resources.FilesHash.txt")!;
        var lines = filesHash.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        Files = lines.Select(File.Parse).ToArray();
    }

    public FileConsistencyService(ISuspectedPlayerFileCheck suspectedPlayerFileCheck)
    {
        _suspectedPlayerFileCheck = suspectedPlayerFileCheck;
    }

    public FileConsistencyResult CheckFilesConsistency(string folder, DateTime startTime)
    {
        var files = Files.Where(file => !file.Consistent(folder, startTime));
        var result = new FileConsistencyResult(files);

        switch (result.Inconsistent)
        {
            case true:
                var inconsistentFiles = result.InconsistentFiles;
                var commands = SuspectedPlayerFileFailCommand.Parse(folder, inconsistentFiles);
                _suspectedPlayerFileCheck.FailAsync(commands).Wait();
                break;

            case false:
                _suspectedPlayerFileCheck.SuccessAsync().Wait();
                break;
        }

        return result;
    }
}