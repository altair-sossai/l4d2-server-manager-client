using System.Diagnostics;

namespace L4D2AntiCheat.ProcessInfo.Infrastructure;

public abstract class ProcessInfo : IProcessInfo
{
    private static readonly Dictionary<string, Process?> Processes = new();
    private readonly string _processName;

    protected ProcessInfo(string processName)
    {
        _processName = processName;
    }

    public Process? CurrentProcess
    {
        get
        {
            if (!Processes.ContainsKey(_processName))
                return null;

            var currentProcess = Processes[_processName];
            if (currentProcess == null)
                return null;

            currentProcess.Refresh();

            if (currentProcess.Id == 0 || currentProcess.HasExited)
                CurrentProcess = null;

            return Processes[_processName];
        }
        set
        {
            if (!Processes.ContainsKey(_processName))
                Processes.Add(_processName, value);

            Processes[_processName] = value;
        }
    }

    public bool IsRunning => GetProcess() != null;

    public bool WasClosed
    {
        get
        {
            var currentProcess = CurrentProcess;
            if (currentProcess == null)
                return true;

            var process = GetProcess();
            if (process == null)
                return true;

            return currentProcess.Id != process.Id;
        }
    }

    protected Process? GetProcess()
    {
        var processes = Process.GetProcessesByName(_processName);
        if (processes.Length != 1)
            return null;

        var process = processes[0];
        if (process.Id == 0 || process.HasExited)
            return null;

        return process;
    }
}