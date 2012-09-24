using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

using DelphiProjectHandler.Dialogs;

namespace DelphiProjectHandler.Tests.Utils
{
    [TestFixture]
    public class PathUtilsClassTests
    {
        #region (Helper methods)

		protected void CompareLists(IList<string> iActual, IList<string> iExpected, string iErrorMsg)
		{
			Assert.AreEqual(iExpected.Count, iActual.Count, "Different no. of items returned.");
			for (int vItemIdx = 0; vItemIdx < iActual.Count; vItemIdx++)
				Assert.AreEqual(iExpected[vItemIdx], iActual[vItemIdx], "Different items found. Item: " + vItemIdx.ToString() + ". " + iErrorMsg);
		}
		
        protected void DoFail(string iMessage, string iCorrectPath, string iPathToFix)
        {
            Assert.Fail(iMessage + "; " + GetPathsMessage(iCorrectPath, iPathToFix));
        }

        protected string GetPathsMessage(string iBasePath, string iPath)
        {
            return string.Format("Base path: {0}; Path: {1}", iBasePath, iPath);
        }

        #endregion
        #region (Test methods)

        protected void TestConcatFolders(int iStartIndex, string[] iFolders, string iExpected)
        {
            string vActual = PathUtils.ConcatFolders(iStartIndex, iFolders);
            Assert.AreEqual(iExpected, vActual);
        }

        protected void TestBuildGoUpPath(int iCount, string iExpected)
        {
            string vActual = PathUtils.BuildGoUpPath(iCount);
            Assert.AreEqual(iExpected, vActual, "Number of levels: " + iCount.ToString());
        }

        protected void TestSplitPaths(string iPath, string[] iExpected)
        {
            string[] vActual = PathUtils.SplitPath(iPath);
            Assert.AreEqual(iExpected.Length, vActual.Length, "Different number of folders returned. Path: " + iPath);
            for (int vFolderIdx = 0; vFolderIdx < vActual.Length; vFolderIdx++)
                Assert.AreEqual(iExpected[vFolderIdx], vActual[vFolderIdx], string.Format("Wrong folder. Path: {0}, Index: {1}", iPath, vFolderIdx.ToString()));
        }

        protected void TestFixRootPath(string iCorrectPath, string iPathToFix, string iExpected)
        {
            try
            {
                string vActual = PathUtils.FixRootPath(iCorrectPath, iPathToFix);
                Assert.AreEqual(iExpected, vActual, GetPathsMessage(iCorrectPath, iPathToFix));
            }
            catch (Exception e)
            {
                DoFail(e.Message, iCorrectPath, iPathToFix);
            }
        }

        protected void TestFixRootPath(string iBasePath)
        {
            try
            {
                string vRootPath = "";
                if (iBasePath != "")
                    vRootPath = Path.GetPathRoot(iBasePath).TrimEnd(Path.DirectorySeparatorChar);
                TestFixRootPath(iBasePath, @"\temp2", vRootPath + @"\temp2");
                TestFixRootPath(iBasePath, @"d:\temp2", @"d:\temp2");
                TestFixRootPath(iBasePath, @"temp2", @"temp2");
                TestFixRootPath(iBasePath, @"", @"");
            }
            catch (Exception e)
            {
                DoFail(e.Message, iBasePath, "<unknown>");
            }
        }

        protected void TestCombine(string iBasePath, string iAddedPath, string iExpected, bool iShouldFail)
        {
            string vActual = "";
            try
            {
                vActual = PathUtils.Combine(iBasePath, iAddedPath);
                if (iShouldFail)
                    DoFail("Exception should be raised, while it wasn't", iBasePath, iAddedPath);
            }
            catch (ArgumentException e)
            {
                if (!iShouldFail)
                    DoFail("Exception was raised while it shouldn't. " + e.Message, iBasePath, iAddedPath);
                else
                    return;
            }
            Assert.AreEqual(iExpected, vActual, GetPathsMessage(iBasePath, iAddedPath));
        }

        protected void TestCombine(string iBasePath)
        {
            try
            {
                string vBasePath = PathUtils.AddEndingDirectorySeparator(iBasePath);
                TestCombine(iBasePath, @"c:\", @"c:\", iBasePath != "");
                TestCombine(iBasePath, @"c:\temp", @"c:\temp", iBasePath != "");
                TestCombine(iBasePath, @"c:", @"c:", iBasePath != "");
                TestCombine(iBasePath, @"\temp", vBasePath + @"temp", false);
                TestCombine(iBasePath, @"temp", vBasePath + @"temp", false);
                TestCombine(iBasePath, @"\", vBasePath, false);
                TestCombine(iBasePath, @"", vBasePath, false);
            }
            catch (Exception e)
            {
                DoFail(e.Message, iBasePath, "");
            }
        }

        protected void TestRemoveEndingDirectorySeparator(string iPath, string iExpected)
        {
            string vActual = PathUtils.RemoveEndingDirectorySeparator(iPath);
            Assert.AreEqual(iExpected, vActual, "Test value: " + iPath);
        }

        protected void TestAddEndingDirectorySeparator(string iPath, string iExpected)
        {
            string vActual = PathUtils.AddEndingDirectorySeparator(iPath);
            Assert.AreEqual(iExpected, vActual, "Test value: " + iPath);
        }

        protected void TestAddEndingDirectorySeparator_List(IList<string> iPaths, IList<string> iExpected)
        {
            IList<string> vActual = PathUtils.AddEndingDirectorySeparator(iPaths);
            CompareLists(vActual, iExpected, "");
        }

        protected void TestHaveSameRoots(string iLeftPath, string iRightPath, bool iExpected)
        {
            bool vActual = PathUtils.HaveSameRoots(iLeftPath, iRightPath);
            Assert.AreEqual(iExpected, vActual, "Left path: " + iLeftPath + "; Right path: " + iRightPath);
        }

        protected void TestGetFirstDifferentFolderIdx(string[] iLeftFolders, string[] iRightFolders, int iExpected)
        {
            int vActual = PathUtils.GetFirstDifferentFolderIdx(iLeftFolders, iRightFolders);
            Assert.AreEqual(iExpected, vActual);
        }
        
        protected void TestIsValidPath(string iPath, bool iExpected, string iErrorMsg)
        {
        	bool vActual = PathUtils.IsValidPath(iPath);
        	Assert.AreEqual(iExpected, vActual, "Path was recognized incorrectly. " + iErrorMsg);
        }
        
        protected void TestRemoveInvalidPaths(IList<string> iPaths, IList<string> iExpected)
        {
        	IList<string> vActual = PathUtils.RemoveInvalidPaths(iPaths);
        	Assert.AreEqual(iExpected.Count, vActual.Count, "Wrong no. of items was returned");
        	for (int vItemIdx = 0; vItemIdx < vActual.Count; vItemIdx++)
        		Assert.AreEqual(iExpected[vItemIdx], vActual[vItemIdx], "Paths are different. Index: " + vItemIdx.ToString());
        }
        
        protected void TestNormalize(IList<string> iPaths, IList<string> iExpected)
        {
        	IList<string> vActual = PathUtils.Normalize(iPaths);
        	CompareLists(vActual, iExpected, "");
        }

        #endregion

        [Test]
        public void ConcatFolders_StartOn1st()
        {
            TestConcatFolders(0, new string[] { "folder1" }, @"folder1\");
            TestConcatFolders(0, new string[] { "folder1", "folder2" }, @"folder1\folder2\");
            TestConcatFolders(0, new string[] { "folder1", "folder2", @"folder3\" }, @"folder1\folder2\folder3\");
        }

        [Test]
        public void ConcatFolders_StartOn2nd()
        {
            TestConcatFolders(1, new string[] { "folder1" }, @"");
            TestConcatFolders(1, new string[] { "folder1", "folder2" }, @"folder2\");
            TestConcatFolders(1, new string[] { "folder1", "folder2", @"folder3\" }, @"folder2\folder3\");
        }

        [Test]
        public void ConcatFolders_StartOn3rd()
        {
            TestConcatFolders(2, new string[] { "folder1" }, @"");
            TestConcatFolders(2, new string[] { "folder1", "folder2" }, @"");
            TestConcatFolders(2, new string[] { "folder1", "folder2", @"folder3\" }, @"folder3\");
        }

        [Test]
        public void ConcatFolders_InvalidIndex()
        {
            TestConcatFolders(-1, new string[] { "folder1", "folder2" }, @"");
        }

        [Test]
        public void BuildGoUpPath()
        {
            TestBuildGoUpPath(-1, @"");
            TestBuildGoUpPath(0, @"");
            TestBuildGoUpPath(1, @"..\");
            TestBuildGoUpPath(2, @"..\..\");
            TestBuildGoUpPath(3, @"..\..\..\");
        }

        [Test]
        public void SplitPaths()
        {
            TestSplitPaths(@"temp1", new string[] { "temp1" });
            TestSplitPaths(@"temp1\temp2", new string[] { "temp1", "temp2" });
            TestSplitPaths(@"c:\temp1\temp2", new string[] { "c:", "temp1", "temp2" });
            TestSplitPaths(@"c:\temp1\temp2\", new string[] { "c:", "temp1", "temp2", "" });
            TestSplitPaths(@"\temp1\temp2\", new string[] { "", "temp1", "temp2", "" });
            TestSplitPaths(@"\temp1\temp2", new string[] { "", "temp1", "temp2" });
            TestSplitPaths(@"", new string[] { });
        }

        [Test]
        public void FixRootPath()
        {
            TestFixRootPath(@"c:\");
            TestFixRootPath(@"temp1");
            TestFixRootPath(@"\temp1");
            TestFixRootPath(@"");
        }

        [Test]
        public void Combine()
        {
            TestCombine(@"c:\");
            TestCombine(@"c:\temp1");
            TestCombine(@"c:");
            TestCombine(@"\temp1");
            TestCombine(@"temp1");
            TestCombine(@"\");
            TestCombine(@"");
        }

        [Test]
        public void RemoveEndingDirectorySeparator()
        {
            TestRemoveEndingDirectorySeparator(@"c:\", @"c:\");
            TestRemoveEndingDirectorySeparator(@"c:\temp", @"c:\temp");
            TestRemoveEndingDirectorySeparator(@"c:\temp\", @"c:\temp");
            TestRemoveEndingDirectorySeparator(@"\", @"\");
            TestRemoveEndingDirectorySeparator(@"", @"");
            TestRemoveEndingDirectorySeparator(@"\temp\", @"\temp");
        }

        [Test]
        public void AddEndingDirectorySeparator()
        {
            TestAddEndingDirectorySeparator(@"c:\temp", @"c:\temp\");
            TestAddEndingDirectorySeparator(@"c:\", @"c:\");
            TestAddEndingDirectorySeparator(@"c:\temp\", @"c:\temp\");
            TestAddEndingDirectorySeparator(@"\", @"\");
            TestAddEndingDirectorySeparator(@"", @"");
            TestAddEndingDirectorySeparator(@"c:", @"c:\");
        }

        [Test]
        public void AddEndingDirectorySeparator_List()
        {
        	TestAddEndingDirectorySeparator_List(
        		new string[] {@"c:\temp", @"c:\", @"c:\temp\", @"\", "", "c:"},
        		new string[] {@"c:\temp\", @"c:\", @"c:\temp\", @"\", "", @"c:\"});
        }
        
        [Test]
        public void HaveSameRoots()
        {
            TestHaveSameRoots(@"c:\", @"c:\temp", true);
            TestHaveSameRoots(@"c:\", @"c:\", true);
            TestHaveSameRoots(@"c:\Temp\", @"c:\temp", true);
            TestHaveSameRoots(@"\temp", @"\temp2", true);
            TestHaveSameRoots(@"\", @"\", true);
            TestHaveSameRoots(@"c:\", @"d:\", false);
            TestHaveSameRoots(@"c:\temp", @"d:\temp", false);
            TestHaveSameRoots(@"\temp", @"c:\temp", false);
        }

        [Test]
        public void GetFirstDifferentFolderIdx()
        {
            TestGetFirstDifferentFolderIdx(
                new string[] {"folder1", "folder2", "folder3"},
                new string[] {"folder1", "folder2", "folder5"},
                2);
            TestGetFirstDifferentFolderIdx(
                new string[] { "folder1" },
                new string[] { "folder2" },
                0);
            TestGetFirstDifferentFolderIdx(
                new string[] { "folder1", "folder2" },
                new string[] { "folder1", "folder2", "folder3", "folder4" },
                2);
            TestGetFirstDifferentFolderIdx(
                new string[] { "folder1", "folder2", "folder3", "folder4" },
                new string[] { "folder1", "folder2" },
                2);
            TestGetFirstDifferentFolderIdx(
                new string[] { "folder1", "folder2" },
                new string[] { "folder1", "folder2" },
                2);
            TestGetFirstDifferentFolderIdx(
                new string[] { "folder1" },
                new string[] { "folder1" },
                1);
            TestGetFirstDifferentFolderIdx(
                new string[] { },
                new string[] { },
                0);
        }
        
        [Test]
        public void IsValidPath()
        {
        	TestIsValidPath(@"C:\", true, "Test 1");
        	TestIsValidPath(@"C:\Windows", true, "Test 2");
        	TestIsValidPath(@"C:\Windows\", true, "Test 3");
        	TestIsValidPath(@"C:\1234", false, "Test 4");
        	TestIsValidPath(@"C:\Windows\abc123", false, "Test 5");
        }
        
        [Test]
        public void RemoveInvalidPaths()
        {
        	TestRemoveInvalidPaths(
        		new string[] {@"C:\", @"c:\12345", @"C:\windows", @"c:\windows\abc123", @"C:\windows\", @"c:\1234\abc"}, 
        		new string[] {@"C:\", @"C:\windows", @"C:\windows\"});
        }
        
        [Test]
        public void Normalize()
        {
			TestNormalize(
				new string[] {@"C:\WINDOWS", null, @"C:\", @"", @"C:\123abc", @"C:\windows", @"c:\windows\", @"", @"c:\windows\123abc\", null},
				new string[] {@"c:\", @"c:\windows\"}
			);
        }
	}
}
