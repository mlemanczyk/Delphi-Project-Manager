using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelphiProjectHandler.Operations.Uses
{
    public class UsesListOperationState: IUsesListOperationState
    {
        private UnitList fUnits;
        public UnitList Units
        {
            get { return fUnits; }
            set { fUnits = value; }
        }
    }
}
