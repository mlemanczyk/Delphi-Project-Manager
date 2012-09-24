using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelphiProjectHandler.Operations.Uses
{
    public interface IUsesListOperation
    {
        void Initialize(UnitList aUnitList);
        bool CanProcess();
        bool Execute();
    }
}
