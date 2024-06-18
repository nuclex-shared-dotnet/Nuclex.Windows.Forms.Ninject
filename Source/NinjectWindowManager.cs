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
