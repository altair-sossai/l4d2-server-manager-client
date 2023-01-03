using File = L4D2AntiCheat.Modules.FileConsistency.Structures.File;

namespace L4D2AntiCheat.Modules.FileConsistency.Results;

public class FileConsistencyResult
{
	public FileConsistencyResult(IEnumerable<File> inconsistentFiles)
	{
		InconsistentFiles = inconsistentFiles.ToList();
	}

	public bool IsValid => InconsistentFiles.Count == 0;
	public List<File> InconsistentFiles { get; }
}