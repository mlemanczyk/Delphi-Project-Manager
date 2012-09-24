using System;
using System.Collections.Generic;
using System.Text;

namespace DelphiProjectHandler.LibraryPaths
{
    public class DelphiLibraryPathsBuilder: ILibraryPathsBuilder
    {
        #region (Protected Methods)

        protected IList<string> CopyFromList(IList<string> iPaths)
        {
            IList<string> vResult = new List<string>();
            foreach (string vPath in iPaths)
                vResult.Add(vPath.ToLower());
            return vResult;
        }

        protected bool SubstituteEnvironmentPaths(IList<string> iPaths, int iPathIdx, IDictionary<string, string> iEnvironmentPaths)
        {
            bool vReplaced = false;
            string vNewPath = iPaths[iPathIdx];
            vNewPath = SubstituteEnvironmentPaths(vNewPath, iEnvironmentPaths);
            vReplaced = !vNewPath.Equals(iPaths[iPathIdx]);
            if (vReplaced)
                iPaths[iPathIdx] = vNewPath;
            return vReplaced;
        }

        protected string SubstituteEnvironmentPaths(string iPath, IDictionary<string, string> iEnvironmentPaths)
        {
            foreach (string vKey in iEnvironmentPaths.Keys)
                iPath = iPath.Replace("$(" + vKey.ToLower() + ")", iEnvironmentPaths[vKey].ToLower());
            return iPath;
        }

        #endregion
        #region ILibraryPathsBuilder Members

        public IList<string> Build(IList<string> iPaths, IDictionary<string, string> iEnvironmentPaths)
        {
            bool vReplaced = false;
            IList<string> vResult = CopyFromList(iPaths);
            do
            {
                vReplaced = false;
                for (int vPathIdx = 0; vPathIdx < vResult.Count; vPathIdx++)
                    vReplaced = SubstituteEnvironmentPaths(vResult, vPathIdx, iEnvironmentPaths) || vReplaced;
            } while (vReplaced);

            return vResult;
        }

        #endregion
    }
}
