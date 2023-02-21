using File = L4D2AntiCheat.Modules.FileConsistency.Structures.File;

namespace L4D2AntiCheat.Modules.FileConsistency.Results;

public class FileConsistencyResult
{
    public FileConsistencyResult(IEnumerable<File> inconsistentFiles)
    {
        InconsistentFiles = inconsistentFiles.ToList();
    }

    public bool Inconsistent => InconsistentFiles.Any();
    public List<File> InconsistentFiles { get; }
}