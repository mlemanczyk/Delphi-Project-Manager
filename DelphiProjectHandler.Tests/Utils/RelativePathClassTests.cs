using System;
using System.IO;
using NUnit.Framework;

using DelphiProjectHandler.Dialogs;

namespace DelphiProjectHandler.Tests.Utils
{
    [TestFixture]
    public class RelativePathClassTests
    {
        #region (Test methods)

        protected string GetTestFailMessage(string iBasePath, string iPath)
        {
            return string.Format("Base path: {0}, Destination path: {1}", iBasePath, iPath);
        }

        protected void DoGetRelativePath(string iBasePath, string iPath, string iExpected)
        {
            try
            {
                string vActual = RelativePath.GetRelativePath(iBasePath, iPath);
                Assert.AreEqual(iExpected, vActual, GetTestFailMessage(iBasePath, iPath));
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message + "; " + GetTestFailMessage(iBasePath, iPath));
            }
        }

        protected void DoGetRelativePath(string iBasePath, string iPath)
        {
            DoGetRelativePath(iBasePath, iPath, iPath);
        }

        protected void DoGetRelativeFileName(bool iRelativeToPath, string iBaseFileName, string iFileName, string iExpected)
        {
            try
            {
                if (iRelativeToPath)
                	iBaseFileName = PathUtils.AddEndingDirectorySeparator(Path.GetDirectoryName(iBaseFileName));

                string vActual = RelativePath.GetRelativeFileName(iBaseFileName, iFileName);
                Assert.AreEqual(iExpected, vActual, GetTestFailMessage(iBaseFileName, iFileName));
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message + "; " + GetTestFailMessage(iBaseFileName, iFileName));
            }
        }

        protected void DoGetRelativeFileName(bool iToBasePath, string iBaseFileName, string iFileName)
        {
            DoGetRelativeFileName(iToBasePath, iBaseFileName, iFileName, iFileName);
        }

        protected void DoGetRelativeFileNameByFolder(string iBaseFolder, string iFileName, string iExpected)
        {
            string vActual = RelativePath.GetRelativeFilenameByFolder(iBaseFolder, iFileName);
            Assert.AreEqual(iExpected, vActual, GetTestFailMessage(iBaseFolder, iFileName));
        }

        protected void DoGetRelativeFileNameByFolder(string iBaseFolder, string iFileName)
        {
            DoGetRelativeFileNameByFolder(iBaseFolder, iFileName, iFileName);
        }

        #endregion

        [Test]
        public void GetRelativePath_DifferentRoots()
        {
            DoGetRelativePath(@"c:\windows\temp", @"d:\windows\temp\");
        }

        [Test]
        public void GetRelativePath_Rooted_NotRooted()
        {
            DoGetRelativePath(@"c:\windows\temp", @"windows\temp\");
        }

        [Test]
        public void GetRelativePath_Rooted()
        {
            DoGetRelativePath(@"c:\Windows\temp", @"c:\windows\temp\folder1\folder2", @"folder1\folder2\");
            DoGetRelativePath(@"c:\windows\temp\folder1\folder2", @"c:\windows\temp\", @"..\..\");
            DoGetRelativePath(@"c:\windows\temp\folder1\folder2", @"c:\windows\temp\folder3\folder4", @"..\..\folder3\folder4\");
            DoGetRelativePath(@"c:\windows\temp\", "", @"c:\windows\temp\");
            DoGetRelativePath(@"c:\windows\temp", @"c:\windows\temp", "");
        }

        [Test]
        public void GetRelativePath_Rooted_RootFolder()
        {
            DoGetRelativePath(@"c:\windows\temp", @"\windows\temp\folder1\folder2", @"folder1\folder2\");
            DoGetRelativePath(@"c:\windows\temp\folder1\folder2", @"\windows\temp\", @"..\..\");
            DoGetRelativePath(@"c:\windows\temp\folder1\folder2", @"\windows\temp\folder3\folder4", @"..\..\folder3\folder4\");
            DoGetRelativePath(@"c:\windows\temp\", @"\", @"..\..\");
        }

        [Test]
        public void GetRelativePath_NotRooted()
        {
            DoGetRelativePath(@"Windows\temp", @"windows\temp\folder1\folder2", @"folder1\folder2\");
            DoGetRelativePath(@"windows\temp\folder1\folder2", @"windows\temp\", @"..\..\");
            DoGetRelativePath(@"windows\temp\folder1\folder2", @"windows\temp\folder3\folder4", @"..\..\folder3\folder4\");
            DoGetRelativePath(@"windows\temp\", "", @"windows\temp\");
            DoGetRelativePath(@"windows\temp", @"windows\temp", "");

            DoGetRelativePath(@"\windows\temp", @"c:\windows\temp\folder1\folder2", @"folder1\folder2\");
            DoGetRelativePath(@"\windows\temp\folder1\folder2", @"c:\windows\temp\", @"..\..\");
            DoGetRelativePath(@"\windows\temp\folder1\folder2", @"c:\windows\temp\folder3\folder4", @"..\..\folder3\folder4\");
            DoGetRelativePath(@"\windows\temp\", "", @"\windows\temp\");
            DoGetRelativePath(@"\windows\temp", @"c:\windows\temp", "");
        }

        [Test]
        public void GetRelativePath_NotRooted_Rooted()
        {
            DoGetRelativePath(@"windows\temp", @"c:\windows\temp\folder1\folder2\");
            DoGetRelativePath(@"windows\temp\folder1\folder2", @"c:\windows\temp\");
            DoGetRelativePath(@"windows\temp\folder1\folder2", @"c:\windows\temp\folder3\folder4\");
            DoGetRelativePath(@"windows\temp\", "", @"windows\temp\");
            DoGetRelativePath(@"windows\temp", @"c:\windows\temp\");
        }

        [Test]
        public void GetRelativeFileName_DifferentRoots()
        {
        	for (int vIdx = 0; vIdx <= 1; vIdx++)
            {
        		bool vToBasePath = Convert.ToBoolean(vIdx);
                DoGetRelativeFileName(vToBasePath, @"c:\windows\temp\file1.txt", @"d:\windows\temp\file2.txt");

                DoGetRelativeFileName(vToBasePath, @"c:\windows\temp\file1.txt", @"windows\temp\file2.txt");

                DoGetRelativeFileName(vToBasePath, @"c:\windows\temp\file1.txt", @"c:\windows\temp\folder1\folder2\file2.txt", @"folder1\folder2\file2.txt");
                DoGetRelativeFileName(vToBasePath, @"c:\windows\temp\folder1\folder2\file1.txt", @"c:\windows\temp\file2.txt", @"..\..\file2.txt");
                DoGetRelativeFileName(vToBasePath, @"c:\windows\temp\folder1\folder2\file1.txt", @"c:\windows\temp\folder3\folder4\file2.txt", @"..\..\folder3\folder4\file2.txt");
                DoGetRelativeFileName(vToBasePath, @"c:\windows\temp\file1.txt", "file2.txt", @"c:\windows\temp\file2.txt");
                DoGetRelativeFileName(vToBasePath, @"c:\windows\temp\file1.txt", @"c:\windows\temp\file2.txt", "file2.txt");

                DoGetRelativeFileName(vToBasePath, @"c:\windows\temp\file1.txt", @"\windows\temp\folder1\folder2\file2.txt", @"folder1\folder2\file2.txt");
                DoGetRelativeFileName(vToBasePath, @"c:\windows\temp\folder1\folder2\file1.txt", @"\windows\temp\file2.txt", @"..\..\file2.txt");
                DoGetRelativeFileName(vToBasePath, @"c:\windows\temp\folder1\folder2\file1.txt", @"\windows\temp\folder3\folder4\file2.txt", @"..\..\folder3\folder4\file2.txt");
                DoGetRelativeFileName(vToBasePath, @"c:\windows\temp\file1.txt", @"\file2.txt", @"..\..\file2.txt");

                DoGetRelativeFileName(vToBasePath, @"\windows\temp\file1.txt", @"c:\windows\temp\folder1\folder2\file2.txt", @"folder1\folder2\file2.txt");
                DoGetRelativeFileName(vToBasePath, @"\windows\temp\folder1\folder2\file1.txt", @"c:\windows\temp\file2.txt", @"..\..\file2.txt");
                DoGetRelativeFileName(vToBasePath, @"\windows\temp\folder1\folder2\file1.txt", @"c:\windows\temp\folder3\folder4\file2.txt", @"..\..\folder3\folder4\file2.txt");
                DoGetRelativeFileName(vToBasePath, @"\windows\temp\file1.txt", @"c:\windows\temp\file2.txt", "file2.txt");
                DoGetRelativeFileName(vToBasePath, @"\windows\temp\file1.txt", "file2.txt", @"\windows\temp\file2.txt");

                DoGetRelativeFileName(vToBasePath, @"windows\temp\file1.txt", @"windows\temp\folder1\folder2\file2.txt", @"folder1\folder2\file2.txt");
                DoGetRelativeFileName(vToBasePath, @"windows\temp\folder1\folder2\file1.txt", @"windows\temp\file2.txt", @"..\..\file2.txt");
                DoGetRelativeFileName(vToBasePath, @"windows\temp\folder1\folder2\file1.txt", @"windows\temp\folder3\folder4\file2.txt", @"..\..\folder3\folder4\file2.txt");
                DoGetRelativeFileName(vToBasePath, @"windows\temp\file1.txt", "file2.txt", @"windows\temp\file2.txt");
                DoGetRelativeFileName(vToBasePath, @"windows\temp\file1.txt", @"windows\temp\file2.txt", "file2.txt");

                DoGetRelativeFileName(vToBasePath, @"windows\temp\file1.txt", @"c:\windows\temp\folder1\folder2\file2.txt");
                DoGetRelativeFileName(vToBasePath, @"windows\temp\folder1\folder2\file1.txt", @"c:\windows\temp\file2.txt");
                DoGetRelativeFileName(vToBasePath, @"windows\temp\folder1\folder2\file1.txt", @"c:\windows\temp\folder3\folder4\file2.txt");
                DoGetRelativeFileName(vToBasePath, @"windows\temp\file1.txt", "file2.txt", @"windows\temp\file2.txt");
                DoGetRelativeFileName(vToBasePath, @"windows\temp\file1.txt", @"c:\windows\temp\file2.txt");
            }
        }

        [Test]
        public void IsPathRooted()
        {
            Assert.IsTrue(Path.IsPathRooted(@"c:\temp"), @"c:\temp");
            Assert.IsTrue(Path.IsPathRooted(@"c:\"), @"c:\");
            Assert.IsTrue(Path.IsPathRooted(@"c:"), @"c:");
            Assert.IsTrue(Path.IsPathRooted(@"\temp"), @"\temp");
            Assert.IsFalse(Path.IsPathRooted(@"temp"), @"temp");
        }

        [Test]
        public void GetRelativeFileName()
        {
            DoGetRelativeFileName(false, @"c:\Temp\folder1\folder2\filename1.txt", @"c:\temp\folder1\filename2.txt", @"..\filename2.txt");
        }

        [Test]
        public void GetRelativeFileNameByFolder()
        {
            DoGetRelativeFileNameByFolder(@"c:\Temp\folder1\folder2", @"c:\temp\filename1.txt", @"..\..\filename1.txt");
            DoGetRelativeFileNameByFolder(@"", @"c:\temp\filename1.txt");
        }
    }
}
