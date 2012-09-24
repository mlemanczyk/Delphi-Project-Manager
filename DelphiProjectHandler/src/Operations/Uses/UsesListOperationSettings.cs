using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelphiProjectHandler.Operations.Uses
{
    public class UsesListOperationSettings: IUsesListOperationSettings
    {
        private ICollection<string> fRequiredUnits;
        public ICollection<string> RequiredUnits
        {
            get { return fRequiredUnits; }
            set { fRequiredUnits = value; }
        }

        private ICollection<string> fUnitsToManipulate;
        public ICollection<string> UnitsToManipulate
        {
            get { return fUnitsToManipulate; }
            set { fUnitsToManipulate = value; }
        }

        private IUnitItemBuilder fUnitItemBuilder;
        public IUnitItemBuilder UnitItemBuilder
        {
            get { return fUnitItemBuilder; }
            set { fUnitItemBuilder = value; }
        }
    }
}
