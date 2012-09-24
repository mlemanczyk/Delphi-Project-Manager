using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using DelphiProjectHandler.LibraryPaths;

namespace DelphiProjectHandler.Tests.LibraryPaths
{
    [TestFixture]
    public class DelphiLibraryPathBuilderTests
    {
        [Test]
        public void Build()
        {
            Dictionary<string, string> vEnvironmentStrings = new Dictionary<string, string>();
            vEnvironmentStrings.Add("BASA_DM", "$(DM_TP)\\Basa");
            vEnvironmentStrings.Add("DM_TP", "$(DM_VIEW)\\THIRDPARTY");
            vEnvironmentStrings.Add("DM_VIEW", "d:\\DM\\sg894652_2006.2_ccrc\\dispmgr");
            vEnvironmentStrings.Add("FCSUtils", "$(DM_TP)\\FcsUtils");
            vEnvironmentStrings.Add("delphi", "c:\\program files\\borland\\delphi5");

            List<string> vPaths = new List<string>();
            vPaths.Add("$(DM_View)\\DPR");
            vPaths.Add("$(DM_TP)\\BASA");
            vPaths.Add("$(Delphi)\\Lib");
            vPaths.Add("C:\\source\\dm\\latest");

            ILibraryPathsBuilder vBuilder = new DelphiLibraryPathsBuilder();
            ICollection<string> vActual = vBuilder.Build(vPaths, vEnvironmentStrings);
            Assert.AreEqual(4, vActual.Count, "Invalid no. of returned paths");
            Assert.IsTrue(vActual.Contains(@"d:\dm\sg894652_2006.2_ccrc\dispmgr\dpr"), "Path 0 wasn't found");
            Assert.IsTrue(vActual.Contains(@"d:\dm\sg894652_2006.2_ccrc\dispmgr\thirdparty\basa"), "Path 1 wasn't found");
            Assert.IsTrue(vActual.Contains(@"c:\program files\borland\delphi5\lib"), "Path 2 wasn't found");
            Assert.IsTrue(vActual.Contains(@"c:\source\dm\latest"), "Path 3 wasn't found");
        }
    }
}
