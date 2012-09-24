using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelphiProjectHandler.Dialogs.FileSelectors
{
    public interface IFolderFilesImporter
    {
        ICollection<string> SelectFiles(string aDescription, string aFileMask);
    }
}
