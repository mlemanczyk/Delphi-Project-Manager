using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelphiProjectHandler.Operations.Projects
{
    public class ProjectOperationState : IProjectOperationState
    {
        private string fFileContent;
        public string FileContent
        {
            get { return fFileContent; }
            set { fFileContent = value; }
        }

        private string fOriginalUsesState;
        public string OriginalUsesClause
        {
            get { return fOriginalUsesState; }
            set { fOriginalUsesState = value; }
        }

        private UnitList fUnits;
        public UnitList Units
        {
            get { return fUnits; }
            set { fUnits = value; }
        }
    }
}
