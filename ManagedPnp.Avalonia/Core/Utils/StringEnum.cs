using System;
using System.Diagnostics.CodeAnalysis;

namespace ManagedPnp.Avalonia.Core.Utils;

public abstract class StringEnum
{
    protected StringEnum(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public override string ToString() => Value;

    public static implicit operator string(StringEnum value) => value.Value;

    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is StringEnum strEnum && Equals(strEnum);

    public bool Equals([NotNullWhen(true)] StringEnum? other) =>
        other is not null &&
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}