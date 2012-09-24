using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelphiProjectHandler.Operations.Uses
{
    public interface IUsesListOperationSettings
    {
        ICollection<string> RequiredUnits { get; set; }
        ICollection<string> UnitsToManipulate { get; set; }
        IUnitItemBuilder UnitItemBuilder { get; set; }
    }
}
