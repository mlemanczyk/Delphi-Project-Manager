using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelphiProjectHandler.Operations.Uses
{
    public abstract class AbstractUsesListOperation: IUsesListOperation
    {
        private IUsesListOperationSettings fSettings;
        protected IUsesListOperationSettings Settings
        {
            get { return fSettings; }
            set { fSettings = value; }
        }

        private IUsesListOperationState fState;
        protected IUsesListOperationState State
        {
            get { return fState; }
        }

        protected virtual bool CanProcessUnit(UnitItem aUnit)
        {
            return (aUnit.Name != "" && State.Units.Contains(aUnit.Name));
        }

        protected virtual IUsesListOperationState CreateState(UnitList aUnitList)
        {
            UsesListOperationState vState = new UsesListOperationState();
            vState.Units = aUnitList;
            return vState;
        }

        protected abstract bool ProcessUnit(UnitList aUnits, UnitItem aUnit);

        public AbstractUsesListOperation(IUsesListOperationSettings aSettings)
        {
            Settings = aSettings;
        }

        public void Initialize(UnitList aUnitList)
        {
            fState = CreateState(aUnitList);
        }

        public bool CanProcess()
        {
            return (State.Units.Intersect(Settings.RequiredUnits).Count() == Settings.RequiredUnits.Count);
        }

        public bool Execute()
        {
            bool vListModified = false;
            foreach (string vUnitFileName in Settings.UnitsToManipulate)
            {
                UnitItem vUnit = Settings.UnitItemBuilder.Create(vUnitFileName);
                if (!CanProcessUnit(vUnit))
                    continue;

                vListModified = ProcessUnit(State.Units, vUnit) || vListModified;
            }

            return vListModified;
        }
    }
}
