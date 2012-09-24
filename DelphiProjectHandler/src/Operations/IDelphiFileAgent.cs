using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelphiProjectHandler.Operations
{
    public interface IDelphiFileAgent
    {
        void Execute(String aUnitFileName, IDelphiFileOperation aOperation);
        void Execute(String aUnitFileName, params IDelphiFileOperation[] aOperations);
    }
}
