using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.Messaging;
using ManagedPnp.Avalonia.Core.Navigation;
using ManagedPnp.Avalonia.Core.Notifications;
using ManagedPnp.Avalonia.Core.Permanence;
using ManagedPnp.Avalonia.Core.PsClient;
using ManagedPnp.Avalonia.Core.Utils;
using ManagedPnp.Avalonia.Core.ViewModelCreation;
using ManagedPnp.Avalonia.Features.Devices;
using Splat;

namespace ManagedPnp.Avalonia;

// ReSharper disable once PartialTypeWithSinglePart
public partial class App : Application
{
    private IPinnedDeviceStore? _pinnedStore;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        SplatRegistrations.RegisterLazySingleton<MainViewModel>();
        SplatRegistrations.RegisterLazySingleton<MainWindow>();
        SplatRegistrations.RegisterLazySingleton<IMessenger, StrongReferenceMessenger>();
        SplatRegistrations.RegisterLazySingleton<IDeviceManagementService, DeviceManagementService>();
        SplatRegistrations.RegisterLazySingleton<IPnpUtilClient, PnpUtilClient>();
        SplatRegistrations.RegisterLazySingleton<INavigationStore, NavigationStore>();
        SplatRegistrations.RegisterLazySingleton<INotificationStore, NotificationStore>();
        SplatRegistrations.RegisterLazySingleton<IPinnedDeviceStore, PinnedDeviceStore>();
        SplatRegistrations.RegisterLazySingleton<IPermanenceService, DiskPermanenceService>();
        RegisterPageModels();
        
        SplatRegistrations.RegisterConstant<Func<Type, ViewModelBase>>
            (type => (ViewModelBase)Locator.Current.GetService(type)!);
        SplatRegistrations.RegisterConstant(typeof(DeviceListViewModel));

        string? pinnedDevicesFilePath = ConfigurationManager.AppSettings[Constants.PinnedDevicesFilePath];
        Trace.Assert(pinnedDevicesFilePath is not null, "invalid file path");
        SplatRegistrations.RegisterConstant(new DiskPermanenceConfiguration(pinnedDevicesFilePath));
        SplatRegistrations.SetupIOC();

        _pinnedStore = Locator.Current.GetService<IPinnedDeviceStore>();
        _pinnedStore!.PinnedDevices.CollectionChanged += UpdateTrayMenu;
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DisableAvaloniaDataAnnotationValidation();
            
            desktop.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            desktop.MainWindow = Locator.Current.GetService<MainWindow>();
            desktop.MainWindow!.Closing += (s, e) =>
            {
                ((Window)s!).Hide();
                e.Cancel = true;
            };
        }

        base.OnFrameworkInitializationCompleted();
    }


    private static void RegisterPageModels()
    {
        SplatRegistrations.Register<DeviceListViewModel>();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }

    private void UpdateTrayMenu(object? sender, NotifyCollectionChangedEventArgs args)
    {
        var oldItems = TrayIcon.GetIcons(this)?.First().Menu?.Items;
        if (oldItems == null || args.NewItems == null) return;
        
        var newItems = args.NewItems.Cast<PinnedDevice>();

        foreach (NativeMenuItem nativeMenuItem in newItems.Select(PinnedDevicesToNativeMenuItems))
        {
            oldItems.Add(nativeMenuItem);
        }
        return;
        
        NativeMenuItem PinnedDevicesToNativeMenuItems(PinnedDevice pinnedDevice)
        {
            var actionsMenu = new NativeMenu();
            foreach (DeviceAction action in pinnedDevice.AvailableActions)
            {
                actionsMenu.Items.Add(new NativeMenuItem(action.Name)
                {
                    Command = action.Command,
                    CommandParameter = pinnedDevice.Device
                });
            }

            return new NativeMenuItem { Header = pinnedDevice.Device.DeviceDescription, Menu = actionsMenu };
        }
    }

    private void ExitOption_OnClick(object? sender, EventArgs e)
    {
        if (Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }
    }

    private void TrayIcon_OnClicked(object? sender, EventArgs e)
    {
        if (Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow?.Show();
        }
    }
}