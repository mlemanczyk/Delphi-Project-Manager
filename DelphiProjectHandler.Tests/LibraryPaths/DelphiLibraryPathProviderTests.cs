using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using DelphiProjectHandler.LibraryPaths;

namespace DelphiProjectHandler.Tests.LibraryPaths
{
    [TestFixture]
    class DelphiLibraryPathProviderTests
    {
        [Test]
        public void Delphi5Paths()
        {
            ILibraryPathsProvider vProvider = new DelphiLibraryPathsProvider(LibraryPathType.Delphi5);
            IList<string> vLibraryPaths = vProvider.List();
            Assert.GreaterOrEqual(vLibraryPaths.IndexOf("c:\\program files\\borland\\delphi5\\lib"), 0, "Test 1");
            Assert.GreaterOrEqual(vLibraryPaths.IndexOf("d:\\dm\\sg894652_5.3_ccrc\\dispmgr\\u"), 0, "Test 2");
        }

        [Test]
        public void Delphi6Paths()
        {
            ILibraryPathsProvider vProvider = new DelphiLibraryPathsProvider(LibraryPathType.Delphi6);
            IList<string> vLibraryPaths = vProvider.List();
            Assert.GreaterOrEqual(vLibraryPaths.IndexOf("c:\\program files\\borland\\delphi6\\lib\\"), 0, "Test 1");
//            Assert.GreaterOrEqual(vLibraryPaths.IndexOf("d:\\dm\\sg894652_2006.2_ccrc\\dispmgr\\u"), 0, "Test 2");
        }
    }
}
