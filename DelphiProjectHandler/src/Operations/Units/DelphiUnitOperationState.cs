using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelphiProjectHandler.Operations.Units
{
    public class DelphiUnitOperationState: IDelphiUnitOperationState
    {
        private string fFileContent;
        public string FileContent
        {
            get { return fFileContent; }
            set { fFileContent = value; }
        }

        private string fOriginalIntfUsesClause;
        public string OriginalIntfUsesClause
        {
            get { return fOriginalIntfUsesClause; }
            set { fOriginalIntfUsesClause = value; }
        }

        private string fOriginalImplUsesClause;
        public string OriginalImplUsesClause
        {
            get { return fOriginalImplUsesClause; }
            set { fOriginalImplUsesClause = value; }
        }

        private UnitList fIntfUnits;
        public UnitList IntfUnits
        {
            get { return fIntfUnits; }
            set { fIntfUnits = value; }
        }

        private UnitList fImplUnits;
        public UnitList ImplUnits
        {
            get { return fImplUnits; }
            set { fImplUnits = value; }
        }
    }
}
