using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DelphiProjectHandler.Operations.Uses;

namespace DelphiProjectHandler.Operations.Units
{
    public abstract class AbstractDelphiUnitOperation: IDelphiFileOperation
    {
        private IUsesListOperation fIntfOperation;
        private IUsesListOperation fImplOperation;

        private IDelphiUnitOperationState fState;
        protected IDelphiUnitOperationState State
        {
            get { return fState; }
            set { fState = value; }
        }

        private IDelphiUnitOperationSettings fSettings;
        protected IDelphiUnitOperationSettings Settings
        {
            get { return fSettings; }
            set { fSettings = value; }
        }

        protected IUnitItemBuilder CreateUnitItemBuilder()
        {
            return new DelphiUnitItemBuilder();
        }

        protected IDelphiUnitOperationSettings CreateSettings(ICollection<string> aRequiredUnits, ICollection<string> aInterfaceUnits, ICollection<string> aImplementationUnits)
        {
            IDelphiUnitOperationSettings vSettings = new DelphiUnitOperationSettings();
            vSettings.RequiredUnits = aRequiredUnits;
            vSettings.IntfUnitsToManipulate = aInterfaceUnits;
            vSettings.ImplUnitsToManipulate = aImplementationUnits;
            return vSettings;
        }

        protected abstract IUsesListOperation CreateUsesOperation(ICollection<string> aUnitsToManipulate, ICollection<string> aRequiredUnits, IUnitItemBuilder aBuilder);

        protected virtual string ReplaceContent(string iContent, string iOldValue, string iNewValue)
        {
            return iContent.Replace(iOldValue, iNewValue);
        }

        public AbstractDelphiUnitOperation(ICollection<string> aInterfaceUnits, ICollection<string> aImplementationUnits, ICollection<string> aRequiredUnits)
        {
            fState = new DelphiUnitOperationState();
            fSettings = CreateSettings(aRequiredUnits, aInterfaceUnits, aImplementationUnits);
        }

        public void Initialize(string aFileName, string aFileContent)
        {
            State.FileContent = aFileContent;
            string vIntfUsesClause = "", vImplUsesClause = "";
            UsesClauseReader.ExtractUses(aFileContent, out vIntfUsesClause, out vImplUsesClause);
            State.OriginalIntfUsesClause = vIntfUsesClause;
            State.OriginalImplUsesClause = vImplUsesClause;
            State.IntfUnits = UsesClauseReader.GetUnits(vIntfUsesClause);
            State.ImplUnits = UsesClauseReader.GetUnits(vImplUsesClause);
            IUnitItemBuilder vBuilder = CreateUnitItemBuilder();
            fIntfOperation = CreateUsesOperation(Settings.IntfUnitsToManipulate, Settings.RequiredUnits, vBuilder);
            fImplOperation = CreateUsesOperation(Settings.ImplUnitsToManipulate, Settings.RequiredUnits, vBuilder);
            fIntfOperation.Initialize(State.IntfUnits);
            fImplOperation.Initialize(State.ImplUnits);
        }

        public bool CanProcess()
        {
            return (fIntfOperation.CanProcess() || fImplOperation.CanProcess());
        }

        public string Execute()
        {
            string vContent = State.FileContent;
            if (fIntfOperation.Execute())
                vContent = ReplaceContent(vContent, State.OriginalIntfUsesClause, State.IntfUnits.ToString());
            if (fImplOperation.Execute())
                vContent = ReplaceContent(vContent, State.OriginalImplUsesClause, State.ImplUnits.ToString());

            return vContent;
        }
    }
}
