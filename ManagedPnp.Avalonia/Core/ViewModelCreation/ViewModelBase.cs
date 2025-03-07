using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace ManagedPnp.Avalonia.Core.ViewModelCreation;

public abstract class ViewModelBase : ObservableRecipient, IDisposable
{
    private bool _disposed;

    protected ViewModelBase()
    {
        IsActive = true;
    }

    protected ViewModelBase(IMessenger messenger) : base(messenger)
    {
        IsActive = true;
    }

    public virtual void Dispose()
    {
        if (!_disposed)
        {
            IsActive = false;
        }

        _disposed = true;
    }
}