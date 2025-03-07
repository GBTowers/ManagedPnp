using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using ManagedPnp.Avalonia.Core.Notifications;
using ManagedPnp.Avalonia.Core.Permanence;
using ManagedPnp.Avalonia.Core.Results;
using Splat;

namespace ManagedPnp.Avalonia.Features.Devices;

public record DeviceAction(string Name, AsyncRelayCommand<Device> Command);


public interface IPinnedDeviceStore
{
    Task PinDevice(Device device);
    Task UnPinDevice(Device device);
    Task Load();
    ObservableCollection<PinnedDevice> PinnedDevices { get; }
    
}

public class PinnedDeviceStore : IPinnedDeviceStore
{
    private readonly INotificationStore _notificationStore;
    private readonly IDeviceManagementService _deviceManagementService;
    private readonly IPermanenceService _permanenceService;
    public ObservableCollection<PinnedDevice> PinnedDevices { get; } = [];
    private readonly List<DeviceAction> _deviceActions;
    private bool _loaded;

    [DependencyInjectionConstructor]
    public PinnedDeviceStore(INotificationStore notificationStore,
        IDeviceManagementService deviceManagementService,
        IPermanenceService permanenceService)
    {
        _deviceActions = [new DeviceAction("Enable / Disable", ChangeDeviceStateCommand)];
        _notificationStore = notificationStore;
        _deviceManagementService = deviceManagementService;
        _permanenceService = permanenceService;
    }

    public async Task Load()
    {
        var result = await _permanenceService
            .Load<List<Device>>()
            .Match(ok => ok, _ => []);
        
        foreach (Device device in result)
        {
            var pinnedDevice = new PinnedDevice(_deviceActions, device);
            PinnedDevices.Add(pinnedDevice);
        }
        
        _loaded = true;
    }

    public async Task PinDevice(Device device)
    {
        if (!_loaded)
        {
            await Load();
        }
        
        if (PinnedDevices.Any(d => d.Device.InstanceID == device.InstanceID)) return;

        PinnedDevices.Add(new PinnedDevice(_deviceActions, device));
        await _permanenceService.SaveAsync(PinnedDevices.Select(d => d.Device));
    }

    public async Task UnPinDevice(Device device)
    {
        if (!_loaded)
        {
            await Load();
        }
        
        PinnedDevice? toBeRemoved = PinnedDevices
            .FirstOrDefault(d => d.Device.InstanceID == device.InstanceID);

        if (toBeRemoved is null) return;

        PinnedDevices.Remove(toBeRemoved);
        await _permanenceService.SaveAsync(PinnedDevices.Select(d => d.Device));
    }

    private AsyncRelayCommand<Device> ChangeDeviceStateCommand => new(
        async device =>
        {
            if (device != null)
            {
                var result = await _deviceManagementService.ChangeDeviceState(device.InstanceID);

                result.Switch(ok =>
                {
                    var notification = new Notification(ok.Item2.DeviceDescription, 
                        ok.Item1,
                        ok.Item2.Status);
                    _notificationStore.ShowNotification(notification);
                }, err =>
                {
                    var notification = new Notification(err.Description, 
                        err.Code, 
                        device.ClassName);
                    _notificationStore.ShowNotification(notification);
                });
            }
        });
}