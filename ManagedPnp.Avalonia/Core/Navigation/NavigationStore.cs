using System;
using Avalonia.Controls;
using ManagedPnp.Avalonia.Core.ViewModelCreation;
using ManagedPnp.Avalonia.Features.Devices;
using Splat;

namespace ManagedPnp.Avalonia.Core.Navigation;


// ReSharper disable once PartialTypeWithSinglePart
public partial class NavigationStore : ViewModelBase, INavigationStore
{
    private readonly Func<Type, ViewModelBase> _viewModelFactory = null!;
    private ViewModelBase? _currentViewModel;

    public ViewModelBase? CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel?.Dispose();
            SetProperty(ref _currentViewModel, value);
        }
    }

    public NavigationStore()
    {
        if (!Design.IsDesignMode) return;
        CurrentViewModel = new DeviceListViewModel();
    }

    [DependencyInjectionConstructor]
    public NavigationStore(Func<Type, ViewModelBase> viewModelFactory, Type homeViewModelType)
    {
        _viewModelFactory = viewModelFactory;
        CurrentViewModel = _viewModelFactory.Invoke(homeViewModelType);
    }

    public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
    {
        CurrentViewModel = _viewModelFactory.Invoke(typeof(TViewModel));
    }
}