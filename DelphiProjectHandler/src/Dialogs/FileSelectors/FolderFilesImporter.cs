using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DelphiProjectHandler.Dialogs.FileSelectors
{
    public class FolderFilesImporter: IFolderFilesImporter
    {
        public ICollection<string> SelectFiles(string aDescription, string aFileMask)
        {
            FolderBrowserDialog vDialog = new FolderBrowserDialog();
            vDialog.ShowNewFolderButton = false;
            vDialog.Description = aDescription;
            if (vDialog.ShowDialog() != DialogResult.OK)
                return new List<string>();

            return Directory.GetFiles(vDialog.SelectedPath, aFileMask, SearchOption.AllDirectories);
        }
    }
}
