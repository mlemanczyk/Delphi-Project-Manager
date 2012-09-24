using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DelphiProjectHandler.Model;

namespace DelphiProjectHandler.Controller
{
    public interface IUnitCleanupSettingsController
    {
        void LoadFromLibraryPaths(ICollection<string> aLibraryPaths);
        void PasteIcarusReport();
        void SetIcarusReport(string aReport);
        void SetIgnoredUnits(ICollection<string> aUnits);
        void SetUnitsToClean(ICollection<string> aUnits);

        IUnitCleanupSettings Model { get; set; }
    }
}
