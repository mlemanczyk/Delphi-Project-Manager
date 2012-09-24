using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DelphiProjectHandler.View;

namespace DelphiProjectHandler.Model
{
    public class FixUnitSettings: IFixUnitSettings
    {
        #region (Private)

        private IList<string> fUnitsToModify = new List<string>();
        private IList<string> fInterfaceUnits = new List<string>();
        private IList<string> fImplementationUnits = new List<string>();
        private IList<string> fUnitConstraints = new List<string>();
        private ICollection<IFixUnitSettingsView> fObservers = new List<IFixUnitSettingsView>();

        #endregion
        #region (Protected)

        protected ICollection<IFixUnitSettingsView> Observers
        {
            get { return fObservers; }
        }

        #endregion
        #region (IFixUnitSettings Implementation)

        public IList<string> UnitsToModify
        {
            get
            {
                return fUnitsToModify;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("UnitsToModify");
                fUnitsToModify = value;
            }
        }

        public IList<string> InterfaceUnits
        {
            get
            {
                return fInterfaceUnits;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("InterfaceUnits");
                fInterfaceUnits = value;
            }
        }

        public IList<string> ImplementationUnits
        {
            get
            {
                return fImplementationUnits;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("ImplementationUnits");
                fImplementationUnits = value;
            }
        }

        public IList<string> UnitConstraints
        {
            get
            {
                return fUnitConstraints;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("UnitConstraints");
                fUnitConstraints = value;
            }
        }

        public void AddObserver(IFixUnitSettingsView aView)
        {
            Observers.Add(aView);
        }

        public void RemoveObserver(IFixUnitSettingsView aView)
        {
            Observers.Remove(aView);
        }

        public void NotifyObservers()
        {
            foreach (IFixUnitSettingsView vObserver in Observers)
                vObserver.UpdateView(this);
        }
    }

        #endregion
}
