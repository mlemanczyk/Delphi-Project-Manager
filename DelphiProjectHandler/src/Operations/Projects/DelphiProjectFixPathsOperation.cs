using System;
using System.Collections.Generic;
using System.Text;
using DelphiProjectHandler.Operations.Uses;

namespace DelphiProjectHandler.Operations.Projects
{
    public class DelphiProjectFixPathsOperation: AbstractDelphiProjectOperation
    {
        protected override IUsesListOperation CreateUsesOperation(string aFileName, IProjectOperationSettings aSettings, IUnitItemBuilder aBuilder)
        {
            IUsesListOperationSettings vUsesSettings = new UsesListOperationSettings();
            vUsesSettings.RequiredUnits = aSettings.RequiredUnits;
            vUsesSettings.UnitItemBuilder = aBuilder;
            vUsesSettings.UnitsToManipulate = aSettings.UnitsToManipulate;
            return new UsesListFixPathOperation(vUsesSettings);
        }

        public DelphiProjectFixPathsOperation(ICollection<string> aRequiredUnits, ICollection<string> aUnitsToManipulate, bool aUsePaths): base(aRequiredUnits, aUnitsToManipulate, aUsePaths)
        {
        }
    }
}
