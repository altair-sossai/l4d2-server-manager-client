using L4D2AntiCheat.Modules.FileConsistency.Results;

namespace L4D2AntiCheat.Modules.FileConsistency.Services;

public interface IFileConsistencyService
{
    FileConsistencyResult CheckFilesConsistency(string folder, DateTime startTime);
}