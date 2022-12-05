using L4D2AntiCheat.Sdk.SuspectedPlayerSecret.Commands;
using L4D2AntiCheat.Sdk.SuspectedPlayerSecret.Results;
using Refit;

namespace L4D2AntiCheat.Sdk.SuspectedPlayerSecret.Services;

public interface ISuspectedPlayerSecretService
{
    [Post("/api/suspected-players-secret")]
    Task<SuspectedPlayerSecretResult> AddAsync([Body] AddSuspectedPlayerSecretCommand command);

    [Post("/api/suspected-players-secret/validate")]
    Task<ValidateSecretResult> ValidateAsync([Body] ValidateSecretCommand command);
}