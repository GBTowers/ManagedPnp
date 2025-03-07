using System;
using System.Threading.Tasks;
using ManagedPnp.Avalonia.Core.Results.Errors;

namespace ManagedPnp.Avalonia.Core.Results;

public static class ResultExtensions
{
    public static async Task<Result<TOut>> Bind<T, TOut>(this Task<Result<T>> task, Func<T, Result<TOut>> next)
    {
        var result = await task.ConfigureAwait(false);
        return result.Bind(next);
    }

    public static async Task<Result<TOut>> Bind<T, TOut>(this Task<Result<T>> task, Func<T, TOut> next)
    {
        var result = await task.ConfigureAwait(false);
        return result.Bind(next);
    }

    public static async Task<Result<TOut>> BindAsync<T, TOut>(this Task<Result<T>> task,
        Func<T, Task<Result<TOut>>> next)
    {
        var result = await task.ConfigureAwait(false);
        return await result.BindAsync(next);
    }

    public static async Task<Result<TOut>> BindAsync<T, TOut>(this Task<Result<T>> task, Func<T, Task<TOut>> next)
    {
        var result = await task.ConfigureAwait(false);
        return await result.BindAsync(next);
    }

    public static async Task<TOut> Match<T, TOut>(this Task<Result<T>> task, Func<T, TOut> ok,
        Func<IError, TOut> err)
    {
        var result = await task.ConfigureAwait(false);
        return result.Match(ok, err);
    }

    public static async Task<TOut> MatchAsync<T, TOut>(this Task<Result<T>> task, Func<T, Task<TOut>> ok,
        Func<IError, Task<TOut>> err)
    {
        var result = await task.ConfigureAwait(false);
        return await result.MatchAsync(ok, err);
    }

    public static async Task Switch<T>(this Task<Result<T>> task, Action<T> ok, Action<IError> err)
    {
        var result = await task.ConfigureAwait(false);
        result.Switch(ok, err);
    }
}