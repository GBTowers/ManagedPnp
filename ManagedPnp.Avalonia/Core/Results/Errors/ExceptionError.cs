using System;
using System.Collections.Generic;

namespace ManagedPnp.Avalonia.Core.Results.Errors;

public record ExceptionError : Error, IExceptionError
{
    public ExceptionError(string code, Exception exception) : this(code, exception, exception.Message)
    {
    }

    public ExceptionError(string code, Exception exception, string description) : base(code, description)
    {
        Exception = exception;
    }

    public ExceptionError(string code, Exception exception, string description, Dictionary<string, object> metadata) :
        base(code, description, metadata)
    {
        Exception = exception;
    }

    public Exception Exception { get; }
}