using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DelphiProjectHandler.Model;
using System.IO;
using DelphiProjectHandler.Operations;

namespace DelphiProjectHandler.Controller
{
    public class UnitCleanupActionsController: IUnitCleanupActionsController
    {
        #region (Private)

        private IUnitCleanupSettings fModel;

        #endregion
        #region (IUnitCleanupActionsController Implementation)

        public void RemoveUnusedUnits()
        {
            SuggestedUnitStructureList vSuggestedStructures = IcarusAnalyzerReportParser.Parse(Model.IcarusReport);
            StringBuilder vLog = new StringBuilder();
            foreach (string vUnitFileName in Model.UnitsToClean)
            {
                if (vUnitFileName.Equals(""))
                    continue;

                string vUnitName = Path.GetFileNameWithoutExtension(vUnitFileName).ToLower();
                if (
                    !vSuggestedStructures.ContainsKey(vUnitName) ||
                    (vSuggestedStructures[vUnitName].MoveToInterface.Count == 0) &&
                    (vSuggestedStructures[vUnitName].ToDelete.Count == 0)
                   )
                {
                    continue;
                }

                vLog.Append("Processing unit " + vUnitName + "\r\n");
                DelphiUnitAgent vAgent = new DelphiUnitAgent(vUnitFileName);
                vAgent.CleanupUnits(vSuggestedStructures[vUnitName], Model.IgnoredUnits);
                if (vAgent.Modified)
                {
                    vLog.Append("Saving changes in unit " + vUnitName + "\r\n");
                    vAgent.Save(vUnitFileName);
                }
            }
            Model.IcarusReport = vLog.ToString();
            Model.NotifyObservers();
        }

        public IUnitCleanupSettings Model
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
