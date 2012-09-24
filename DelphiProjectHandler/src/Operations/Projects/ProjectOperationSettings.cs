using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelphiProjectHandler.Operations.Projects
{
    public class ProjectOperationSettings: IProjectOperationSettings
    {
        private ICollection<string> fRequiredUnits;
        public ICollection<string> RequiredUnits
        {
            get { return fRequiredUnits; }
            set { fRequiredUnits = value; }
        }

        private ICollection<string> fUnitsToManipulate;
        public ICollection<string> UnitsToManipulate
        {
            get { return fUnitsToManipulate; }
            set { fUnitsToManipulate = value; }
        }

        private bool fUsePaths;
        public bool UsePaths
        {
            get { return fUsePaths; }
            set { fUsePaths = value; }
        }
    }
}
