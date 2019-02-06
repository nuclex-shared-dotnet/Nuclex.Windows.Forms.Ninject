using System;

using Ninject;
using Ninject.Activation;
using Ninject.Modules;

using Nuclex.Windows.Forms.AutoBinding;

namespace Nuclex.Windows.Forms.Ninject {

	/// <summary>Sets up the service bindings for an MVVM-based WPF application</summary>
	public class MvvmModule : NinjectModule {

		/// <summary>Called when the module is loaded into the kernel</summary>
		public override void Load() {

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
