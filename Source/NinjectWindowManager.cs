using System;

using Ninject;

using Nuclex.Windows.Forms.AutoBinding;

namespace Nuclex.Windows.Forms.Ninject {

	/// <summary>Window manager that is using Ninject</summary>
	public class NinjectWindowManager : WindowManager {

		/// <summary>Initializes a new window manager</summary>
		///	<param name="kernel">
		///	  Ninject kernel the window manager uses to construct view models
		///	</param>
		/// <param name="autoBinder">
		///   View model binder that will be used to bind all created views to their models
		/// </param>
		public NinjectWindowManager(IKernel kernel, IAutoBinder autoBinder = null) :
			base(autoBinder) {
			this.kernel = kernel;
		}

		/// <summary>Creates an instance of the specified type</summary>
		/// <param name="type">Type an instance will be created of</param>
		/// <returns>The created instance</returns>
		/// <remarks>
		///   Use this to wire up your dependency injection container. By default,
		///   the Activator class will be used to create instances which only works
		///   if all of your view models are concrete classes.
		/// </remarks>
		protected override object CreateInstance(Type type) {
			return this.kernel.Get(type);
		}

		/// <summary>The Ninject kernel used to create new instances</summary>
		private readonly IKernel kernel;

	}

} // namespace Nuclex.Windows.Forms.Ninject
