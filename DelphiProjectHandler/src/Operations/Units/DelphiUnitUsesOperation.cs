using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DelphiProjectHandler.Operations;

namespace DelphiProjectHandler.Operations.Units
{
    public abstract class DelphiUnitUsesOperation: DelphiOperation, IDelphiUnitOperation
    {
        protected virtual bool CanProcessUnit(UnitList aCurrentIntfUnits, UnitList aCurrentImplUnits, ICollection<string> aRequiredUnits)
        {
            return aCurrentIntfUnits.Concat<string>(aCurrentImplUnits).Intersect<string>(aRequiredUnits).Count() == aRequiredUnits.Count;
        }

        public string Execute(string aUnitContent, ICollection<string> aInterfaceUnits, ICollection<string> aImplementationUnits, ICollection<string> aRequiredUnits)
        {
            string vCurrentIntfUses = "";
            string vCurrentImplUses = "";
            UsesClauseReader.ExtractUses(aUnitContent, out vCurrentIntfUses, out vCurrentImplUses);
            UnitList vCurrentIntfUnits = UsesClauseReader.GetUnits(vCurrentIntfUses);
            UnitList vCurrentImplUnits = UsesClauseReader.GetUnits(vCurrentImplUses);

            if (!CanProcessUnit(vCurrentIntfUnits, vCurrentImplUnits, aRequiredUnits))
                return aUnitContent;

            string vNewUnitContent = ProcessUsesSection(aUnitContent, vCurrentIntfUses, vCurrentIntfUnits, aInterfaceUnits, "interface");
            return ProcessUsesSection(vNewUnitContent, vCurrentImplUses, vCurrentImplUnits, aImplementationUnits, "implementation");
        }

        protected string ProcessUsesSection(string aUnitContent, string aCurrentUsesClause, UnitList aCurrentUnitList, ICollection<string> aUnits, string aMissingUsesReplacement)
        {
            bool vListModified = ProcessUsesList(aCurrentUnitList, aUnits);
            if (!vListModified)
                return aUnitContent;

            return BuildNewUsesClause(aUnitContent, aCurrentUsesClause, aCurrentUnitList, aMissingUsesReplacement);
        }

        protected string BuildNewUsesClause(string aUnitContent, string aCurrentUsesClause, UnitList aCurrentUnitList, string aMissingUsesReplacement)
        {
            string vLineStarting = "";
            string vLineEnding = "";
            if (aCurrentUsesClause == "")
            {
                aCurrentUsesClause = aMissingUsesReplacement + Environment.NewLine;
                vLineStarting = aCurrentUsesClause + Environment.NewLine;
            }

            return ReplaceContent(aUnitContent, aCurrentUsesClause, vLineStarting + aCurrentUnitList.ToString() + vLineEnding);
        }

        protected abstract bool ProcessUsesList(UnitList aCurrentUnitList, ICollection<string> aUnits);
    }
}
