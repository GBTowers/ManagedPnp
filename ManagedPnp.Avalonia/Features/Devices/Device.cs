using System.Collections.Generic;
using ManagedPnp.Avalonia.Core.Utils;

namespace ManagedPnp.Avalonia.Features.Devices;

public record Device
{
    public required string InstanceID { get; init; }
    public required string DeviceDescription { get; init; }
    public required string ClassName { get; init; }
    public required string ClassGUID { get; init; }
    public required string ManufacturerName { get; init; }
    public required string Status { get; init; }
    public required string DriverName { get; init; }
    

    public static Device FromDictionary(IDictionary<string, string> dict)
    {
        const string fallbackValue = "Not Found";
        
        return new Device
        {
            Status = dict.GetValueOrDefault(nameof(Status), fallbackValue),
            InstanceID = dict.GetValueOrDefault(nameof(InstanceID), fallbackValue),
            DeviceDescription = dict.GetValueOrDefault(nameof(DeviceDescription), fallbackValue),
            ClassName = dict.GetValueOrDefault(nameof(ClassName), fallbackValue),
            ClassGUID = dict.GetValueOrDefault(nameof(ClassGUID), fallbackValue),
            ManufacturerName = dict.GetValueOrDefault(nameof(ManufacturerName), fallbackValue),
            DriverName = dict.GetValueOrDefault(nameof(DriverName), fallbackValue),
        };
    }

    public override string ToString()
    {
        return $"{DeviceDescription} | {ClassName}";
    }
}

public enum DeviceStatus
{
    On,
    Off
}