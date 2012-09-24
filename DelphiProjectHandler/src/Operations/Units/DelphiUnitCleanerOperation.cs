/*
 * Created by SharpDevelop.
 * User: SG0894652
 * Date: 8/12/2009
 * Time: 11:06 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using DelphiProjectHandler.Operations;

namespace DelphiProjectHandler.Operations.Units
{
	/// <summary>
	/// Description of DelphiUnitCleanerOperation.
	/// </summary>
	public class DelphiUnitCleanerOperation: DelphiOperation
	{
		public DelphiUnitCleanerOperation(): base()
		{
		}

        protected SuggestedUnitStructure RemoveIgnoredUnits(SuggestedUnitStructure iSuggestedUnitStructure, IList<string> iIgnoredUnits)
        {
        	List<string> vList = new List<string>(iIgnoredUnits);
            for (int vUnitIdx = 0; vUnitIdx < vList.Count; vUnitIdx++)
                vList[vUnitIdx] = vList[vUnitIdx].ToLower();

            SuggestedUnitStructure vResult = iSuggestedUnitStructure.Clone();
            RemoveUnits(vResult.MoveToInterface, vList);
            RemoveUnits(vResult.ToDelete, vList);
            return vResult;
        }

        protected void RemoveUnits(UnitList iList, IList<string> iToRemove)
		{
			for (int vUnitIdx = iList.Count - 1; vUnitIdx >= 0; vUnitIdx--)
				if (iToRemove.IndexOf(iList[vUnitIdx].Name.ToLower()) >= 0)
					iList.RemoveAt(vUnitIdx);
		}
		
		protected void AddUnits(UnitList iList, UnitList iToAdd)
		{
			for (int vUnitIdx = iToAdd.Count - 1; vUnitIdx >= 0; vUnitIdx--)
			{
				UnitItem vUnit = iToAdd[vUnitIdx];
				if (iList.IndexOf(vUnit.Name) < 0)
					iList.Insert(0, vUnit);
			}
		}
		
		protected UnitList GetUsesList(string iUsesContent)
		{
			if (!iUsesContent.Trim().Equals(""))
				return UsesClauseReader.GetUnits(iUsesContent);
			else
				return new UnitList();
		}
		
		protected string CleanUpInterface(string iUnitContent, string iUsesContent, SuggestedUnitStructure iSuggestedStructure)
		{
			if (iUsesContent.Trim().Equals(""))
				return iUnitContent;
			
			UnitList vUnits = GetUsesList(iUsesContent);
			RemoveUnits(vUnits, iSuggestedStructure.ToDelete);
			RemoveUnits(vUnits, iSuggestedStructure.MoveToInterface);
			return ReplaceContent(iUnitContent, iUsesContent, vUnits.ToString());
		}

        protected string CleanUpImplementation(string iUnitContent, string iUsesContent, SuggestedUnitStructure iSuggestedStructure)
		{
			UnitList vUnits = GetUsesList(iUsesContent);
			RemoveUnits(vUnits, iSuggestedStructure.ToDelete);
			AddUnits(vUnits, iSuggestedStructure.MoveToInterface);
			if (vUnits.Count == 0 && iUsesContent.Trim().Equals(""))
				return iUnitContent;
			
			string vNewValue = vUnits.ToString();
			if (iUsesContent.Trim().Equals(""))
			{
				iUsesContent = "implementation";
				vNewValue = "implementation\r\n\r\n" + vNewValue;
			}
			return ReplaceContent(iUnitContent, iUsesContent, vNewValue);
		}
		
		public string Execute(string iUnitContent, SuggestedUnitStructure iSuggestedStructure, IList<string> iIgnoredUnits)
		{
			string vInterface, vImplementation;
            SuggestedUnitStructure vSuggestedStructure = RemoveIgnoredUnits(iSuggestedStructure.Clone(), iIgnoredUnits);
			UsesClauseReader.ExtractUses(iUnitContent, out vInterface, out vImplementation);
			string vResult = iUnitContent;
			vResult = CleanUpInterface(vResult, vInterface, vSuggestedStructure);
			vResult = CleanUpImplementation(vResult, vImplementation, vSuggestedStructure);
			
			return vResult;
		}
	}
}
