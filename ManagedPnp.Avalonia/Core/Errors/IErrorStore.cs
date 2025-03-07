using ManagedPnp.Avalonia.Core.Results.Errors;

namespace ManagedPnp.Avalonia.Core.Errors;

public interface IErrorStore
{
    ErrorViewModel GetViewModel(IError error, string message);
}