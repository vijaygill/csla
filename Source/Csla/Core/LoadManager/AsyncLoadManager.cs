﻿//-----------------------------------------------------------------------
// <copyright file="AsyncLoadManager.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: https://cslanet.com
// </copyright>
// <summary>no summary</summary>
//-----------------------------------------------------------------------

using System.Collections.ObjectModel;
using Csla.Properties;

namespace Csla.Core.LoadManager
{
  internal class AsyncLoadManager : INotifyBusy
  {
    private readonly IManageProperties _target;
    private readonly Action<IPropertyInfo> _onPropertyChanged;

    public AsyncLoadManager(IManageProperties target, Action<IPropertyInfo> onPropertyChanged)
    {
      _target = target;
      _onPropertyChanged = onPropertyChanged;

    }

    private Lock _syncRoot = LockFactory.Create();
    private readonly ObservableCollection<IAsyncLoader> _loading = [];

    public bool IsLoading
    {
      get
      {
        lock (_syncRoot)
          return _loading.Any();
      }
    }

    public bool IsLoadingProperty(IPropertyInfo property)
    {
      lock (_syncRoot)
      {
        return _loading.Any(l => l.Property == property);
      }
    }

    public void BeginLoad(IAsyncLoader loader)
    {
      if (IsLoadingProperty(loader.Property)) return;

      lock (_syncRoot)
      {
        _loading.Add(loader);
      }
      // notify property busy
      OnPropertyBusy(loader.Property.Name, true);

      // start async loading 
      loader.Load(LoaderComplete);
    }
    
    void LoaderComplete(IAsyncLoader loader, IDataPortalResult e)
    {
      // remove from loading list 
      lock (_syncRoot)
      {
        _loading.Remove(loader);
      }

      // no error then load new property value and notify property changed
      if (e.Error == null) 
      {
        _target.LoadProperty(loader.Property, e.Object);
        _onPropertyChanged(loader.Property);
      }

      // mark property as not busy 
      OnPropertyBusy(loader.Property.Name, false);

      // if error raise OnUnhandledAsyncException event
      if (e.Error != null) 
        OnUnhandledAsyncException(this, new AsyncLoadException(loader.Property,string.Format(Resources.AsyncLoadException, loader.Property.FriendlyName), e.Error));
    }

    #region INotifyPropertyBusy Members

    public event BusyChangedEventHandler? BusyChanged;
    protected void OnPropertyBusy(string propertyName, bool busy)
    {
      BusyChanged?.Invoke(this, new BusyChangedEventArgs(propertyName, busy));
    }

    #endregion

    #region INotifyBusy Members

    bool INotifyBusy.IsBusy => (this as INotifyBusy).IsSelfBusy;

    bool INotifyBusy.IsSelfBusy => IsLoading;

    #endregion

    #region INotifyUnhandledAsyncException Members

    [NotUndoable]
    [NonSerialized]
    private EventHandler<ErrorEventArgs>? _unhandledAsyncException;

    public event EventHandler<ErrorEventArgs>? UnhandledAsyncException
    {
      add => _unhandledAsyncException = (EventHandler<ErrorEventArgs>?)Delegate.Combine(_unhandledAsyncException, value);
      remove => _unhandledAsyncException = (EventHandler<ErrorEventArgs>?)Delegate.Remove(_unhandledAsyncException, value);
    }

    protected virtual void OnUnhandledAsyncException(ErrorEventArgs error)
    {
      _unhandledAsyncException?.Invoke(this, error);
    }

    protected void OnUnhandledAsyncException(object originalSender, Exception error)
    {
      OnUnhandledAsyncException(new ErrorEventArgs(originalSender, error));
    }

    #endregion
  }
}