using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelphiProjectHandler.Operations.Uses
{
    public class UsesListDeleteOperation : AbstractUsesListOperation
    {
        protected override bool ProcessUnit(UnitList aUnits, UnitItem aUnit)
        {
            int vUnitIdx = aUnits.IndexOf(aUnit.Name);
            UnitItem vToDelete = aUnits[vUnitIdx];
            if (vToDelete.HasStartingConditions && !vToDelete.HasEndingConditions && vUnitIdx < aUnits.Count - 1)
                aUnits[vUnitIdx + 1].StartingConditions = vToDelete.StartingConditions;
            else
                if (vToDelete.HasEndingConditions && !vToDelete.HasStartingConditions && vUnitIdx > 0)
                    aUnits[vUnitIdx - 1].EndingConditions = vToDelete.EndingConditions;

            aUnits.RemoveAt(vUnitIdx);
            return true;
        }

        public UsesListDeleteOperation(IUsesListOperationSettings aSettings)
            : base(aSettings)
        {
        }
    }
}
