using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DelphiProjectHandler.Dialogs.FileSelectors;

namespace DelphiProjectHandler.Dialogs.FileSelectors.Specialized
{
    public class DprFilesSelector: IFilesSelector
    {
        #region Private

        private IFilesSelector fBaseSelector;
        private IFilesSelectorOptions fDefaultOptions;

        #endregion
        #region Protected

        protected IFilesSelector BaseSelector
        {
            get { return fBaseSelector; }
        }

        protected IFilesSelectorOptions CreateDefaultOptions()
        {
            IFilesSelectorOptions vOptions = new FilesSelectorOptions();
            vOptions.Filter = "DPR files|*.dpr|All files|*.*";
            vOptions.DefaultExt = "dpr";
            vOptions.MultiSelect = true;
            vOptions.Title = "Select one or more projects";
            return vOptions;
        }

        #endregion
        #region Public

        public DprFilesSelector(IFilesSelector aBaseSelector): base()
        {
            fBaseSelector = aBaseSelector;
            fDefaultOptions = CreateDefaultOptions();
        }

        #endregion
        #region IFilesSelector Members

        public IFilesSelectorOptions DefaultOptions
        {
            get { return fDefaultOptions; }
        }

        public ICollection<string> SelectFiles(IFilesSelectorOptions aOptions)
        {
            return BaseSelector.SelectFiles(aOptions);
        }

        public ICollection<string> SelectFiles()
        {
            return SelectFiles(DefaultOptions);
        }

        #endregion
    }
}
