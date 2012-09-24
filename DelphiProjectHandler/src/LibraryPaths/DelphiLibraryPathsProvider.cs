using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace DelphiProjectHandler.LibraryPaths
{
    public class DelphiLibraryPathsProvider: ILibraryPathsProvider
    {
        #region (Public Methods)

        public DelphiLibraryPathsProvider(LibraryPathType iType)
        {
            fLibraryType = iType;
        }

        #endregion
        #region ILibraryPathsProvider Members

        IList<string> ILibraryPathsProvider.List()
        {
            List<string> vPaths = ListPaths(LibraryType);
            IDictionary<string, string> vEnvironmentPaths = ListEnvironmentPaths(LibraryType);
            return BuildPaths(vPaths, vEnvironmentPaths);
        }

        #endregion
        #region (Protected Methods)

        protected string ReadString(RegistryKey iKey, string iValueName)
        {
            return iKey.GetValue(iValueName).ToString();
        }

        protected List<string> ListPaths(LibraryPathType iType)
        {
            List<string> vResult = new List<string>();
            vResult.AddRange(ReadString(DelphiRegistryKeys.LibraryKey(iType), "Search Path").ToString().Split(';'));
            return vResult;
        }

        protected IDictionary<string, string> ListEnvironmentPaths(LibraryPathType iType)
        {
            Dictionary<string, string> vResult = new Dictionary<string, string>();
            try
            {
                vResult.Add("delphi", ReadString(DelphiRegistryKeys.RootDirKey(iType), "RootDir"));
                RegistryKey vKey = DelphiRegistryKeys.EnvironmentPathsKey(iType);
                foreach (string vValueName in vKey.GetValueNames())
                    vResult.Add(vValueName, vKey.GetValue(vValueName).ToString());
            }
            catch (NotSupportedException)
            {
                // In this case return empty dictionary - no environment variables to return
            }
            return vResult;
        }

        protected IList<string> BuildPaths(List<string> vPaths, IDictionary<string, string> vEnvironmentPaths)
        {
            ILibraryPathsBuilder vBuilder = new DelphiLibraryPathsBuilder();
            return vBuilder.Build(vPaths, vEnvironmentPaths);
        }

        #endregion
        #region (Protected Properties)

        protected LibraryPathType LibraryType
        {
            get { return fLibraryType; }
        }
        public LibraryPathType fLibraryType;

        #endregion
    }
}
