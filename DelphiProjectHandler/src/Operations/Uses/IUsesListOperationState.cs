using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelphiProjectHandler.Operations.Uses
{
    public interface IUsesListOperationState: IOperationState
    {
        UnitList Units { get; }
    }
}
