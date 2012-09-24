using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DelphiProjectHandler.Model;

namespace DelphiProjectHandler.Controller
{
    public interface IFixUnitSettingsController
    {
        void ImportUnitsToModify();
        void SelectUnitsToModify();
        void ImportInterfaceUnits();
        void SelectInterfaceUnits();
        void ImportImplementationUnits();
        void SelectImplementationUnits();
        void SetUnitsToModify(ICollection<string> aUnits);
        void SetInterfaceUnits(ICollection<string> aUnits);
        void SetImplementationUnits(ICollection<string> aUnits);
        void SetUnitConstraints(ICollection<string> aConstraints);

        IFixUnitSettings Model { get; set; }
    }
}
