using L4D2AntiCheat.ProcessInfo.Infrastructure;

namespace L4D2AntiCheat.ProcessInfo;

public interface ILeft4Dead2ProcessInfo : IProcessInfo
{
    bool IsFocused { get; }
    string? RootFolder { get; }
    bool AttachProcess();
}