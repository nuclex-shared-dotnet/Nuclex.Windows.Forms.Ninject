#region CPL License
/*
Nuclex Framework
Copyright (C) 2002-2019 Nuclex Development Labs

This library is free software; you can redistribute it and/or
modify it under the terms of the IBM Common Public License as
published by the IBM Corporation; either version 1.0 of the
License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
IBM Common Public License for more details.

You should have received a copy of the IBM Common Public
License along with this library
*/
#endregion

using System;

using Ninject;
using Ninject.Activation;
using Ninject.Modules;

using Nuclex.Windows.Forms.AutoBinding;
using Nuclex.Windows.Forms.CommonDialogs;
using Nuclex.Windows.Forms.Messages;

namespace Nuclex.Windows.Forms.Ninject {

  /// <summary>Sets up the service bindings for an MVVM-based WPF application</summary>
  public class MvvmModule : NinjectModule {

    /// <summary>Called when the module is loaded into the kernel</summary>
    public override void Load() {

      // The task dialog message service actually supports two interfaces
      Kernel.Bind<IMessageService>().To<StandardMessageBoxManager>().InSingletonScope();

      // Use the common dialog manager to display file open, save or print dialogs
      Kernel.Bind<ICommonDialogService>().To<CommonDialogManager>().InSingletonScope();

      // The window manager keeps track of active windows and can figure out
      // which window to display for a view model by its naming convention.
      Kernel.Bind<WindowManager>().To<NinjectWindowManager>().InSingletonScope();
      Kernel.Bind<IWindowManager>().ToMethod(getWindowManager).InSingletonScope();
      Kernel.Bind<IActiveWindowTracker>().ToMethod(getWindowManager).InSingletonScope();
      Kernel.Bind<IAutoBinder>().ToMethod(CreateAutoBinder).InSingletonScope();

    }

    /// <summary>Creates and initializd the auto view model binder</summary>
    /// <param name="context">
    ///   Context containing environmental informations about the request and the kernel
    /// </param>
    /// <returns>The view model auto binder that will be used by the application</returns>
    protected virtual IAutoBinder CreateAutoBinder(IContext context) {
      return new ConventionBinder();
    }

    /// <summary>Retrieves the window manager from the kernel</summary>
    /// <param name="context">
    ///   Context containing environmental informations about the request and the kernel
    /// </param>
    /// <returns>The window manager registered to the kernel</returns>
    private static WindowManager getWindowManager(IContext context) {
      return context.Kernel.Get<WindowManager>();
    }

  }

} // namespace Nuclex.Windows.Forms.Ninject
