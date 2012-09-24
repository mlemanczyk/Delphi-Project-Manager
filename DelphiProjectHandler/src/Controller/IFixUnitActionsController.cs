using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DelphiProjectHandler.Model;

namespace DelphiProjectHandler.Controller
{
    public interface IFixUnitActionsController
    {
        void AddUnits();
        void RemoveUnits();

        IFixUnitSettings Model { get; set; }
    }
}
