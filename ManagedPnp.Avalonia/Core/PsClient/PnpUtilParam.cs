using ManagedPnp.Avalonia.Core.Utils;

namespace ManagedPnp.Avalonia.Core.PsClient;

public class PnpUtilParam : StringEnum
{
    private PnpUtilParam(string value) : base(value)
    {
    }
    
    
    
    public static class Devices
    {
        public static PnpUtilParam EnumDevices { get; } = new("/enum-devices");
        public static PnpUtilParam EnableDevice { get; } = new("/enable-device");
        public static PnpUtilParam DisableDevice { get; } = new("/disable-device");
        public static PnpUtilParam InstanceId { get; } = new("/instanceid");
    }
}