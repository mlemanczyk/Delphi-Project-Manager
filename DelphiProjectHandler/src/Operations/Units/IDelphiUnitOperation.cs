using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelphiProjectHandler.Operations.Units
{
    public interface IDelphiUnitOperation
    {
        string Execute(string aUnitContent, ICollection<string> aInterfaceUnits, ICollection<string> aImplementationUnits, ICollection<string> aRequiredUnits);
    }
}
