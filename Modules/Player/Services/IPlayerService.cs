using L4D2AntiCheat.Sdk.SuspectedPlayer.Results;

namespace L4D2AntiCheat.Modules.Player.Services;

public interface IPlayerService
{
	List<SuspectedPlayerResult> Accounts();
}