using ManagedPnp.Avalonia.Core.Results.Errors;

namespace ManagedPnp.Avalonia.Features.Devices;

public class DeviceErrors
{
    public static IError DeviceStateError(string message) => new Error("device.state.error", message);
}