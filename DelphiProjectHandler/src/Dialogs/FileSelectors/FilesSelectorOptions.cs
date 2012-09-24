using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelphiProjectHandler.Dialogs.FileSelectors
{
    public class FilesSelectorOptions: IFilesSelectorOptions
    {
        #region (Private)

        private string fTitle;
        private string fFilter;
        private string fDefaultExt;
        private bool fMultiSelect;

        #endregion
        #region (IFilesSelectorOptions Implementation)

        public string Title
        {
            get
            {
                return fTitle;
            }
            set
            {
                fTitle = value;
            }
        }

        public string Filter
        {
            get
            {
                return fFilter;
            }
            set
            {
                fFilter = value;
            }
        }

        public string DefaultExt
        {
            get
            {
                return fDefaultExt;
            }
            set
            {
                fDefaultExt = value;
            }
        }

        public bool MultiSelect
        {
            get
            {
                return fMultiSelect;
            }
            set
            {
                fMultiSelect = value;
            }
        }

        #endregion
    }
}
