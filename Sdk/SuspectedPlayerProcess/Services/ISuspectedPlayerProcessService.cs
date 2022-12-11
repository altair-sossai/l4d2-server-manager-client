using L4D2AntiCheat.Sdk.SuspectedPlayerProcess.Commands;
using Refit;

namespace L4D2AntiCheat.Sdk.SuspectedPlayerProcess.Services;

public interface ISuspectedPlayerProcessService
{
    [Post("/api/suspected-players-process")]
    Task AddOrUpdateAsync([Body] List<ProcessCommand> commands);
}