using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelphiProjectHandler.Operations.Units
{
    public interface IDelphiUnitOperationState: IOperationState
    {
        string FileContent { get; set; }
        string OriginalIntfUsesClause { get; set; }
        string OriginalImplUsesClause { get; set; }
        UnitList IntfUnits { get; set; }
        UnitList ImplUnits { get; set; }
    }
}
