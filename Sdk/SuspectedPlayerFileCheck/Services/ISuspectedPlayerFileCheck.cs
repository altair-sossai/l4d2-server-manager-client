using Refit;

namespace L4D2AntiCheat.Sdk.SuspectedPlayerFileCheck.Services;

public interface ISuspectedPlayerFileCheck
{
	[Post("/api/suspected-players-file-check-success")]
	Task SuccessAsync();

	[Post("/api/suspected-players-file-check-fail")]
	Task FailAsync();
}