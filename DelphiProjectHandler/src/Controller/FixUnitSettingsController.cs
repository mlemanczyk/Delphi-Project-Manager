using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DelphiProjectHandler.Dialogs.FileSelectors;
using DelphiProjectHandler.Dialogs.FileSelectors.Specialized;
using DelphiProjectHandler.Model;
using DelphiProjectHandler.Utils;

namespace DelphiProjectHandler.Controller
{
    public class FixUnitSettingsController: IFixUnitSettingsController
    {
        #region Private

        private IFixUnitSettings fModel;
        private IFilesSelector fPasFilesSelector;

        #endregion
        #region Protected

        protected IFilesSelector PasFilesSelector
        {
            get { return fPasFilesSelector; }
        }

        private IFilesSelector CreatePasFilesSelector()
        {
            return new PasFilesSelector(new FilesSelector());
        }

        protected void ImportFiles(string aDescription, string aFileMask, ICollection<string> aDestinationList)
        {
            ValidateModel();
            IFolderFilesImporter vImporter = new FolderFilesImporter();
            aDestinationList.Add(vImporter.SelectFiles(aDescription, aFileMask));
            Model.NotifyObservers();
        }

        protected void SelectFiles(IList<string> aDestList)
        {
            ValidateModel();
            aDestList.Add(PasFilesSelector.SelectFiles());
            Model.NotifyObservers();
        }

        protected void SetFiles(ICollection<string> aNewFiles, IList<string> aDestList)
        {
            ValidateModel();
            aDestList.Clear();
            aDestList.Add(aNewFiles);
            Model.NotifyObservers();
        }

        protected void ValidateModel()
        {
            if (Model == null)
                throw new ArgumentNullException("Model", "Model is not assigned to the controller");
        }

        #endregion
        #region Public

        public FixUnitSettingsController()
            : base()
        {
            fPasFilesSelector = CreatePasFilesSelector();
        }

        #endregion
        #region IFixUnitSettingsController Members

        public void ImportUnitsToModify()
        {
            ImportFiles("Select a folder to import Delphi units from. Subfolders are automatically included.", "*.pas", Model.UnitsToModify);
        }

        public void SelectUnitsToModify()
        {
            SelectFiles(Model.UnitsToModify);
        }

        public void ImportInterfaceUnits()
        {
            ImportFiles("Select a folder to import Delphi units from. Subfolders are automatically included.", "*.pas", Model.InterfaceUnits);
        }

        public void SelectInterfaceUnits()
        {
            SelectFiles(Model.InterfaceUnits);
        }

        public void ImportImplementationUnits()
        {
            ImportFiles("Select a folder to import Delphi units from. Subfolders are automatically included.", "*.pas", Model.ImplementationUnits);
        }

        public void SelectImplementationUnits()
        {
            SelectFiles(Model.ImplementationUnits);
        }

        public void SetUnitsToModify(ICollection<string> aUnits)
        {
            SetFiles(aUnits, Model.UnitsToModify);
        }

        public void SetInterfaceUnits(ICollection<string> aUnits)
        {
            SetFiles(aUnits, Model.InterfaceUnits);
        }

        public void SetImplementationUnits(ICollection<string> aUnits)
        {
            SetFiles(aUnits, Model.ImplementationUnits);
        }

        public void SetUnitConstraints(ICollection<string> aConstraints)
        {
            SetFiles(aConstraints, Model.UnitConstraints);
        }

        public Model.IFixUnitSettings Model
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
