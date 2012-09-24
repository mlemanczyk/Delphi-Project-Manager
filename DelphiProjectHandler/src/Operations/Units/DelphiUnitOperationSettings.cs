using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelphiProjectHandler.Operations.Units
{
    public class DelphiUnitOperationSettings: IDelphiUnitOperationSettings
    {
        private ICollection<string> fRequiredUnits;
        public ICollection<string> RequiredUnits
        {
            get { return fRequiredUnits; }
            set { fRequiredUnits = value; }
        }

        private ICollection<string> fIntfUnitsToManipulate;
        public ICollection<string> IntfUnitsToManipulate
        {
            get { return fIntfUnitsToManipulate; }
            set { fIntfUnitsToManipulate = value; }
        }

        private ICollection<string> fImplUnitsToManipulate;
        public ICollection<string> ImplUnitsToManipulate
        {
            get { return fImplUnitsToManipulate; }
            set { fImplUnitsToManipulate = value; }
        }
    }
}
