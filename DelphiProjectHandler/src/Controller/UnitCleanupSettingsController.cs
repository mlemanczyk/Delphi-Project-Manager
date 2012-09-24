using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DelphiProjectHandler.Model;
using System.IO;
using System.Windows.Forms;

namespace DelphiProjectHandler.Controller
{
    public class UnitCleanupSettingsController: IUnitCleanupSettingsController
    {
        #region (Private)

        private IUnitCleanupSettings fModel;

        #endregion
        #region (IUnitCleanupSettingsController Implementation)

        public void LoadFromLibraryPaths(ICollection<string> aLibraryPaths)
        {
            List<string> vCompleteUnitList = new List<string>();
            foreach (string vPath in aLibraryPaths)
            {
                if (vPath.Equals(""))
                    continue;

                try
                {
                    IList<string> vUnits = Directory.GetFiles(vPath, "*.pas", SearchOption.TopDirectoryOnly);
                    vCompleteUnitList.AddRange(vUnits);
                }
                catch (Exception iError)
                {
                    throw new IOException("Error occurred when processing path: " + vPath + ". " + iError.Message);
                }
            }
            Model.UnitsToClean = vCompleteUnitList;
            Model.NotifyObservers();
        }

        public void PasteIcarusReport()
        {
            Model.IcarusReport = Clipboard.GetText();
            Model.NotifyObservers();
        }

        public void SetIcarusReport(string aReport)
        {
            Model.IcarusReport = aReport;
            Model.NotifyObservers();
        }

        public void SetIgnoredUnits(ICollection<string> aUnits)
        {
            Model.IgnoredUnits = new List<string>(aUnits);
            Model.NotifyObservers();
        }

        public void SetUnitsToClean(ICollection<string> aUnits)
        {
            Model.UnitsToClean = new List<string>(aUnits);
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
