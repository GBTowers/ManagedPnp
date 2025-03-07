using System.Collections.Generic;

namespace ManagedPnp.Avalonia.Core.Results.Errors;

/// <summary>
/// <inheritdoc cref="IError"/>
/// </summary>
public record Error : IError
{
    public Error(string code, string description)
    {
        Description = description;
        Code = code;
    }


    public Error(string code, string description, Dictionary<string, object> metadata)
        : this(code, description)
    {
        Metadata = metadata;
    }

    public string Code { get; }
    public string Description { get; }

    public Dictionary<string, object> Metadata { get; } = new();

    public void AddMetadata(string key, object value)
    {
        Metadata.Add(key, value);
    }
}