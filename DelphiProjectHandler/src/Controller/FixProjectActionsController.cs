using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DelphiProjectHandler.Model;
using DelphiProjectHandler.Operations;
using DelphiProjectHandler.Operations.Projects;

namespace DelphiProjectHandler.Controller
{
    public class FixProjectActionsController: IFixProjectActionsController
    {
        #region Private

        private IFixProjectSettings fModel;
        private IDelphiFileAgent fDelphiFileAgent;

        #endregion
        #region Protected

        protected void ExecuteOperation(IDelphiFileOperation aOperation)
        {
            foreach (string vProjectName in Model.Projects)
            {
                if (vProjectName == "")
                    continue;

                fDelphiFileAgent.Execute(vProjectName, aOperation);
            }
        }

        #endregion
        #region Public

        public FixProjectActionsController()
        {
            fDelphiFileAgent = new DelphiFileAgent();
        }

        #endregion
        #region IProjectManipulator Implementation

        public void AddUnits()
        {
            IDelphiFileOperation vOperation = new DelphiProjectAddUnitOperation(Model.UnitConstraints, Model.UnitsToManipulate, Model.AddUnitPathsToDPRs);
            ExecuteOperation(vOperation);
        }

        public void RemoveUnits()
        {
            IDelphiFileOperation vOperation = new DelphiProjectRemoveUnitOperation(Model.UnitConstraints, Model.UnitsToManipulate, Model.AddUnitPathsToDPRs);
            ExecuteOperation(vOperation);
        }

        public void FixPaths()
        {
            IDelphiFileOperation vOperation = new DelphiProjectFixPathsOperation(Model.UnitConstraints, Model.UnitsToManipulate, Model.AddUnitPathsToDPRs);
            ExecuteOperation(vOperation);
        }

        public void FixForms()
        {
            IDelphiFileOperation vOperation = new DelphiProjectFixFormsOperation(Model.UnitConstraints, Model.UnitsToManipulate, Model.AddUnitPathsToDPRs);
            ExecuteOperation(vOperation);
        }

        public IFixProjectSettings Model
        {
            get { return fModel; }
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
