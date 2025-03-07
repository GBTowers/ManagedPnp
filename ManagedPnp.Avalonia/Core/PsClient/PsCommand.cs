using ManagedPnp.Avalonia.Core.Utils;

namespace ManagedPnp.Avalonia.Core.PsClient;

public class PsCommand : StringEnum
{
    private PsCommand(string value) : base(value)
    {
    }

    public static PsCommand PnpUtil { get; } = new("pnputil");
    public static PsCommand SelectObject { get; } = new("Select-Object");
    public static PsCommand OutString { get; } = new("Out-String");
}