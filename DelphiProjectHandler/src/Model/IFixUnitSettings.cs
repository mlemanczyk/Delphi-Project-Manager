using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DelphiProjectHandler.View;

namespace DelphiProjectHandler.Model
{
    public interface IFixUnitSettings
    {
        IList<string> UnitsToModify { get; set; }
        IList<string> InterfaceUnits { get; set; }
        IList<string> ImplementationUnits { get; set; }
        IList<string> UnitConstraints { get; set; }

        void AddObserver(IFixUnitSettingsView aView);
        void RemoveObserver(IFixUnitSettingsView aView);
        void NotifyObservers();
    }
}
