using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DelphiProjectHandler.View;

namespace DelphiProjectHandler.Model
{
    public interface IFixProjectSettings
    {
        IList<string> Projects { get; set;  }
        IList<string> UnitsToManipulate { get; set; }
        IList<string> UnitConstraints { get; set; }
        bool AddUnitPathsToDPRs { get; set; }

        void AddObserver(IFixProjectSettingsView aView);
        void RemoveObserver(IFixProjectSettingsView aView);
        void NotifyObservers();
    }
}
