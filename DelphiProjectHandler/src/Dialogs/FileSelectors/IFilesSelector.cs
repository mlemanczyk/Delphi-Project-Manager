using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelphiProjectHandler.Dialogs.FileSelectors
{
    public interface IFilesSelectorOptions
    {
        string Title { get; set;  }
        string Filter { get; set;  }
        string DefaultExt { get; set;  }
        bool MultiSelect { get; set;  }
    }

    public interface IFilesSelector
    {
        IFilesSelectorOptions DefaultOptions { get; }

        ICollection<string> SelectFiles(IFilesSelectorOptions aOptions);
        ICollection<string> SelectFiles();
    }
}
