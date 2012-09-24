using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DelphiProjectHandler.View;

namespace DelphiProjectHandler.Model
{
    public class FixProjectSettings: IFixProjectSettings
    {
        #region (Private)

        private bool fAddUnitPathsToDPRs = true;
        private IList<string> fProjects = new List<string>();
        private IList<string> fUnitsToManipulate = new List<string>();
        private IList<string> fUnitConstraints = new List<string>();
        private IList<IFixProjectSettingsView> fObservers = new List<IFixProjectSettingsView>();

        #endregion
        #region (Protected)

        protected IList<IFixProjectSettingsView> Observers 
        { 
            get { return fObservers; } 
        }

        #endregion

        #region (IFixProjectSettingsModel implementation)

        public bool AddUnitPathsToDPRs
        {
            get
            {
                return fAddUnitPathsToDPRs;
            }

            set
            {
                fAddUnitPathsToDPRs = value;
            }
        }

        public IList<string> Projects
        {
            get
            {
                return fProjects;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Projects");

                fProjects = value;
            }
        }

        public IList<string> UnitsToManipulate
        {
            get
            {
                return fUnitsToManipulate;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("UnitsToManipulate");

                fUnitsToManipulate = value;
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
            }
        }

        public void AddObserver(View.IFixProjectSettingsView aView)
        {
            Observers.Add(aView);
        }

        public void RemoveObserver(View.IFixProjectSettingsView aView)
        {
            Observers.Remove(aView);
        }

        public void NotifyObservers()
        {
            foreach (IFixProjectSettingsView vObserver in Observers)
                vObserver.UpdateView(this);
        }

        #endregion
    }
}
