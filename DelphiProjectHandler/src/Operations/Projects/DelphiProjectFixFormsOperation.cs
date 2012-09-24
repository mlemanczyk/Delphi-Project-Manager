using System;
using DelphiProjectHandler.Operations.Uses;
using System.Collections.Generic;

namespace DelphiProjectHandler.Operations.Projects
{
    public class DelphiProjectFixFormsOperation: AbstractDelphiProjectOperation
    {
        protected override IUsesListOperation CreateUsesOperation(string aFileName, IProjectOperationSettings aSettings, IUnitItemBuilder aBuilder)
        {
            IUsesListOperationSettings vUsesSettings = new UsesListOperationSettings();
            vUsesSettings.RequiredUnits = aSettings.RequiredUnits;
            vUsesSettings.UnitItemBuilder = aBuilder;
            vUsesSettings.UnitsToManipulate = aSettings.UnitsToManipulate;
            return new UsesListFixFormOperation(vUsesSettings);
        }

        public DelphiProjectFixFormsOperation(ICollection<string> aRequiredUnits, ICollection<string> aUnitsToManipulate, bool aUsePaths): base(aRequiredUnits, aUnitsToManipulate, aUsePaths)
        {
        }

    }
}
