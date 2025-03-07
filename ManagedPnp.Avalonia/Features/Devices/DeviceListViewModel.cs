using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ManagedPnp.Avalonia.Core.Errors;
using ManagedPnp.Avalonia.Core.Notifications;
using ManagedPnp.Avalonia.Core.ViewModelCreation;
using Splat;

namespace ManagedPnp.Avalonia.Features.Devices;

public partial class DeviceListViewModel : ViewModelBase
{
    private readonly IDeviceManagementService _deviceManagementService = null!;
    private readonly IErrorStore _errorStore = null!;
    private readonly INotificationStore _notificationStore = null!;
    private readonly IPinnedDeviceStore _pinnedDeviceStore = null!;

    [ObservableProperty] private List<Device> _devices = [];

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(HasError))]
    private ErrorViewModel? _error;

    [ObservableProperty] private Device? _selectedDevice;


    public bool IsDeviceSelected => SelectedDevice is not null;

    public bool HasError => Error != null;

    public DeviceListViewModel()
    {
        if (!Design.IsDesignMode)
        {
            throw new InvalidOperationException("Default constructor should be called only on design mode");
        }
        
        Devices = [];

        Error = new ErrorViewModel();
    }

    [DependencyInjectionConstructor]
    public DeviceListViewModel(
        IDeviceManagementService deviceManagementService,
        IErrorStore errorStore,
        INotificationStore notificationStore,
        IPinnedDeviceStore pinnedDeviceStore)
    {
        _deviceManagementService = deviceManagementService;
        _errorStore = errorStore;
        _notificationStore = notificationStore;
        _pinnedDeviceStore = pinnedDeviceStore;
    }

    [RelayCommand]
    private async Task Load()
    {
        if (Design.IsDesignMode) return;
        await UpdateDevices();
        await _pinnedDeviceStore.Load();
    }

    [RelayCommand(CanExecute = nameof(IsDeviceSelected))]
    private async Task PinDevice(Device device)
    {
        await _pinnedDeviceStore.PinDevice(device);
    }

    [RelayCommand(CanExecute = nameof(IsDeviceSelected))]
    private async Task ChangeDeviceState(Device device)
    {
        var result = await _deviceManagementService.ChangeDeviceState(device.InstanceID);

        result.Switch(ok =>
        {
            string message = ok.Item1;
            Device dev = ok.Item2;
            var notification = new Notification(message, dev.DeviceDescription, dev.Status);
            _notificationStore.ShowNotification(notification);
        }, err =>
        {
            Error = _errorStore.GetViewModel(err, "Error trying to change device state");
            var notification = new Notification(err.Description, err.Code, device.DeviceDescription);
            _notificationStore.ShowNotification(notification);
        });
        await UpdateDevices();
    }

    [RelayCommand]
    private async Task UpdateDevices()
    {
        Device? selectedDevice = SelectedDevice;

        var result = await _deviceManagementService.EnumDevices();
        result.Switch(ok =>
            {
                Devices = ok.ToList();
                if (selectedDevice is not null)
                {
                    SelectedDevice = Devices.FirstOrDefault(d => d.InstanceID == selectedDevice.InstanceID);
                }
            },
            err => Error = _errorStore.GetViewModel(err, "Error trying to get Devices"));
    }
}