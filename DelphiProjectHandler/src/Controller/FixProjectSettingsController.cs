using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DelphiProjectHandler.Model;
using DelphiProjectHandler.Dialogs.FileSelectors;
using DelphiProjectHandler.Dialogs.FileSelectors.Specialized;
using DelphiProjectHandler.Utils;

namespace DelphiProjectHandler.Controller
{
    public class FixProjectSettingsController: IFixProjectSettingsController
    {
        #region Private

        private IFixProjectSettings fModel;
        private IFilesSelector fDprFilesSelector;
        private IFilesSelector fPasFilesSelector;

        #endregion
        #region Protected

        protected IFilesSelector CreatePasFilesSelector()
        {
            return new PasFilesSelector(new FilesSelector());
        }

        protected IFilesSelector CreateDprFilesSelector()
        {
            return new DprFilesSelector(new FilesSelector());
        }

        protected void ImportFiles(string aDescription, string aFileMask, ICollection<string> aList)
        {
            ValidateModel();
            IFolderFilesImporter vImporter = new FolderFilesImporter();
            aList.Add(vImporter.SelectFiles(aDescription, aFileMask));
            Model.NotifyObservers();
        }

        protected void SelectFiles(IFilesSelector aFilesSelector, ICollection<string> aList)
        {
            ValidateModel();
            aList.Add(aFilesSelector.SelectFiles());
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

        protected IFilesSelector DprFilesSelector
        {
            get { return fDprFilesSelector; }
        }

        protected IFilesSelector PasFilesSelector
        {
            get { return fPasFilesSelector; }
        }

        #endregion
        #region Public

        public FixProjectSettingsController(): base()
        {
            fDprFilesSelector = CreateDprFilesSelector();
            fPasFilesSelector = CreatePasFilesSelector();
        }

        #endregion
        #region IFixProjectSettingsController Members

        public void ImportProjects()
        {
            ImportFiles("Select any folder containing your projects. Subfolders are automatically included.", "*.dpr", Model.Projects);
        }

        public void ImportUnits()
        {
            ImportFiles("Select any folder containing your units. Subfolders are automatically included.", "*.pas", Model.UnitsToManipulate);
        }

        public void SelectProjects()
        {
            SelectFiles(DprFilesSelector, Model.Projects);
        }

        public void SelectUnits()
        {
            SelectFiles(PasFilesSelector, Model.UnitsToManipulate);
        }

        public void SwitchAddUnitPathsToDPRs()
        {
            ValidateModel();
            Model.AddUnitPathsToDPRs = !Model.AddUnitPathsToDPRs;
            Model.NotifyObservers();
        }

        public void SetProjects(ICollection<string> aProjects)
        {
            SetFiles(aProjects, Model.Projects);
        }

        public void SetUnitConstraints(ICollection<string> aConstraints)
        {
            SetFiles(aConstraints, Model.UnitConstraints);
        }

        public void SetUnitsToManipulate(ICollection<string> aUnits)
        {
            SetFiles(aUnits, Model.UnitsToManipulate);
        }

        public IFixProjectSettings Model
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
