#region Apache License 2.0
/*
Nuclex .NET Framework
Copyright (C) 2002-2024 Markus Ewald / Nuclex Development Labs

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
#endregion // Apache License 2.0

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
