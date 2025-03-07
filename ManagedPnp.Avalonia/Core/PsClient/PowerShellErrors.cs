using System;
using System.Collections.Generic;
using System.Management.Automation;
using ManagedPnp.Avalonia.Core.Results.Errors;

namespace ManagedPnp.Avalonia.Core.PsClient;

public static class PowerShellErrors
{
    public static Error RuntimeError(ErrorRecord record)
    {
        return new Error("powershell.command.runtime", record.ErrorDetails.Message);
    }

    public static CompositeError CompositeError(List<IError> errors, string message)
    {
        return new CompositeError("powershell.composite.error", message, errors);
    }

    public static ExceptionError RuntimeException(Exception ex)
    {
        return new ExceptionError("powershell.command.runtime.exception", ex);
    }
}