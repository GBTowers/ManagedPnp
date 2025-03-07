using System.Collections.Generic;

namespace ManagedPnp.Avalonia.Core.Utils;

public static class DictionaryExtensions
{
    public static TValue? GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
    {
        return dict.TryGetValue(key, out TValue? val) ? val : default;
    }

    public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key,
        TValue defaultVal)
    {
        return dict.TryGetValue(key, out TValue? val) ? val : defaultVal;
    }
}