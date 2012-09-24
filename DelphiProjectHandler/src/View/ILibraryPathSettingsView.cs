using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DelphiProjectHandler.Model;

namespace DelphiProjectHandler.View
{
    public interface ILibraryPathSettingsView
    {
        void UpdateView(ILibraryPathSettings aModel);
    }
}
