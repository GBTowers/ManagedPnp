using ManagedPnp.Avalonia.Core.ViewModelCreation;

namespace ManagedPnp.Avalonia.Core.Navigation;

public interface INavigationStore
{
    public ViewModelBase? CurrentViewModel { get; set; }
    public void NavigateTo<T>() where T : ViewModelBase;
}