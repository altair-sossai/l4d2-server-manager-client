using L4D2AntiCheat.Context;

namespace L4D2AntiCheat.Tasks.Infrastructure;

public interface IIntervalTask
{
    void TryRun(AntiCheatContext context);
}