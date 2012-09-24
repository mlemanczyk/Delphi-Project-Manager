using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DelphiProjectHandler
{
	public class IcarusAnalyzerReportException: Exception
	{
		public IcarusAnalyzerReportException(string iMessage): base(iMessage) {}
	}
	
	/// <summary>
	/// This class is used for parsing Icarus Analyzer results. For the given report it creates a
	/// list containing all units listed in the report and list of used units, splitted into
	/// interface & implementation categories.
	/// </summary>
	public class IcarusAnalyzerReportParser
	{
		protected enum UsesListType: int
		{
			Interface = 0,
			Implementation = 1
		}
		
		protected static string ExtractUnitName(string iMatch)
		{
			Regex vRegExpr = new Regex(@"Module (\w+) uses:", RegexOptions.IgnoreCase | RegexOptions.Singleline);
			Match vMatch = vRegExpr.Match(iMatch);
			if (!vMatch.Success)
				throw new IcarusAnalyzerReportException("Not recognized module name. " + iMatch);
			
			return vMatch.Groups[1].Value;
		}
		
		protected static UsesListType ExtractUsesListType(string iMatch)
		{
			if (Regex.IsMatch(iMatch, "Units used in interface", RegexOptions.IgnoreCase | RegexOptions.Singleline))
				return UsesListType.Interface;
			else
			if (Regex.IsMatch(iMatch, "Units used in implementation", RegexOptions.IgnoreCase | RegexOptions.Singleline))
			    return UsesListType.Implementation;
			else
				throw new IcarusAnalyzerReportException("Not recognized uses section type. " + iMatch);
		}
		
		protected static void ExtractUnitNames(string iMatch, UsesListType iUsesType, SuggestedUnitStructure iUnitUses)
		{
			Regex vRegExpr = new Regex(@"(-->|==>)?\s?(\w+)\s(in\simplementation|in\sinterface|unnecessary|source\snot\sfound)(?=\s\(|\r\n)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
			MatchCollection vMatches = vRegExpr.Matches(iMatch);

            ExtractUnitsGroup(vMatches, "", (iUsesType == UsesListType.Interface ? iUnitUses.Uses.Interface : iUnitUses.Uses.Implementation));
            ExtractUnitsGroup(vMatches, "==>", iUnitUses.ToDelete);
            if (iUsesType == UsesListType.Interface)
                ExtractUnitsGroup(vMatches, "-->", iUnitUses.MoveToInterface, iUnitUses.Uses.Implementation);
		}

        protected static void ExtractUnitsGroup(MatchCollection aUnitLineMatches, string aGroupCondition, params UnitList[] aListsToAddTo)
        {
            var vMatchedUnits = from Match vMatch in aUnitLineMatches
                                where vMatch.Groups[1].Value.Equals(aGroupCondition)
                                select new UnitItem(vMatch.Groups[2].Value);

            foreach (var vUnit in vMatchedUnits)
                foreach (var vListToAddTo in aListsToAddTo)
                    vListToAddTo.Add(vUnit);
        }

		protected static void ParseMatch(string aUnitName, UsesListType aUsesListType, string aModuleReport, SuggestedUnitStructureList aLists)
		{
			if (!aLists.ContainsKey(aUnitName))
				aLists.Add(aUnitName, new SuggestedUnitStructure());

			SuggestedUnitStructure vList = aLists[aUnitName];
			ExtractUnitNames(aModuleReport, aUsesListType, vList);
		}
		
		protected static void DoParse(string iReport, SuggestedUnitStructureList iLists)
		{
			Regex vRegEx = new Regex(@"\bModule\s[\w\s.\:\(\),\-\>=]+?(?=\bModule|\bProgram|\bLibrary)|\bModule\s[\w\s.\:\(\),\-\>=]+", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var vModuleReports = from Match vMatch in vRegEx.Matches(iReport)
                                 let vModuleReport = vMatch.Value
                                 select new { UnitName = ExtractUnitName(vModuleReport).ToLower(), UsesListType = ExtractUsesListType(vModuleReport), Report = vModuleReport };

            foreach (var vModuleReport in vModuleReports)
                ParseMatch(vModuleReport.UnitName, vModuleReport.UsesListType, vModuleReport.Report, iLists);
		}
		
		public static SuggestedUnitStructureList Parse(string iReport)
		{
			SuggestedUnitStructureList vResult = new SuggestedUnitStructureList();
			DoParse(iReport, vResult);
			return vResult;
		}
	}
}
