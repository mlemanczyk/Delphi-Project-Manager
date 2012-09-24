using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DelphiProjectHandler.Dialogs.FileSelectors
{
    public class FilesSelector: IFilesSelector
    {
        #region Private

        private IFilesSelectorOptions fDefaultOptions;

        #endregion
        #region Protected

        protected IFilesSelectorOptions CreateDefaultOptions()
        {
            IFilesSelectorOptions vOptions = new FilesSelectorOptions();
            vOptions.Filter = "Pas files|*.pas|All files|*.*";
            vOptions.DefaultExt = "pas";
            vOptions.MultiSelect = true;
            vOptions.Title = "Select one or more units to add";
            return vOptions;
        }

        #endregion
        #region Public

        public FilesSelector()
            : base()
        {
            fDefaultOptions = CreateDefaultOptions();
        }

        #endregion
        #region IFilesSelctor Implementation

        public IFilesSelectorOptions DefaultOptions
        {
            get { return fDefaultOptions; }
        }

        public ICollection<string> SelectFiles(IFilesSelectorOptions aOptions)
        {
            OpenFileDialog vDialog = new OpenFileDialog();
            vDialog.Filter = aOptions.Filter;
            vDialog.DefaultExt = aOptions.DefaultExt;
            vDialog.Multiselect = aOptions.MultiSelect;
            vDialog.SupportMultiDottedExtensions = true;
            vDialog.Title = aOptions.Title;

            if (vDialog.ShowDialog() != DialogResult.OK)
                return new List<string>();

            return vDialog.FileNames;
        }

        public ICollection<string> SelectFiles()
        {
            return SelectFiles(DefaultOptions);
        }

        #endregion
    }
}
