using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelphiProjectHandler.Operations.Units
{
    public interface IDelphiUnitOperationSettings
    {
        ICollection<string> RequiredUnits { get; set; }
        ICollection<string> IntfUnitsToManipulate { get; set; }
        ICollection<string> ImplUnitsToManipulate { get; set; }
    }
}
