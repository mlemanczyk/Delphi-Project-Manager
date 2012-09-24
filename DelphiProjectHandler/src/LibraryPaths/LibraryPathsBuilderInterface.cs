using System;
using System.Collections.Generic;
using System.Text;

namespace DelphiProjectHandler.LibraryPaths
{
    public interface ILibraryPathsBuilder
    {
        IList<string> Build(IList<string> iPaths, IDictionary<string, string> iEnvironmentPaths);
    }
}
