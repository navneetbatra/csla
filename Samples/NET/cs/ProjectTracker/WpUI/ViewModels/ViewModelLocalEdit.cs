﻿using System;
using Bxf;

namespace WpUI.ViewModels
{
  /// <summary>
  /// Base viewmodel type for use with editable model types that are
  /// NOT loaded from the app server (editable root objects).
  /// </summary>
  public class ViewModelLocalEdit<T> : ViewModelLocal<T>
  {
    public void Cancel()
    {
      base.DoCancel();
    }

    public void Save()
    {
      App.ViewModel.ShowStatus(new Status { IsBusy = true, Text = "Saving..." });
      base.BeginSave();
    }

    protected override void OnSaved()
    {
      App.ViewModel.ShowStatus(new Status { IsOk = true, Text = "Saved..." });
      base.OnSaved();
    }

    protected override void OnError(Exception error)
    {
      App.ViewModel.ShowStatus(new Status { IsOk = false });
      string message = null;
      var be = error as Csla.DataPortalException;
      if (be != null)
      {
        if (be.ErrorInfo != null)
          message = be.ErrorInfo.Message;
        else if (be.InnerException != null)
          message = be.InnerException.Message;
        else
          message = be.Message;
      }
      else
        message = error.Message;

      App.ViewModel.ShowError(message, "Error");
      base.OnError(error);
    }
  }
}