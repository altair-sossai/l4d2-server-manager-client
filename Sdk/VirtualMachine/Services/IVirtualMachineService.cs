using L4D2AntiCheat.Sdk.VirtualMachine.Results;
using Refit;

namespace L4D2AntiCheat.Sdk.VirtualMachine.Services;

public interface IVirtualMachineService
{
    [Get("/api/virtual-machine/info")]
    Task<VirtualMachineResult> InfoAsync();
}