using System.Collections.Generic;

namespace ManagedPnp.Avalonia.Features.Devices;

public class PinnedDevice
{
    public PinnedDevice(List<DeviceAction> availableActions, Device device)
    {
        AvailableActions = availableActions;
        Device = device;
    }
    public List<DeviceAction> AvailableActions { get; set; }
    public Device Device { get; }
}