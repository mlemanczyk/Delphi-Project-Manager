using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelphiProjectHandler.Controller
{
    public interface IProjectBulkOperationController
    {
        bool ProcessFile();
        bool SelectFile();
    }
}
