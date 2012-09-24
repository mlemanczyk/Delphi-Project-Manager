using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelphiProjectHandler.Operations.Uses
{
    public class UsesListFixPathOperation: AbstractUsesListOperation
    {
        protected override bool ProcessUnit(UnitList aUnits, UnitItem aUnit)
        {
            int vUnitIdx = aUnits.IndexOf(aUnit.Name);
            UnitItem iToFix = aUnits[vUnitIdx];
            iToFix.UsePath = aUnit.UsePath;
            iToFix.Path = aUnit.Path;

            return true;
        }

        public UsesListFixPathOperation(IUsesListOperationSettings aSettings): base(aSettings)
        {
        }

    }
}
