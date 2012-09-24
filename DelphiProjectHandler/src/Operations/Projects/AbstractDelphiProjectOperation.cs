using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DelphiProjectHandler.Operations.Uses;

namespace DelphiProjectHandler.Operations.Projects
{
    public abstract class AbstractDelphiProjectOperation : IDelphiFileOperation
    {
        private IUsesListOperation fOperation;
        private IProjectOperationSettings fSettings;
        private IProjectOperationState fState;

        protected IUsesListOperation Operation
        {
            get { return fOperation; }
            set { fOperation = value; }
        }

        protected IProjectOperationSettings Settings
        {
            get { return fSettings; }
            set { fSettings = value; }
        }

        protected IProjectOperationState State
        {
            get { return fState; }
            set { fState = value; }
        }

        protected virtual string ReplaceContent(string iContent, string iOldValue, string iNewValue)
        {
            return iContent.Replace(iOldValue, iNewValue);
        }

        protected IProjectOperationSettings CreateProjectSettings(ICollection<string> aRequiredUnits, ICollection<string> aUnitsToManipulate, bool aUsePaths)
        {
            IProjectOperationSettings vSettings = new ProjectOperationSettings();
            vSettings.RequiredUnits = aRequiredUnits;
            vSettings.UnitsToManipulate = aUnitsToManipulate;
            vSettings.UsePaths = aUsePaths;
            return vSettings;
        }

        protected IUnitItemBuilder CreateUnitItemBuilder(string aFileName, bool aUsePaths)
        {
            return new ProjectUnitItemBuilder(aFileName, aUsePaths);
        }

        protected abstract IUsesListOperation CreateUsesOperation(string aFileName, IProjectOperationSettings aSettings, IUnitItemBuilder aBuilder);

        public AbstractDelphiProjectOperation(ICollection<string> aRequiredUnits, ICollection<string> aUnitsToManipulate, bool aUsePaths)
        {
            fState = new ProjectOperationState();
            fSettings = CreateProjectSettings(aRequiredUnits, aUnitsToManipulate, aUsePaths);
        }

        public virtual void Initialize(string aFileName, string aFileContent)
        {
            State.FileContent = aFileContent;
            State.OriginalUsesClause = UsesClauseReader.ExtractUses(aFileContent);
            State.Units = UsesClauseReader.GetUnits(State.FileContent);
            IUnitItemBuilder vBuilder = CreateUnitItemBuilder(aFileName, Settings.UsePaths);
            fOperation = CreateUsesOperation(aFileName, Settings, vBuilder);
            Operation.Initialize(State.Units);
        }

        public bool CanProcess()
        {
            return Operation.CanProcess();
        }

        public string Execute()
        {
            if (!Operation.Execute())
                return State.FileContent;

            return ReplaceContent(State.FileContent, State.OriginalUsesClause, State.Units.ToString());
        }
    }
}
