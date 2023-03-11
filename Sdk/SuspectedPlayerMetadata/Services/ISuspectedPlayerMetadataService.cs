using L4D2AntiCheat.Sdk.SuspectedPlayerMetadata.Commands;
using Refit;

namespace L4D2AntiCheat.Sdk.SuspectedPlayerMetadata.Services;

public interface ISuspectedPlayerMetadataService
{
    [Post("/api/suspected-players-metadata")]
    Task AddOrUpdateAsync([Body] List<MetadataCommand> commands);
}