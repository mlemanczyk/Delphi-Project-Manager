/*
 * Created by SharpDevelop.
 * User: SG0894652
 * Date: 8/10/2009
 * Time: 11:21 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using NUnit.Framework;

using DelphiProjectHandler.LibraryPaths;

namespace DelphiProjectHandler.Tests.LibraryPaths
{
	[TestFixture]
	public class DelphiRegistryKeysTests
	{
		[Test]
		public void LibraryKey()
		{
			Assert.IsNotNull(DelphiRegistryKeys.LibraryKey(LibraryPathType.Delphi5), "Delphi5 should be created");
			Assert.IsNotNull(DelphiRegistryKeys.LibraryKey(LibraryPathType.Delphi6), "Delphi5 should be created");
		}

		[Test]
		[ExpectedException(typeof(NotSupportedException))]
		public void LibraryKey_NotSupported()
		{
			DelphiRegistryKeys.LibraryKey(LibraryPathType.Delphi2009);
		}

		[Test]
		public void RootDirKey()
		{
			Assert.IsNotNull(DelphiRegistryKeys.RootDirKey(LibraryPathType.Delphi5), "Delphi5 should be created");
			Assert.IsNotNull(DelphiRegistryKeys.RootDirKey(LibraryPathType.Delphi6), "Delphi5 should be created");
		}

		[Test]
		[ExpectedException(typeof(NotSupportedException))]
		public void RootDirKey_NotSupported()
		{
			DelphiRegistryKeys.RootDirKey(LibraryPathType.Delphi2009);
		}

		[Test]
		public void EnvironmentPathsKey()
		{
			Assert.IsNotNull(DelphiRegistryKeys.EnvironmentPathsKey(LibraryPathType.Delphi6), "Delphi6 should be created");
		}

		[Test]
		[ExpectedException(typeof(NotSupportedException))]
		public void EnvironmentPathsKey_NotSupported()
		{
			DelphiRegistryKeys.EnvironmentPathsKey(LibraryPathType.Delphi5);
		}
	}
}
