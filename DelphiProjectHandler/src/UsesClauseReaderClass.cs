using System;
using System.Text.RegularExpressions;

namespace DelphiProjectHandler
{
	public class UsesClauseReader
	{
		protected static string cLineComments = @"//[^\r\n]*[\r\n]+";
		protected static string cDelphiComments = @"{(?!\$)(?:[/\w\s.]*?[,;]|[\s.]*\w+[\s.]+\w+\s*[/\w\s.]*?)}\r?\n?";
		protected static string cIfDefCondExpr = @"\{[\s\n]*\$IFDEF[\s\n]+[^}]+\}[\s\n]*";
		protected static string cElseCondExpr = @"\{[\s\n]*\$ELSE[\s\n]+[^}]+\}[\s\n]*";
		protected static string cEndifCondExpr = @"[\s\n]*\{[\s\n]*\$ENDIF[\s\n]*[^}]*\}";
		protected static string cUnitExpr = @"\w[\d\w_.]*";
		protected static string cUnitPathExpr = @"[\s\n]+in[\s\n]+'([^']+)'";
		protected static string cUnitCommentExpr = @"\{([^$]*)\}";
		protected static string cUnitWithPathExpr = "(" + cUnitExpr + ")(" + cUnitPathExpr + "){0,1}";
		protected static string cUnitsPathsAndCommentsExpr = cUnitWithPathExpr + @"[\s\n]*(" + cUnitCommentExpr + "){0,1}[,;]{0,1}";
		protected static string cUnitsAndConditions = "((" + cIfDefCondExpr + @")*)[\s\n]*(" + cUnitsPathsAndCommentsExpr + ")((" + cEndifCondExpr + @")*)[,;]{0,1}";

		protected static string cUnitLine = @"(([^,;]+)[,;])";
		protected static string cUnitLines = "((" + cIfDefCondExpr + ")*(" + cUnitLine + ")(" + cEndifCondExpr + ")*)";

		protected static string cAllConditionals = @"\{(\$\w+)([\s\n]+[^}]*)*\}";

		protected static string cAllUnits = @"\buses[\s\n]+([^;]*;)";
// 1	unit name						
		protected static string c1 = @"[\s\n\r]*/*[\s\n\r]*[\w_]+[\s\n\r]*[,;]";
// 2	unit name with form name		
		protected static string c2 = @"[\s\n\r]*/*[\s\n\r]*[\w_]+[\s\n\r]*{[^}]+}[\s\n\r]*[,;]";
// 3	unit name with in & form name
		protected static string c3 = @"[\s\n\r]*/*[\s\n\r]*[\w_]+[\s\n\r]+in[\s\n\r]+'[^']+'[\s\n\r]*{[^}]+}[\s\n\r]*[,;]";
// 4	unit name with in
		protected static string c4 = @"[\s\n\r]*/*[\s\n\r]*[\w_]+[\s\n\r]+in[\s\n\r]+'[^']+'[\s\n\r]*[,;]";
// 5	conditional define start
		protected static string c5 = @"[\s\n\r]*/*[\s\n\r]*{[\s\n\r]*\$IFDEF[\s\n\r]+[^}]+}";
// 6	conditional define end
		protected static string c6 = @"[\s\n\r]*/*[\s\n\r]*{[\s\n\r]*\$ENDIF[\s\n\r]*}";
// 7	C++ like comment
		protected static string c7 = @"[\s\n\r]*/*[\s\n\r]*" + cLineComments;
// 8	Delphi like comment
		protected static string c8 = @"[\s\n\r]*/*[\s\n\r]*" + cDelphiComments;
// 9	unit name with following C++ comment
		protected static string c9 = @"[\s\n\r]*/*[\s\n\r]*[\w_]+\s*(?:" + cLineComments + @")[\s\n\r]*[,;]";
// 10	unit name with following Delphi comment
		protected static string c10 = @"[\s\n\r]*/*[\s\n\r]*[\w_]+\s*(?:([\s\n\r]*" + cDelphiComments + @")+)[\s\n\r]*[,;]";
		
		protected static string cUsesClausesExpr = @"(\buses\b(" + c1 + "|" + c2 + "|" + c3 + "|" + c4 + "|" + c5 + "|" + c6 + "|" + c7 + "|" + c8 + "|" + c9 + "|" + c10 + ")+)";

		protected static string cInterfaceSectionExpr = @"interface\b((.|\n)*)";
		protected static string cImplementationSectionExpr = @"\nimplementation((.|\n)*)";
		protected static string cBothSectionsExpr = cInterfaceSectionExpr + cImplementationSectionExpr;
		
		// @"uses[\s\n\r]+[\w\s,\{\}\\'.$:]+?(?=;);"

		protected static string GetMatchResult(int iMatchID, Match iMatch)
		{
			if (iMatch.Groups.Count <= iMatchID)
				return "";
			else
				return iMatch.Groups[iMatchID].Value;
		}
		
		protected static string GetMatchResult(int iMatchIdx, string iText, string iExpression)
		{
			MatchCollection vMatches = ExecuteExpression(iText, iExpression);
			if (vMatches.Count <= iMatchIdx)
				return "";
			else
				return GetMatchResult(1, vMatches[iMatchIdx]);
		}

		protected static MatchCollection ExecuteExpression(string iText, string iExpression)
		{
			return Regex.Matches(iText, iExpression, RegexOptions.IgnoreCase);
		}

		protected static string GetAllUnits(string iText)
		{
			return GetMatchResult(0, iText, cAllUnits);
		}

		protected static UnitItem SplitUnitInfo(string iText)
		{
			MatchCollection vMatches = ExecuteExpression(iText, cUnitsAndConditions);
			foreach (Match vMatch in vMatches)
			{
				string vUnitName = GetMatchResult(4, vMatch).Trim();
				if (vUnitName == "")
					continue;

				string vUnitPath = GetMatchResult(6, vMatch).Trim();
				bool vHasPath = (vUnitPath != "");
				if (vHasPath)
					vUnitPath = System.IO.Path.GetDirectoryName(vUnitPath);

				string vForm = GetMatchResult(8, vMatch).Trim();

				UnitItem vUnit = new UnitItem(vUnitName, vUnitPath, vHasPath, vForm);

				vUnit.StartingConditions = GetMatchResult(1, vMatch);
				vUnit.EndingConditions = GetMatchResult(9, vMatch);
				return vUnit;
			}

			return null;
		}
		
		protected static string RemoveCommentLines(string iText)
		{
			string vResult = Regex.Replace(iText, cLineComments, "");
			return Regex.Replace(vResult, cDelphiComments, "");
		}

		protected static UnitList SplitUnits(string iText)
		{
			UnitList vList = new UnitList();
			MatchCollection vMatches = ExecuteExpression(iText, cUnitLines);
			foreach (Match vMatch in vMatches)
			{
				string vUnitLine = GetMatchResult(1, vMatch);
				if (vUnitLine == "")
					continue;

				UnitItem vUnit = SplitUnitInfo(vUnitLine);
				if (vUnit != null)
					vList.Add(vUnit);
			}

			return vList;
		}

		public static UnitList GetUnits(string iText)
		{
			string vAllUnits = GetAllUnits(RemoveCommentLines(iText));
			return SplitUnits(vAllUnits);
		}

		public static void ExtractUses(string iText, out string ivInterface, out string ivImplementation)
		{
			ivInterface = "";
			ivImplementation = "";

			string vInterfaceSection = "";
			string vImplementationSection = "";

			MatchCollection vMatches = ExecuteExpression(iText, cBothSectionsExpr);
			if (vMatches.Count == 0)
			{
				vMatches = ExecuteExpression(iText, cInterfaceSectionExpr);
				if (vMatches.Count == 0)
				{
					vMatches = ExecuteExpression(iText, cImplementationSectionExpr);
					if (vMatches.Count > 0)
						vImplementationSection = GetMatchResult(1, vMatches[0]);
					else
						vImplementationSection = "";
				}
				else
					if (vMatches.Count > 0)
					vInterfaceSection = GetMatchResult(1, vMatches[0]);
				else
					vInterfaceSection = "";
			}
			else
			{
				vInterfaceSection = GetMatchResult(1, vMatches[0]);
				vImplementationSection = GetMatchResult(3, vMatches[0]);
			}

			ivInterface = ExtractUses(vInterfaceSection);
			ivImplementation = ExtractUses(vImplementationSection);
		}

		public static string ExtractUses(string iContent)
		{
			return GetMatchResult(0, iContent, cUsesClausesExpr);
		}
	}
}
