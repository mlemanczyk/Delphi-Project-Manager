using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DelphiProjectHandler.LibraryPaths;
using DelphiProjectHandler.View;

namespace DelphiProjectHandler.Model
{
    public interface ILibraryPathSettings
    {
        IList<LibraryPathType> SupportedProviders { get; }
        IList<string> LibraryPaths { get; set; }

        void AddObserver(ILibraryPathSettingsView aView);
        void RemoveObserver(ILibraryPathSettingsView aView);
        void NotifyObservers();
    }
}
