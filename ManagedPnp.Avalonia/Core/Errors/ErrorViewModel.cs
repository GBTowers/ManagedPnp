using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using ManagedPnp.Avalonia.Core.Results.Errors;
using ManagedPnp.Avalonia.Core.ViewModelCreation;

namespace ManagedPnp.Avalonia.Core.Errors;

public partial class ErrorViewModel : ViewModelBase
{
    [ObservableProperty] private string _message = null!;
    [ObservableProperty] private IError _error = null!;

    public ErrorViewModel()
    {
        if (!Design.IsDesignMode) return;
        Message = "There wasn't an error, this is design time";
        Error = ApplicationErrors.DesignTestError;
    }

    public ErrorViewModel(IError error, string message)
    {
        Message = message;
        Error = error;
    }
}