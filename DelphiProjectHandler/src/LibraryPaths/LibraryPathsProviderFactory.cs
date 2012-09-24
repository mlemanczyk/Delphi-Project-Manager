using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelphiProjectHandler.LibraryPaths
{
    public sealed class LibraryPathsProviderFactory
    {
        public static bool IsProviderSupported(LibraryPathType iLibraryType)
        {
            try
            {
                ILibraryPathsProvider vProvider = LibraryPathsProviderFactory.CreateProvider(iLibraryType);
                return (vProvider != null);
            }
            catch (NotSupportedException)
            {
                return false;
            }
        }

        public static LibraryPathType[] ListSupportedProviders()
        {
            Array vLibraryTypes = Enum.GetValues(typeof(LibraryPathType));
            List<object> vSupported = new List<object>();
            for (int vLibraryTypeIdx = 0; vLibraryTypeIdx < vLibraryTypes.Length; vLibraryTypeIdx++)
            {
                LibraryPathType vType = (LibraryPathType)vLibraryTypes.GetValue(vLibraryTypeIdx);
                if (!IsProviderSupported(vType))
                    continue;

                vSupported.Add(vType);
            }

            LibraryPathType[] vResult = new LibraryPathType[vSupported.Count];
            for (int vSupportedIdx = 0; vSupportedIdx < vSupported.Count; vSupportedIdx++)
                vResult[vSupportedIdx] = (LibraryPathType)vSupported[vSupportedIdx];

            return vResult;
        }

        public static ILibraryPathsProvider CreateProvider(LibraryPathType iType)
        {
            switch (iType)
            {
                case LibraryPathType.Delphi5:
                case LibraryPathType.Delphi6:
                    return new DelphiLibraryPathsProvider(iType);
                
                case LibraryPathType.Clipboard:
                    return new ClipboardLibraryPathsProvider();

                default:
                    throw new NotSupportedException("Provider " + iType.ToString() + " is not supported, yet.");
            }
        }
    }
}
