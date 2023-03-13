using System.Management;
using L4D2AntiCheat.Context;
using L4D2AntiCheat.ProcessInfo;
using L4D2AntiCheat.Sdk.SuspectedPlayerMetadata.Commands;
using L4D2AntiCheat.Sdk.SuspectedPlayerMetadata.Extensions;
using L4D2AntiCheat.Sdk.SuspectedPlayerMetadata.Services;
using L4D2AntiCheat.Tasks.Infrastructure;
using Serilog;

namespace L4D2AntiCheat.Tasks;

public class MetadatasTask : IntervalTask
{
    private static readonly TimeSpan Interval = TimeSpan.FromMinutes(10);
    private readonly ILeft4Dead2ProcessInfo _left4Dead2ProcessInfo;

    private readonly ILogger _logger;
    private readonly ISteamProcessInfo _steamProcessInfo;
    private readonly ISuspectedPlayerMetadataService _suspectedPlayerMetadataService;

    public MetadatasTask(ILogger logger,
        ISuspectedPlayerMetadataService suspectedPlayerMetadataService,
        ISteamProcessInfo steamProcessInfo,
        ILeft4Dead2ProcessInfo left4Dead2ProcessInfo)
        : base(Interval)
    {
        _logger = logger;
        _suspectedPlayerMetadataService = suspectedPlayerMetadataService;
        _steamProcessInfo = steamProcessInfo;
        _left4Dead2ProcessInfo = left4Dead2ProcessInfo;
    }

    protected override bool CanRun(AntiCheatContext context)
    {
        return _steamProcessInfo.IsRunning && _left4Dead2ProcessInfo.IsRunning;
    }

    protected override void Run(AntiCheatContext context)
    {
        var commands = new List<MetadataCommand>();

        commands.AddIfNotNull(SteamCommandLine());
        commands.AddIfNotNull(Left4Dead2CommandLine());

        if (commands.Count == 0)
            return;

        _suspectedPlayerMetadataService.AddOrUpdateAsync(commands).Wait();
    }

    private MetadataCommand? SteamCommandLine()
    {
        return _steamProcessInfo.CurrentProcess == null ? null : CommandLine("Steam command line", _steamProcessInfo.CurrentProcess.Id);
    }

    private MetadataCommand? Left4Dead2CommandLine()
    {
        return _left4Dead2ProcessInfo.CurrentProcess == null ? null : CommandLine("L4D2 command line", _left4Dead2ProcessInfo.CurrentProcess.Id);
    }

    private MetadataCommand? CommandLine(string name, int processId)
    {
        try
        {
            var queryString = $"select CommandLine from win32_process where ProcessId = {processId}";
            var managementObjectSearcher = new ManagementObjectSearcher(queryString);
            var managementObjectCollection = managementObjectSearcher.Get();

            foreach (var managementBaseObject in managementObjectCollection)
                return new MetadataCommand(name, managementBaseObject["CommandLine"]?.ToString());

            return null;
        }
        catch (Exception exception)
        {
            _logger.Error(exception, nameof(CommandLine));
            return null;
        }
    }
}