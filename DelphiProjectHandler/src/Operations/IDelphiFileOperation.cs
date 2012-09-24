using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelphiProjectHandler.Operations
{
    public interface IDelphiFileOperation
    {
        void Initialize(string aFileName, string aFileContent);
        bool CanProcess();
        string Execute();
    }
}
