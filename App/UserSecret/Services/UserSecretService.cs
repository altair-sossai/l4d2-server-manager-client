using L4D2AntiCheat.App.UserSecret.Repositories;
using L4D2AntiCheat.Infrastructure.Helpers;
using L4D2AntiCheat.Sdk.SuspectedPlayerSecret.Commands;
using L4D2AntiCheat.Sdk.SuspectedPlayerSecret.Services;

namespace L4D2AntiCheat.App.UserSecret.Services;

public class UserSecretService : IUserSecretService
{
    private readonly ISuspectedPlayerSecretService _suspectedPlayerSecretService;
    private readonly IUserSecretRepository _userSecretRepository;

    public UserSecretService(IUserSecretRepository userSecretRepository,
        ISuspectedPlayerSecretService suspectedPlayerSecretService)
    {
        _userSecretRepository = userSecretRepository;
        _suspectedPlayerSecretService = suspectedPlayerSecretService;
    }

    public string? GetOrCreatUserSecret(long communityId)
    {
        return _userSecretRepository.Get(communityId) ?? TryCreatUserSecret(communityId);
    }

    private string? TryCreatUserSecret(long communityId)
    {
        try
        {
            var steam3 = SteamIdHelper.CommunityIdToSteam3(communityId);
            var command = new AddSuspectedPlayerSecretCommand(steam3);
            var result = _suspectedPlayerSecretService.AddAsync(command).Result;

            _userSecretRepository.Set(communityId, result.Secret!);

            return result.Secret;
        }
        catch (Exception)
        {
            return null;
        }
    }
}