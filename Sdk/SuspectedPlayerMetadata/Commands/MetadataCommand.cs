namespace L4D2AntiCheat.Sdk.SuspectedPlayerMetadata.Commands;

public class MetadataCommand
{
    public MetadataCommand(string? name, string? value)
    {
        Name = name;
        Value = value;
    }

    public string? Name { get; }
    public string? Value { get; }
}