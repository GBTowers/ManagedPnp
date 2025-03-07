using ManagedPnp.Avalonia.Core.Results.Errors;

namespace ManagedPnp.Avalonia.Core.Errors;

public class ErrorStore : IErrorStore
{
    public ErrorViewModel GetViewModel(IError error, string message)
    {
        return new ErrorViewModel(error, message);
    }
}