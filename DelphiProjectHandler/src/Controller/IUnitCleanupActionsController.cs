using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DelphiProjectHandler.Model;

namespace DelphiProjectHandler.Controller
{
    public interface IUnitCleanupActionsController
    {
        void RemoveUnusedUnits();

        IUnitCleanupSettings Model { get; set; }
    }
}
