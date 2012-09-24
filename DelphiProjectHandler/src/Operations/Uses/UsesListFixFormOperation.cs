using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelphiProjectHandler.Operations.Uses
{
    public class UsesListFixFormOperation: AbstractUsesListOperation
    {
        protected override bool ProcessUnit(UnitList aUnits, UnitItem aUnit)
        {
            int vUnitIdx = aUnits.IndexOf(aUnit.Name);
            if (aUnits[vUnitIdx].Form == aUnit.Form)
                return false;

            UnitItem vToFix = aUnits[vUnitIdx];
            vToFix.Form = aUnit.Form;

            return true;
        }

        public UsesListFixFormOperation(IUsesListOperationSettings aSettings): base(aSettings)
        {
        }
    }
}
