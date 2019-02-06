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
