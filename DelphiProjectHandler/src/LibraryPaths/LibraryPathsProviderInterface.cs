using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelphiProjectHandler.LibraryPaths
{
    public enum LibraryPathType : int
    {
        Delphi5 = 0,
        Delphi6 = 1,
        Delphi7 = 2,
        Delphi2005 = 3,
        Delphi2007 = 4,
        Delphi2009 = 5,
        Clipboard = 6
    }

    public interface ILibraryPathsProvider
    {
        IList<string> List();
    }
}
