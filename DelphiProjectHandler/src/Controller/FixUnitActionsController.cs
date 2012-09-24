using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DelphiProjectHandler.Model;
using DelphiProjectHandler.Operations;
using DelphiProjectHandler.Operations.Units;

namespace DelphiProjectHandler.Controller
{
    public class FixUnitActionsController: IFixUnitActionsController
    {
        #region Private

        private IFixUnitSettings fModel;

        #endregion
        #region IFixUnitActionsController Members

        public void AddUnits()
        {
            IDelphiFileOperation vOperation = new DelphiUnitAddOperation(Model.InterfaceUnits, Model.ImplementationUnits, Model.UnitConstraints);
            IDelphiFileAgent vAgent = new DelphiFileAgent();
            foreach (string vUnitFilename in Model.UnitsToModify)
                vAgent.Execute(vUnitFilename, vOperation);
        }

        public void RemoveUnits()
        {
            throw new NotImplementedException();
        }

        public Model.IFixUnitSettings Model
        {
            get { return fModel; }
            set {
                if (value == null)
                    throw new ArgumentNullException("Model");
                fModel = value;
            }
        }

        #endregion
    }
}
