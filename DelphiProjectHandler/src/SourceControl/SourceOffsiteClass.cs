using System;
using System.Diagnostics;
using System.IO;

using DelphiProjectHandler.Dialogs;

namespace DelphiProjectHandler.SourceControl
{
    public class SourceOffsite
    {
        protected bool WasFileCheckedOut(string iResult)
        {
            return iResult.StartsWith("Checked out file:");
        }

        protected void Initialize(
            string iServerName, 
               int iServerPort, 
            string iAlias, 
            string iUserName, 
            string iPassword,
            string iBaseFolder)
        {
            ServerName = iServerName;
            ServerPort = iServerPort;
            Alias = iAlias;
            UserName = iUserName;
            Password = iPassword;
            BaseFolder = iBaseFolder;
        }

        protected virtual string GetApplicationFileName()
        {
            return @"c:\program files\SourceOffsite\soscmd.exe";
        }

        protected string GetCommandParameters(string iFileName)
        {
            string vFileName = Path.GetFileName(iFileName);
            string vProjectPath = Path.GetDirectoryName(iFileName);
            if (vProjectPath.StartsWith(BaseFolder))
            {
                vProjectPath = vProjectPath.Substring(BaseFolder.Length);
                vProjectPath = vProjectPath.TrimStart(Path.DirectorySeparatorChar);
                vProjectPath = PathUtils.RemoveEndingDirectorySeparator(vProjectPath);
            }
            vProjectPath = vProjectPath.Replace(Path.DirectorySeparatorChar, '/');

            return string.Format(
                @"-command CheckoutFile -server {0}:{1} " +
                @"-alias {2} -name {3} -password {4} " +
                @"-project $/{5} -file {6}",
                ServerName, ServerPort, Alias, UserName, Password, vProjectPath, vFileName);
        }

        protected virtual string RunCommand(string iFileName)
        {
            iFileName = iFileName.Trim();
            if (iFileName == "")
                throw new ArgumentException("File name to checkout can not be empty", "iFileName");

            Process vProcess = new Process();
            vProcess.StartInfo.FileName = GetApplicationFileName();
            vProcess.StartInfo.Arguments = GetCommandParameters(iFileName);
            vProcess.StartInfo.CreateNoWindow = true;
            vProcess.StartInfo.ErrorDialog = false;
            vProcess.StartInfo.RedirectStandardError = true;
            vProcess.StartInfo.RedirectStandardInput = true;
            vProcess.StartInfo.RedirectStandardOutput = true;
            vProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            vProcess.StartInfo.UseShellExecute = false;
            vProcess.Start();

            vProcess.WaitForExit();
            return vProcess.StandardOutput.ReadToEnd();
        }

        public SourceOffsite(
            string iServerName, 
               int iServerPort, 
            string iAlias, 
            string iUserName, 
            string iPassword,
            string iBaseFolder)
        {
            Initialize(iServerName, iServerPort, iAlias, iUserName, iPassword, iBaseFolder);
        }

        public bool CheckoutFile(string iFileName)
        {
            string vResult = RunCommand(iFileName);
            return WasFileCheckedOut(vResult);
        }

        public string ServerName
        {
            get { return fServerName; }
            set { fServerName = value; }
        }
        protected string fServerName;

        public int ServerPort
        {
            get { return fServerPort; }
            set { fServerPort = value; }
        }
        private int fServerPort;

        public string UserName
        {
            get { return fUserName; }
            set { fUserName = value; }
        }
        protected string fUserName;

        public string Password
        {
            get { return fPassword; }
            set { fPassword = value; }
        }
        protected string fPassword;

        public string Alias
        {
            get { return fAlias; }
            set { fAlias = value; }
        }
        protected string fAlias;

        public string BaseFolder
        {
            get { return fBaseFolder; }
            set { fBaseFolder = value; }
        }
        private string fBaseFolder;
    }
}
