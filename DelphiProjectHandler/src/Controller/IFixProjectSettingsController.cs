using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DelphiProjectHandler.Model;

namespace DelphiProjectHandler.Controller
{
    public interface IFixProjectSettingsController
    {
        void ImportProjects();
        void SelectProjects();
        void SelectUnits();
        void ImportUnits();
        void SwitchAddUnitPathsToDPRs();
        void SetProjects(ICollection<string> aProjects);
        void SetUnitsToManipulate(ICollection<string> aUnits);
        void SetUnitConstraints(ICollection<string> aConstraints);

        IFixProjectSettings Model { get; set; }
    }
}
