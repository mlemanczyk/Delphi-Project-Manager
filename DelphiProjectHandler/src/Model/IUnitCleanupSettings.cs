using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DelphiProjectHandler.View;

namespace DelphiProjectHandler.Model
{
    public interface IUnitCleanupSettings
    {
        IList<string> UnitsToClean { get; set; }
        IList<string> IgnoredUnits { get; set; }
        String IcarusReport { get; set; }

        void AddObserver(IUnitCleanupSettingsView aView);
        void RemoveObserver(IUnitCleanupSettingsView aView);
        void NotifyObservers();
    }
}
