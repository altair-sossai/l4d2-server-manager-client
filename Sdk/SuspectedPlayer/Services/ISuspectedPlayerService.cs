using L4D2AntiCheat.Sdk.SuspectedPlayer.Results;
using Refit;

namespace L4D2AntiCheat.Sdk.SuspectedPlayer.Services;

public interface ISuspectedPlayerService
{
    [Get("/api/suspected-players/{communityId}")]
    Task<SuspectedPlayerResult> Find(long communityId);
}