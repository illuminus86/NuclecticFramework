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

using Microsoft.Xna.Framework.Content;
using Nuclectic.Fonts;
using Nuclectic.Graphics.Helpers;
using Nuclectic.Tests.Mocks;
#if UNITTEST
using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Nuclectic.Tests.Fonts
{
	/// <summary>Unit tests for the vector font class</summary>
	[TestFixture(IgnoreReason = "Unstable, may freeze test runner.")]
	[RequiresSTA]
	public class VectorFontTest
		: TestFixtureBase
	{
		#region class KerningPairTest

		/// <summary>Unit tests for the vector font's kerning pair structure</summary>
		[TestFixture]
		public class KerningPairTest
		{
			/// <summary>
			///     Validates that the constructor of the kerning pair structure is working
			/// </summary>
			[Test]
			public void TestConstructor()
			{
				KerningPair pair = new KerningPair('A', 'B');

				Assert.AreEqual('A', pair.Left);
				Assert.AreEqual('B', pair.Right);
			}

			/// <summary>
			///     Validates that the constructor of the kerning pair structure is working
			/// </summary>
			[Test]
			public void TestGetHashCode()
			{
				KerningPair pair1 = new KerningPair('A', 'B');
				KerningPair pair2 = new KerningPair('A', 'B');

				// Can't check for inequality, always returning "123" as hash code would
				// be legal (even if not exactly the crown of efficiency :D)
				Assert.AreEqual(pair1.GetHashCode(), pair2.GetHashCode());
			}

			/// <summary>Verifies that the Equals() method is implemented correctly</summary>
			[Test]
			public void TestEqualityCheck()
			{
				KerningPair pair1 = new KerningPair('A', 'B');
				KerningPair pair2 = new KerningPair('A', 'B');
				KerningPair pair3 = new KerningPair('B', 'C');

				Assert.IsTrue(pair1.Equals(pair2));
				Assert.IsFalse(pair2.Equals(pair3));
			}

			/// <summary>
			///     Tests whether the Equals() method can cope with an incompatible object
			/// </summary>
			[Test]
			public void TestEqualityCheckAgainstIncompatibleType()
			{
				KerningPair pair = new KerningPair('A', 'B');
				Assert.IsFalse(pair.Equals(new object()));
			}
		}

		#endregion // class KerningPairTest

		/// <summary>Tests whether the line height of a vector font can be obtained</summary>
		[Test]
		public void TestGetLineHeight() { Assert.Greater(this.vectorFont.LineHeight, 0); }

		/// <summary>
		///     Verifies that the characters of the vector font can be accessed
		/// </summary>
		[Test]
		public void TestCharacters()
		{
			for (int index = 0; index < this.vectorFont.Characters.Count; ++index)
			{
				var character = this.vectorFont.Characters[index];
				Assert.IsNotNull(character);
			}
		}

		/// <summary>Tests whether the character map points to valid characters</summary>
		[Test]
		public void TestCharacterMap()
		{
			foreach (KeyValuePair<char, int> character in this.vectorFont.CharacterMap)
			{
				Assert.GreaterOrEqual(character.Value, 0);
				Assert.Less(character.Value, this.vectorFont.Characters.Count);
			}
		}

		/// <summary>Tests whether kerning table lists valid characters</summary>
		[Test]
		public void TestKerningTable()
		{
			foreach (var entry in this.vectorFont.KerningTable)
			{
				char left = entry.Key.Left;
				char right = entry.Key.Right;

				Assert.That(this.vectorFont.CharacterMap.ContainsKey(left));
				Assert.That(this.vectorFont.CharacterMap.ContainsKey(right));
			}
		}

		/// <summary>Tests whether outlined text can be constructed by the font</summary>
		[Test]
		public void TestOutlining()
		{
			char testChar = getFirstVisibleCharacter();
			var text = this.vectorFont.Outline(new string(testChar, 3));
			Assert.IsNotNull(text); // nonsense; avoids compiler warning
		}

		/// <summary>Tests whether filled text can be constructed by the font</summary>
		[Test]
		public void TestFilling()
		{
			char testChar = getFirstVisibleCharacter();
			var text = this.vectorFont.Fill(new string(testChar, 3));
			Assert.IsNotNull(text); // nonsense; avoids compiler warning
		}

		/// <summary>Tests whether extruded text can be constructed by the font</summary>
		[Test]
		public void TestExtrusion()
		{
			char testChar = getFirstVisibleCharacter();
			var text = this.vectorFont.Extrude(new string(testChar, 3));
			Assert.IsNotNull(text); // nonsense; avoids compiler warning
		}

		/// <summary>Called before each test is run</summary>
		[SetUp]
		public void Setup()
		{
			this.mockedGraphicsDeviceService = PrepareGlobalExclusiveMockedGraphicsDeviceService();

			this.contentManager = mockedGraphicsDeviceService.CreateResourceContentManager(Resources.UnitTestResources.ResourceManager);

			this.vectorFont = this.contentManager.Load<VectorFont>("UnitTestVectorFont");
		}

		/// <summary>Called after each test has run</summary>
		[TearDown]
		public void Teardown()
		{
			if (this.contentManager != null)
			{
				this.contentManager.Dispose();
				this.contentManager = null;
			}
			if (this.mockedGraphicsDeviceService != null)
			{
				this.mockedGraphicsDeviceService.DestroyDevice();
				this.mockedGraphicsDeviceService = null;
			}
		}

		/// <summary>Retrieves the first visible character in the font</summary>
		/// <returns>The first visible character in the font</returns>
		private char getFirstVisibleCharacter()
		{
			foreach (KeyValuePair<char, int> character in this.vectorFont.CharacterMap)
			{
				int index = character.Value;

				if (this.vectorFont.Characters[index].Outlines.Count > 0)
				{
					return character.Key;
				}
			}

			throw new InvalidOperationException("No visible characters found");
		}

		/// <summary>Mocked graphics device service used for unit testing</summary>
		private IMockedGraphicsDeviceService mockedGraphicsDeviceService;

		/// <summary>Content manager used to load the vector font</summary>
		private ResourceContentManager contentManager;

		/// <summary>Vector font being tested</summary>
		private VectorFont vectorFont;
	}
} // namespace Nuclex.Fonts

#endif // UNITTEST