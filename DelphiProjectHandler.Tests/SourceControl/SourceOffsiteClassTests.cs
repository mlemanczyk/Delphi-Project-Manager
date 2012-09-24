using System;
using System.IO;
using NUnit.Framework;

using DelphiProjectHandler.SourceControl;

namespace DelphiProjectHandler.Tests.SourceControl
{
    [TestFixture]
    public class SourceOffsiteClassTests: SourceOffsite
    {
        #region (Overrides)
        
        public SourceOffsiteClassTests()
            :
            base("fcsweb4.dev.sabre.com", 8888, "ft-latest", "mpl", "mpl", @"d:\source\ft-latest")
        {
        }

        protected override string RunCommand(string iFileName)
        {
            if (!MockRunCommand)
                return base.RunCommand(iFileName);
            else
                if (MockSuccess)
                    return TestValueSuccessful;
                else
                    return TestValueUnsuccessful;
        }

        protected override string GetApplicationFileName()
        {
            if (!MockGetApplicationFileName)
                return base.GetApplicationFileName();
            else
                if (MockSuccess)
                    return @"c:\program files\SourceOffsite\soscmd.exe";
                else
                    return @"c:\program files\SourceOffsite\soscmd_wrong.exe";
        }

        #endregion
        #region (Properties)

        protected bool MockRunCommand
        {
            get { return fMockRunCommand; }
            set { fMockRunCommand = value; }
        }
        private bool fMockRunCommand = false;

        protected bool MockGetApplicationFileName
        {
            get { return fMockGetApplicationFileName; }
            set { fMockGetApplicationFileName = value; }
        }
        private bool fMockGetApplicationFileName;

        protected bool MockSuccess
        {
            get { return fMockSuccess; }
            set { fMockSuccess = value; }
        }
        private bool fMockSuccess;

        protected string TestValueSuccessful
        {
            get { return fTestValueSuccessful; }
            set { fTestValueSuccessful = value; }
        }
        private string fTestValueSuccessful = "";

        protected string TestValueUnsuccessful
        {
            get { return fTestValueUnsuccessful; }
            set { fTestValueUnsuccessful = value; }
        }
        private string fTestValueUnsuccessful = "";

        #endregion
        #region (Test methods)

        protected void TestGetCommandParameters(string iProjectPath, string iFileName)
        {
            string vExpected = string.Format(
                @"-command CheckoutFile -server {0}:{1} " +
                @"-alias {2} -name {3} -password {4} " +
                @"-project $/{5} -file {6}",
                ServerName, ServerPort, Alias, UserName, Password, iProjectPath, Path.GetFileName(iFileName));

            string vActual = GetCommandParameters(iFileName);
            Assert.AreEqual(vExpected, vActual, "Wrong command line was returned. Project path: {0}, File name: {1}", iProjectPath, iFileName);
        }

        protected void TestCheckoutFile(string iFileName, bool iExpected)
        {
            MockRunCommand = true;
            try
            {
                MockSuccess = iExpected;
                bool vActual = CheckoutFile(iFileName);
                Assert.AreEqual(iExpected, vActual, "Wrong checkout result. FileName: " + iFileName);
            }
            finally
            {
                MockRunCommand = false;
            }
        }

        #endregion
        #region (Setup/TearDown)

        [SetUp]
        protected virtual void Setup()
        {
            TestValueSuccessful = "Checked out file: ftadmin.dpr\r\n";
            TestValueUnsuccessful = "Unable to lock file: ftadmin.dpr\r\n";
        }

        [TearDown]
        protected virtual void TearDown()
        {
        }

        #endregion

        [Test]
        public void CheckoutFile()
        {
            TestCheckoutFile(@"d:\source\ft-latest\all\dpr\ftadmin.dpr", true);
            TestCheckoutFile(@"d:\source\ft-latest\all\dpr\ftadmin.dpr", false);
        }

        [Test]
        public void WasFileCheckedOut()
        {
            bool vActual = WasFileCheckedOut(TestValueSuccessful);
            Assert.IsTrue(vActual, "Successful checkout was not recognized");
            vActual = WasFileCheckedOut(TestValueUnsuccessful);
            Assert.IsFalse(vActual, "Unsuccessful checkout was not recognized");
        }

        [Test]
        //[Ignore("Not on VPN")]
        public void RunCommand()
        {
            TestRunCommand(false, true, @"d:\source\ft-latest\all\dpr\test_project.dpr", "Error:  Server response = 401 NoSuchFileOrProject\r\nError:  Server response = 401 NoSuchFileOrProject\r\n");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void RunCommand_EmptyFileName()
        {
            TestRunCommand(false, true, "", "");
        }

        [Test]
        [ExpectedException(typeof(System.ComponentModel.Win32Exception))]
        public void RunCommand_WrongApplicationName()
        {
            TestRunCommand(true, false, "doesn't have meaning.txt", "");
        }

        private void TestRunCommand(bool iMockBehavior, bool iSuccessful, string iFileName, string iExpected)
        {
            MockGetApplicationFileName = iMockBehavior;
            try
            {
                MockSuccess = iSuccessful;
                string vActual = RunCommand(iFileName);
                Assert.AreEqual(iExpected, vActual, "File Name: " + iFileName);
            }
            finally
            {
                MockGetApplicationFileName = false;
            }
        }

        [Test]
        public void TestCreate()
        {
            SourceOffsiteClassTests vControl = new SourceOffsiteClassTests();
            Assert.AreEqual("fcsweb4.dev.sabre.com", vControl.ServerName, "ServerName is wrong");
            Assert.AreEqual(8888, vControl.ServerPort, "ServerPort is wrong");
            Assert.AreEqual("ft-latest", vControl.Alias, "Alias is wrong");
            Assert.AreEqual("mpl", vControl.UserName, "UserName is wrong");
            Assert.AreEqual("mpl", vControl.Password, "Password is wrong");
            Assert.AreEqual(@"d:\source\ft-latest", vControl.BaseFolder, "BaseFolder is wrong");
        }

        [Test]
        public void GetCommandParameters()
        {
            TestGetCommandParameters(@"all/dpr", @"d:\source\ft-latest\all\dpr\ftadmin.dpr");
            TestGetCommandParameters(@"", @"ftadmin.dpr");
        }
    }
}
