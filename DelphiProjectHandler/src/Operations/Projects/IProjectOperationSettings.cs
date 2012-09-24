using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelphiProjectHandler.Operations.Projects
{
    public interface IProjectOperationSettings
    {
        ICollection<string> RequiredUnits { get; set; }
        ICollection<string> UnitsToManipulate { get; set; }
        bool UsePaths { get; set; }
    }
}
