using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using ManagedPnp.Avalonia.Core.Results.Errors;

namespace ManagedPnp.Avalonia.Core.Results;

/// <summary>
/// Type representing a Union between <typeparamref name="T"/> and <see cref="IError"/> 
/// </summary>
/// <typeparam name="T">kType the Result object is going to wrap</typeparam>
public class Result<T>
{
    private readonly T? _value;
    private readonly IError? _error;

    public Result(T value)
    {
        _value = value;
        _error = null;
        IsSuccess = true;
    }

    public Result(IError error)
    {
        _error = error;
        _value = default;
        IsSuccess = false;
    }

    [MemberNotNullWhen(true, nameof(_value))]
    [MemberNotNullWhen(false, nameof(_error))]
    private bool IsSuccess { get; }


    public TOut Match<TOut>(Func<T, TOut> ok, Func<IError, TOut> err)
    {
        return IsSuccess ? ok(_value) : err(_error);
    }

    public async Task<TOut> MatchAsync<TOut>(Func<T, Task<TOut>> ok, Func<IError, Task<TOut>> err)
    {
        return IsSuccess ? await ok(_value) : await err(_error);
    }

    public void Switch(Action<T> ok, Action<IError> err)
    {
        if (IsSuccess)
        {
            ok(_value);
            return;
        }

        err(_error);
    }

    public async Task SwitchAsync(Func<T, Task> ok, Func<IError, Task> err)
    {
        if (IsSuccess)
        {
            await ok(_value);
            return;
        }

        await err(_error);
    }


    public Result<TOut> Bind<TOut>(Func<T, Result<TOut>> next)
    {
        return IsSuccess ? next(_value) : new Result<TOut>(_error);
    }

    public Result<TOut> Bind<TOut>(Func<T, TOut> next)
    {
        return IsSuccess ? next(_value) : new Result<TOut>(_error);
    }

    public async Task<Result<TOut>> BindAsync<TOut>(Func<T, Task<Result<TOut>>> next)
    {
        return IsSuccess ? await next(_value) : new Result<TOut>(_error);
    }


    public async Task<Result<TOut>> BindAsync<TOut>(Func<T, Task<TOut>> next)
    {
        return IsSuccess ? await next(_value) : new Result<TOut>(_error);
    }


    public static implicit operator Result<T>(T value) => Result.Ok(value);
    public static implicit operator Result<T>(Error error) => Result.Fail<T>(error);
}

public static class Result
{
    public static Result<T> Ok<T>(T value) => new(value);
    public static Result<T> Fail<T>(IError error) => new(error);
    public static Result<T> Fail<T>(string code, string description) => new(new Error(code, description));
}