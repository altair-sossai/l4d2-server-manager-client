using System.Diagnostics;
using System.Management;

namespace L4D2AntiCheat.Infrastructure.Extensions;

public static class ProcessExtensions
{
    public static Process? Parent(this Process process)
    {
        try
        {
            using var query = new ManagementObjectSearcher($"SELECT * FROM Win32_Process WHERE ProcessId={process.Id}");

            return query
                .Get()
                .OfType<ManagementObject>()
                .Select(managementObject => Process.GetProcessById((int)(uint)managementObject["ParentProcessId"]))
                .FirstOrDefault();
        }
        catch
        {
            return null;
        }
    }
}