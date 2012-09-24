using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DelphiProjectHandler.Dialogs;

namespace DelphiProjectHandler.Operations.Uses
{
    public class UsesListAddOperation: AbstractUsesListOperation
    {
        protected override bool ProcessUnit(UnitList aUnits, UnitItem aUnit)
        {
            aUnits.Add(aUnit);
            return true;
        }

        protected override bool CanProcessUnit(UnitItem aUnit)
        {
            return (aUnit.Name != "") && !State.Units.Contains(aUnit.Name);
        }

        public UsesListAddOperation(IUsesListOperationSettings aSettings)
            : base(aSettings)
        {
        }
    }
}
