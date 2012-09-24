/*
 * Created by SharpDevelop.
 * User: SG0894652
 * Date: 8/12/2009
 * Time: 10:49 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;

namespace DelphiProjectHandler.Operations
{
	/// <summary>
	/// Description of FileModifierAgent.
	/// </summary>
	public class FileModifierAgent
	{
		public FileModifierAgent(string iFileName)
		{
            Load(iFileName);
		}

		public virtual void Save(string iFileName)
        {
            string vTempFileName = iFileName + ".tmp";
            string vBackupFileName = iFileName + ".backup";

            File.SetAttributes(iFileName, FileAttributes.Normal);
            File.WriteAllText(vTempFileName, Content);
            File.Replace(vTempFileName, iFileName, vBackupFileName, false);
            fFileName = iFileName;
            fOriginalContent = Content;
        }

        public virtual void Load(string iFileName)
        {
            fFileName = iFileName;
            fOriginalContent = File.ReadAllText(iFileName);
            Content = OriginalContent;
        }

        public string FileName
        {
            get { return fFileName; }
        }
        protected string fFileName;

        public string Content
        {
            get { return fContent; }
            set { fContent = value; }
        }
        protected string fContent;

        public string OriginalContent
        {
            get { return fOriginalContent; }
        }
        protected string fOriginalContent;

        public bool Modified
        {
            get { return OriginalContent != Content; }
        }
	}
}
