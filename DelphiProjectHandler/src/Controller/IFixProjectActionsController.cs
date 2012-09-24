using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DelphiProjectHandler.Model;

namespace DelphiProjectHandler.Controller
{
    public interface IFixProjectActionsController
    {
        void AddUnits();
        void RemoveUnits();
        void FixPaths();
        void FixForms();

        IFixProjectSettings Model { get; set; }
    }
}
