using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelphiProjectHandler.Operations.Projects
{
    public interface IProjectOperationState : IOperationState
    {
        string FileContent { get; set; }
        string OriginalUsesClause { get; set; }
        UnitList Units { get; set; }
    }
}
