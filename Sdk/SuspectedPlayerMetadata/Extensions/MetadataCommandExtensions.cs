using L4D2AntiCheat.Sdk.SuspectedPlayerMetadata.Commands;

namespace L4D2AntiCheat.Sdk.SuspectedPlayerMetadata.Extensions;

public static class MetadataCommandExtensions
{
    public static void AddIfNotNull(this List<MetadataCommand> commands, MetadataCommand? command)
    {
        if (command == null)
            return;

        commands.Add(command);
    }
}