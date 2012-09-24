using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelphiProjectHandler.Dialogs.FileSelectors.Specialized
{
    public class XmlFileSelector: IFilesSelector
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
            vOptions.Filter = "XML files|*.xml|All files|*.*";
            vOptions.DefaultExt = "xml";
            vOptions.MultiSelect = false;
            vOptions.Title = "Select existing XML file";
            return vOptions;
        }

        #endregion
        #region Public

        public XmlFileSelector(IFilesSelector aBaseSelector): base()
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
