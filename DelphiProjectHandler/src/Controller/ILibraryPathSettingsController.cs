using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DelphiProjectHandler.LibraryPaths;
using DelphiProjectHandler.Model;

namespace DelphiProjectHandler.Controller
{
    public interface ILibraryPathSettingsController
    {
        void LoadPreset(LibraryPathType aType);
        void NormalizePaths();
        void SetLibraryPaths(ICollection<string> aLibraryPaths);

        ILibraryPathSettings Model { get; set; }
    }
}
