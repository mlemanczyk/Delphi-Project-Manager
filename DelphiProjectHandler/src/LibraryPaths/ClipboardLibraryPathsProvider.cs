using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DelphiProjectHandler.LibraryPaths
{
    public class ClipboardLibraryPathsProvider: ILibraryPathsProvider
    {
        #region ILibraryPathsProvider Members

        public IList<string> List()
        {
            string vPathsStr = Clipboard.GetText();
            string[] vPaths = vPathsStr.Split(';');
            return new List<string>(vPaths);
        }

        #endregion
    }
}
