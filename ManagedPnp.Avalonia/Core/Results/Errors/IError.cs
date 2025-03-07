using System.Collections.Generic;

namespace ManagedPnp.Avalonia.Core.Results.Errors;

/// <summary>
/// Type representing an expected error during normal execution
/// </summary>
public interface IError
{
    /// <summary>
    /// Unique code representing the type of error
    /// </summary>
    string Code { get; }

    /// <summary>
    /// Brief description for the specific error
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Extra user-defined data
    /// </summary>
    Dictionary<string, object> Metadata { get; }
}