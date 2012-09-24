using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DelphiProjectHandler.View;

namespace DelphiProjectHandler.Model
{
    public class UnitCleanupSettings: IUnitCleanupSettings
    {
        #region (Private)

        private IList<IUnitCleanupSettingsView> fObservers = new List<IUnitCleanupSettingsView>();
        private IList<string> fUnitsToClean = new List<string>();
        private IList<string> fIgnoredUnits = new List<string>();
        private string fIcarusReport = "";

        #endregion
        #region (Protected)

        protected IList<IUnitCleanupSettingsView> Observers
        {
            get { return fObservers; }
        }

        #endregion
        #region (IUnitCleanupSettings Implementation)

        public IList<string> UnitsToClean
        {
            get
            {
                return fUnitsToClean;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("UnitsToClean");
                fUnitsToClean = value;
            }
        }

        public IList<string> IgnoredUnits
        {
            get
            {
                return fIgnoredUnits;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("IgnoredUnits");
                fIgnoredUnits = value;
            }
        }

        public string IcarusReport
        {
            get
            {
                return fIcarusReport;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("IcarusReport");
                fIcarusReport = value;
            }
        }

        public void AddObserver(View.IUnitCleanupSettingsView aView)
        {
            Observers.Add(aView);
        }

        public void RemoveObserver(View.IUnitCleanupSettingsView aView)
        {
            Observers.Remove(aView);
        }

        public void NotifyObservers()
        {
            foreach (IUnitCleanupSettingsView vObserver in Observers)
                vObserver.UpdateView(this);
        }

        #endregion
    }
}
