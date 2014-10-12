﻿#region CPL License

/*
Nuclex Framework
Copyright (C) 2002-2009 Nuclex Development Labs

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

using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclectic.Graphics.TriD;
using Nuclectic.Tests.Mocks;
#if UNITTEST
using System;
using NUnit.Framework;

namespace Nuclectic.Tests.Graphics
{
	/// <summary>Unit tests for the 'Drawable' class</summary>
	[TestFixture(IgnoreReason = "Unstable, may freeze test runner.")]
	[RequiresSTA]
	internal class DrawableTest
		: TestFixtureBase
	{
		#region class TestDrawable

		/// <summary>Drawable implementation for the unit test</summary>
		private class TestDrawable : Drawable
		{
			/// <summary>Initializes a new drawable</summary>
			/// <param name="service">
			///     Graphics device service that will be used for rendering
			/// </param>
			public TestDrawable(IGraphicsDeviceService service)
				: base(service) { }

			/// <summary>Initializes the drawable from a service provider</summary>
			/// <param name="serviceProvider">
			///     Service provider containing the graphics device service
			/// </param>
			public TestDrawable(IServiceProvider serviceProvider)
				:
					base(GetGraphicsDeviceService(serviceProvider)) { }

#pragma warning disable 672 // Overrides obsolete method
			/// <summary>
			///     Called when the object needs to set up graphics resources. Override to
			///     set up any object specific graphics resources.
			/// </summary>
			/// <param name="createAllContent">
			///     True if all graphics resources need to be set up; false if only
			///     manual resources need to be set up.
			/// </param>
			protected override void LoadGraphicsContent(bool createAllContent)
			{
				if (!createAllContent)
				{
					++this.LoadContentFalseCount;
				}

#pragma warning disable 618 // Call to obsolete method
				base.LoadGraphicsContent(createAllContent);
#pragma warning restore 618 // Call to obsolete method
			}

			/// <summary>
			///     Called when graphics resources should be released. Override to
			///     handle component specific graphics resources.
			/// </summary>
			/// <param name="destroyAllContent">
			///     True if all graphics resources should be released; false if only
			///     manual resources should be released.
			/// </param>
			protected override void UnloadGraphicsContent(bool destroyAllContent)
			{
				if (!destroyAllContent)
				{
					++this.UnloadContentFalseCount;
				}

#pragma warning disable 618 // Call to obsolete method
				base.UnloadGraphicsContent(destroyAllContent);
#pragma warning restore 618 // Call to obsolete method
			}
#pragma warning restore 672 // Overrides obsolete method

			/// <summary>
			///     Number of calls to the LoadContent() method with false as parameter
			/// </summary>
			public int LoadContentFalseCount;

			/// <summary>
			///     Number of calls to the UnloadContent() method with false as parameter
			/// </summary>
			public int UnloadContentFalseCount;
		}

		#endregion // class TestDrawable

		/// <summary>Verifies that the constructor is working</summary>
		[Test]
		public void TestConstructor()
		{
			using (var service = PrepareGlobalExclusiveMockedGraphicsDeviceService(callCreateDeviceOnInit: false))
			using (Drawable drawable = new TestDrawable(service))
			{
				Assert.IsNotNull(drawable);
			}
		}

		/// <summary>
		///     Tests whether the drawable can handle the graphics device being created
		///     after the drawable's constructor has already run
		/// </summary>
		[Test]
		public void TestCreateGraphicsDeviceAfterConstructor()
		{
			using (var service = PrepareGlobalExclusiveMockedGraphicsDeviceService(callCreateDeviceOnInit: false))
			using (Drawable drawable = new TestDrawable(service))
			using (IDisposable keeper = service.CreateDevice())
			{
				Assert.AreSame(drawable.GraphicsDevice, service.GraphicsDevice);
			}
		}

		/// <summary>
		///     Tests whether the drawable can handle the graphics device being created
		///     before the drawable's constructor has run
		/// </summary>
		[Test]
		public void TestCreateGraphicsDeviceBeforeConstructor()
		{
			using (var service = PrepareGlobalExclusiveMockedGraphicsDeviceService(callCreateDeviceOnInit: false))
			using (IDisposable keeper = service.CreateDevice())
			using (Drawable drawable = new TestDrawable(service))
			{
				Assert.AreSame(drawable.GraphicsDevice, service.GraphicsDevice);
			}
		}

		/// <summary>
		///     Tests whether the drawable can retrieve the graphics device service
		///     from a service provider
		/// </summary>
		[Test]
		public void TestCreateFromServiceProvider()
		{
			using (var service = PrepareGlobalExclusiveMockedGraphicsDeviceService(callCreateDeviceOnInit: false))
			{
				GameServiceContainer container = new GameServiceContainer();
				container.AddService(typeof (IGraphicsDeviceService), service);

				using (IDisposable keeper = service.CreateDevice())
				{
					using (Drawable drawable = new TestDrawable(container))
					{
						Assert.AreSame(drawable.GraphicsDevice, service.GraphicsDevice);
					}
				}
			}
		}

		/// <summary>
		///     Verifies that an exception is thrown if the drawable is constructed from
		///     a service provider which doesn't contain the graphics device service
		/// </summary>
		[Test]
		public void TestThrowOnMissingGraphicsDeviceService()
		{
			GameServiceContainer container = new GameServiceContainer();

			Assert.Throws<InvalidOperationException>(
													 delegate() { using (Drawable drawable = new TestDrawable(container)) { } }
				);
		}

		/// <summary>Verifies that the Draw() method can be called</summary>
		[Test]
		public void TestDraw()
		{
			using (var service = PrepareGlobalExclusiveMockedGraphicsDeviceService(callCreateDeviceOnInit: false))
			using (Drawable drawable = new TestDrawable(service))
			{
				drawable.Draw(new GameTime());
			}
		}

		/// <summary>
		///     Verifies that the Drawable class correctly responds to a graphics device reset
		/// </summary>
		[Test]
		public void TestGraphicsDeviceReset()
		{
			using (var service = PrepareGlobalExclusiveMockedGraphicsDeviceService(callCreateDeviceOnInit: false))
			using (IDisposable keeper = service.CreateDevice())
			{
				using (TestDrawable drawable = new TestDrawable(service))
				{
					Assert.AreEqual(0, drawable.LoadContentFalseCount);
					Assert.AreEqual(0, drawable.UnloadContentFalseCount);

					Assert.Throws<NotSupportedException>(() => service.ResetDevice());

					//Assert.AreEqual(1, drawable.LoadContentFalseCount);
					//Assert.AreEqual(1, drawable.UnloadContentFalseCount);
				}
			}
		}

		[Test]
		public void GraphicsDeviceDoesNotHaveReset()
		{
			var foundMethods = typeof (Microsoft.Xna.Framework.Graphics.GraphicsDevice).GetTypeInfo().GetMethods().Where(m => m.Name == "Reset").ToArray();
			Assert.IsEmpty(foundMethods, "MonoGame previously did not support GraphicsDevice.Reset, and now the method is present. Update Nuclectic.");
		}
	}
} // namespace Nuclex.Graphics

#endif // UNITTEST