using System;

namespace ManagedPnp.Avalonia.Core.Results.Errors;

public interface IExceptionError
{
    public Exception Exception { get; }
}