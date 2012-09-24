using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DelphiProjectHandler.Model;
using DelphiProjectHandler.LibraryPaths;
using DelphiProjectHandler.Dialogs;

namespace DelphiProjectHandler.Controller
{
    public class LibraryPathSettingsController: ILibraryPathSettingsController
    {
        #region (Private)

        private ILibraryPathSettings fModel;

        #endregion

        #region (Protected)

        protected void ValidateModel()
        {
            if (Model == null)
                throw new ArgumentNullException("Model", "Model is not assigned to the controller");
        }

        #endregion

        #region (ILibraryPathSettingsController Implementation)

        public void LoadPreset(LibraryPathType aType)
        {
            ValidateModel();
            ILibraryPathsProvider vProvider = LibraryPathsProviderFactory.CreateProvider(aType);
            Model.LibraryPaths = vProvider.List();
            Model.NotifyObservers();
        }

        public void NormalizePaths()
        {
            ValidateModel();
            Model.LibraryPaths = PathUtils.Normalize(Model.LibraryPaths);
            Model.NotifyObservers();
        }

        public void SetLibraryPaths(ICollection<string> aLibraryPaths)
        {
            ValidateModel();
            Model.LibraryPaths = new List<string>(aLibraryPaths);
            Model.NotifyObservers();
        }

        public ILibraryPathSettings Model
        {
            get
            {
                return fModel;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Model");
                fModel = value;
            }
        }

        #endregion
    }
}
