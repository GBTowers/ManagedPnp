using CommunityToolkit.Mvvm.ComponentModel;
using ManagedPnp.Avalonia.Core.Navigation;
using ManagedPnp.Avalonia.Core.ViewModelCreation;
using Splat;

namespace ManagedPnp.Avalonia;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty] private INavigationStore _navigation;

    public MainViewModel()
    {
        Navigation = new NavigationStore();
    }

    [DependencyInjectionConstructor]
    public MainViewModel(INavigationStore navigation)
    {
        Navigation = navigation;
    }
}