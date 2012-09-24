using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace DelphiProjectHandler.LibraryPaths
{
    public sealed class DelphiRegistryKeys
    {
        public static RegistryKey LibraryKey(LibraryPathType iType)
        {
            string vKey = "";
            switch (iType)
            {
                case LibraryPathType.Delphi5:
                    vKey = @"Software\Borland\Delphi\5.0\Library";
                    break;
                case LibraryPathType.Delphi6:
                    vKey = @"Software\Borland\Delphi\6.0\Library";
                    break;
                default:
                    throw new NotSupportedException("The library type " + iType.ToString() + " is not supported");
            }

            return Registry.CurrentUser.OpenSubKey(vKey);
        }

        public static RegistryKey EnvironmentPathsKey(LibraryPathType iType)
        {
            string vKey = "";
            switch (iType)
            {
                case LibraryPathType.Delphi6:
                    vKey = @"Software\Borland\Delphi\6.0\Environment Variables";
                    break;
                default:
                    throw new NotSupportedException("The library type " + iType.ToString() + " is not supported");
            }
            return Registry.CurrentUser.OpenSubKey(vKey);
        }

        public static RegistryKey RootDirKey(LibraryPathType iType)
        {
            switch (iType)
            {
                case LibraryPathType.Delphi5:
                    return Registry.LocalMachine.OpenSubKey(@"Software\Borland\Delphi\5.0");
                case LibraryPathType.Delphi6:
                    return Registry.CurrentUser.OpenSubKey(@"Software\Borland\Delphi\6.0");
                default:
                    throw new NotSupportedException("The library type " + iType.ToString() + " is not supported");
            }
        }
    }
}
