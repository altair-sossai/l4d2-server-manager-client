using L4D2AntiCheat.Sdk.SuspectedPlayer.Services;
using L4D2AntiCheat.Sdk.SuspectedPlayerSecret.Services;
using L4D2AntiCheat.Sdk.VirtualMachine.Services;

namespace L4D2AntiCheat.Sdk;

public interface IServerManagerContext
{
    ISuspectedPlayerService SuspectedPlayerService { get; }
    ISuspectedPlayerSecretService SuspectedPlayerSecretService { get; }
    IVirtualMachineService VirtualMachineService { get; }
}