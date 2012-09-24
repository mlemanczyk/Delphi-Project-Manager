using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DelphiProjectHandler.Operations.Uses;

namespace DelphiProjectHandler.Operations.Units
{
    public class DelphiUnitAddOperation: AbstractDelphiUnitOperation
    {
        protected override IUsesListOperation CreateUsesOperation(ICollection<string> aUnitsToManipulate, ICollection<string> aRequiredUnits, IUnitItemBuilder aBuilder)
        {
            IUsesListOperationSettings vUsesSettings = new UsesListOperationSettings();
            vUsesSettings.RequiredUnits = aRequiredUnits;
            vUsesSettings.UnitItemBuilder = aBuilder;
            vUsesSettings.UnitsToManipulate = aUnitsToManipulate;
            return new UsesListAddOperation(vUsesSettings);
        }

        public DelphiUnitAddOperation(ICollection<string> aInterfaceUnits, ICollection<string> aImplementationUnits, ICollection<string> aRequiredUnits)
            : base(aInterfaceUnits, aImplementationUnits, aRequiredUnits)
        {
        }
    }
}
