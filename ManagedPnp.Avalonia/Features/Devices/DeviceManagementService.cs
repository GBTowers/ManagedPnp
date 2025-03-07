using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagedPnp.Avalonia.Core.PsClient;
using ManagedPnp.Avalonia.Core.Results;
using ManagedPnp.Avalonia.Core.Utils;
using Splat;

namespace ManagedPnp.Avalonia.Features.Devices;

public interface IDeviceManagementService
{
    public Task<Result<IEnumerable<Device>>> EnumDevices();
    public Task<Result<(string, Device)>> ChangeDeviceState(string instanceId);
}

public class DeviceManagementService : IDeviceManagementService
{
    private readonly IPnpUtilClient _pnpUtilClient;

    [DependencyInjectionConstructor]
    public DeviceManagementService(IPnpUtilClient pnpUtilClient)
    {
        _pnpUtilClient = pnpUtilClient;
    }

    public DeviceManagementService()
    {
        _pnpUtilClient = new PnpUtilClient();
    }


    public async Task<Result<IEnumerable<Device>>> EnumDevices()
    {
        return await _pnpUtilClient.FireCommand(PnpUtilParam.Devices.EnumDevices)
            .Bind(commandOutput => commandOutput.Split("\r\n\r\n").Select(ResourceStringToDevice));
    }

    public async Task<Result<Device>> SearchDevice(string instanceId)
    {
        return await _pnpUtilClient.FireCommand(
                PnpUtilParam.Devices.EnumDevices,
                (PnpUtilParam.Devices.InstanceId, instanceId))
            .Bind(r => r.Trim())
            .Bind(ResourceStringToDevice);
    }

    private Device ResourceStringToDevice(string resourceString)
    {
        resourceString.ReplaceLineEndings();
        var lines = resourceString.Split(Environment.NewLine);

        var resourceDict = lines
            .Select(line =>
            {
                var keyValueString = line.Split(':', StringSplitOptions.TrimEntries);
                return (keyValueString[0].Replace(" ", ""), keyValueString[1]);
            }).ToDictionary();

        return Device.FromDictionary(resourceDict);
    }

    public async Task<Result<(string, Device)>> ChangeDeviceState(string instanceId)
    {
        return await SearchDevice(instanceId)
            .BindAsync(device =>
                device.Status == Constants.StatusEnabled
                    ? DisableDevice(instanceId)
                    : EnableDevice(instanceId));
    }

    private async Task<Result<(string Message, Device Device)>> DisableDevice(string instanceId)
    {
        var message = await _pnpUtilClient.FireCommand(PnpUtilParam.Devices.DisableDevice, (null, instanceId))
            .Bind(message => message.EndsWith(Constants.DisableSuccessMessage)
                ? Result.Ok(Constants.DisableSuccessMessage)
                : Result.Fail<string>(DeviceErrors.DeviceStateError(message)))
            .Match(ok => ok, err => err.Description);

        return await SearchDevice(instanceId).Bind(device => (message, device));
    }

    private async Task<Result<(string Message, Device Device)>> EnableDevice(string instanceId)
    {
        string message = await _pnpUtilClient.FireCommand(PnpUtilParam.Devices.EnableDevice, (null, instanceId))
            .Bind(message => message.EndsWith(Constants.EnableSuccessMessage)
                ? Result.Ok(Constants.EnableSuccessMessage)
                : Result.Fail<string>(DeviceErrors.DeviceStateError(message)))
            .Match(ok => ok, err => err.Description);

        return await SearchDevice(instanceId).Bind(device => (message, device));
    }
}