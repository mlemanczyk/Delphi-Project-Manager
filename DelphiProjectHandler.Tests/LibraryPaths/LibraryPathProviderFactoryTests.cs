using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

using DelphiProjectHandler.LibraryPaths;

namespace DelphiProjectHandler.Tests.LibraryPaths
{
    [TestFixture]
    public class LibraryPathProviderFactoryTests
    {
        protected void TestCreateProvider(LibraryPathType iLibraryType, Type iExpectedType, string iErrorMsg)
        {
            ILibraryPathsProvider vProvider = LibraryPathsProviderFactory.CreateProvider(iLibraryType);
            Assert.IsNotNull(vProvider, "Provider was not created. " + iErrorMsg);
            Assert.IsInstanceOf(iExpectedType, vProvider, "Created provider is of wrong type. " + iErrorMsg);
        }

        [Test]
        public void CreateProvider_Supported()
        {
            TestCreateProvider(LibraryPathType.Delphi5, typeof(DelphiLibraryPathsProvider), "Test 1");
            TestCreateProvider(LibraryPathType.Delphi6, typeof(DelphiLibraryPathsProvider), "Test 2");
            TestCreateProvider(LibraryPathType.Clipboard, typeof(ClipboardLibraryPathsProvider), "Test 3");
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void CreateProvider_NotSupported_Delphi2005()
        {
            LibraryPathsProviderFactory.CreateProvider(LibraryPathType.Delphi2005);
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void CreateProvider_NotSupported_Delphi2007()
        {
            LibraryPathsProviderFactory.CreateProvider(LibraryPathType.Delphi2007);
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void CreateProvider_NotSupported_Delphi7()
        {
            LibraryPathsProviderFactory.CreateProvider(LibraryPathType.Delphi7);
        }

        [Test]
        public void ListSupportedProviders()
        {
            LibraryPathType[] vSupported = LibraryPathsProviderFactory.ListSupportedProviders();
            Assert.AreEqual(3, vSupported.Length, "No. of supported providers is wrong");
            Assert.AreEqual(LibraryPathType.Delphi5, vSupported[0], "Item 0 is wrong");
            Assert.AreEqual(LibraryPathType.Delphi6, vSupported[1], "Item 1 is wrong");
            Assert.AreEqual(LibraryPathType.Clipboard, vSupported[2], "Item 2 is wrong");
        }

        [Test]
        public void IsProviderSupported_Supported()
        {
            Assert.IsTrue(LibraryPathsProviderFactory.IsProviderSupported(LibraryPathType.Delphi5), "Delphi5 should be supported");
            Assert.IsTrue(LibraryPathsProviderFactory.IsProviderSupported(LibraryPathType.Delphi6), "Delphi6 should be supported");
            Assert.IsTrue(LibraryPathsProviderFactory.IsProviderSupported(LibraryPathType.Clipboard), "Clipboard should be supported");
        }

        [Test]
        public void IsProviderSupported_NotSupported()
        {
            Assert.IsFalse(LibraryPathsProviderFactory.IsProviderSupported(LibraryPathType.Delphi7), "Delphi7 should not be supported");
            Assert.IsFalse(LibraryPathsProviderFactory.IsProviderSupported(LibraryPathType.Delphi2005), "Delphi2005 should not be supported");
            Assert.IsFalse(LibraryPathsProviderFactory.IsProviderSupported(LibraryPathType.Delphi2007), "Delphi2007 should not be supported");
            Assert.IsFalse(LibraryPathsProviderFactory.IsProviderSupported(LibraryPathType.Delphi2009), "Delphi2009 should not be supported");
        }
    }
}
