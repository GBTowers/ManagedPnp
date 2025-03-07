using System.Collections.Generic;

namespace ManagedPnp.Avalonia.Core.Results.Errors;

public record CompositeError : Error
{
    public List<IError> Errors { get; }

    public CompositeError(string code, string description, List<IError> errors) : base(code, description)
    {
        Errors = errors;
    }
}