using System;
using System.Collections.Generic;
using NUnit.Framework;

using DelphiProjectHandler;

namespace DelphiProjectHandler.Tests
{
    [TestFixture]
    public class UsesClauseReaderClassTests
    {
        protected UnitList GetDefaultUnits(int iCount, bool iWithPaths, bool iRelative, bool iWithConditions)
        {
            UnitList vResult = new UnitList();
            for (int vUnitIdx = 0; vUnitIdx < iCount; vUnitIdx++)
            {
                string vUnitName = "Unit" + vUnitIdx.ToString();
                string vUnitPath = "";
                if (iWithPaths)
                    if (iRelative)
                        vUnitPath = "..\\.." + "DefaultPath" + vUnitIdx.ToString();
                    else
                        vUnitPath = @"c:\windows\temp\DefaultPath" + vUnitIdx.ToString() + "\\" + vUnitName + ".pas";

                UnitItem vUnit = new UnitItem(vUnitName, vUnitPath, iWithPaths, "");
                if (iWithConditions)
                {
                    vUnit.StartingConditions = "{$IFDEF TestCondition}";
                    vUnit.EndingConditions = "{$ENDIF}";
                }

                vResult.Add(vUnit);
            }

            return vResult;
        }

        protected void CompareUnitLists(UnitList iExpected, UnitList iActual, bool iWithPaths)
        {
            Assert.AreEqual(iExpected.Count, iActual.Count, "Wrong number of units was read");
            for (int vUnitIdx = 0; vUnitIdx < iActual.Count; vUnitIdx++)
            {
                Assert.AreEqual(iExpected[vUnitIdx].Name, iActual[vUnitIdx].Name, "Wrong name of the unit was read");
                Assert.AreEqual(iExpected[vUnitIdx].UsePath, iActual[vUnitIdx].UsePath, "Wrong HasPath was read");
                if (iWithPaths)
                    Assert.AreEqual(iExpected[vUnitIdx].Path, iActual[vUnitIdx].Path, "Wrong path of the unit was read");
                Assert.AreEqual(iExpected[vUnitIdx].StartingConditions, iActual[vUnitIdx].StartingConditions, "Wrong StartingCondition was read");
                Assert.AreEqual(iExpected[vUnitIdx].EndingConditions, iActual[vUnitIdx].EndingConditions, "Wrong EndingCondition was read (" + vUnitIdx.ToString() + ")");
            }
        }

        protected void DoTestGetUnits(int iCount, bool iWithPaths, bool iRelative, bool iWithConditions)
        {
            UnitList vExpected = GetDefaultUnits(iCount, iWithPaths, iRelative, iWithConditions);
            string vTestValue =
                "interface\n" +
                "\n" +
                vExpected.ToString();

            UnitList vActual = UsesClauseReader.GetUnits(vTestValue);
            CompareUnitLists(vExpected, vActual, iWithPaths);
        }

        protected void DoTestGetUnits_MultipleUnitsInConditionals(int iCount, int iStartingIdx, int iEndingIdx, bool iWithPaths, bool iRelativePaths)
        {
            UnitList vExpected = GetDefaultUnits(iCount, iWithPaths, iRelativePaths, false);
            vExpected[iStartingIdx].StartingConditions = "{$IFDEF test}";
            vExpected[iEndingIdx].EndingConditions = "{$ENDIF}";

            string vTestValue = vExpected.ToString();
            UnitList vActual = UsesClauseReader.GetUnits(vTestValue);
            CompareUnitLists(vExpected, vActual, iWithPaths);
        }

        protected string ConstructUnit(
            string iInterfaceUses,
            string iImplementationUses,
            bool iAddTypes,
            bool iAddVars)
        {
            string vTypeDeclaration =
                "type\n" +
                "  TType = class(TObject)\n" +
                "  private\n" +
                "    FField: Integer;\n" +
                "  public\n" +
                "    property Field: Integer read FField write FField;\n" +
                "  end;\n" +
                "\n";
            string vVarsDeclaration =
                "var\n" +
                "  vIndex: Integer;\n" +
                "  vValue: AnsiString;\n" +
                "\n" +
                "const\n" +
                "  cInteger: Integer = 10;\n" +
                "  cString: AnsiString = \"test\";\n" +
                "\n";

            string vTestValue =
                "interface\n" +
                "\n" +
                iInterfaceUses + "\n" +
                "\n";

            if (iAddTypes)
                vTestValue += vTypeDeclaration;
            if (iAddVars)
                vTestValue += vVarsDeclaration;

            vTestValue +=
                "implementation\n" +
                "\n" +
                iImplementationUses;

            if (iAddTypes)
                vTestValue += vTypeDeclaration;
            if (iAddVars)
                vTestValue += vVarsDeclaration;

            return vTestValue;
        }

        protected void DoTestExtractUses(
            bool iHasInterface,
            bool iHasImplementation,
            bool iWithPaths,
            bool iRelative,
            bool iWithConditionals,
            bool iAddTypes,
            bool iAddVars)
        {
            string vActualInterface;
            string vActualImplementation;

            string vInterface = "";
            string vImplementation = "";
            if (iHasInterface)
                vInterface = GetDefaultUnits(2, iWithPaths, iRelative, iWithConditionals).ToString();

            if (iHasImplementation)
                vImplementation = GetDefaultUnits(3, iWithPaths, iRelative, iWithConditionals).ToString();
            string vTestValue = ConstructUnit(vInterface, vImplementation, iAddTypes, iAddVars);

            UsesClauseReader.ExtractUses(vTestValue, out vActualInterface, out vActualImplementation);

            Assert.AreEqual(vInterface, vActualInterface, "Uses clause from interface is wrong");
            Assert.AreEqual(vImplementation, vActualImplementation, "Uses clause from implementation is wrong");
        }

        protected void CheckUnitList(string iUsesClause, IList<string> iExpectedList, string iErrorMsg)
        {
        	if (iExpectedList.Count > 0)
        		Assert.AreNotEqual("", iUsesClause, "Parse uses clause is empty. " + iErrorMsg);
        	UnitList vActual = UsesClauseReader.GetUnits(iUsesClause);
        	Assert.AreEqual(iExpectedList.Count, vActual.Count, "Incorrect no. of units on list. " + iErrorMsg);
        	foreach (string vUnitName in iExpectedList)
        		Assert.AreNotEqual(-1, vActual.IndexOf(vUnitName), "Unit " + vUnitName + " was not found on the list. " + iErrorMsg);
        }
        
        protected void TestGetUnits(string iTestValue, IList<string> iExpectedInterfaceUnits, IList<string> iExpectedImplementationUnits)
        {
        	string vInterface, vImplementation;
        	UsesClauseReader.ExtractUses(iTestValue, out vInterface, out vImplementation);
        	CheckUnitList(vInterface, iExpectedInterfaceUnits, "Interface parsing error");
        	CheckUnitList(vImplementation, iExpectedImplementationUnits, "Implementation parsing error");
        }

        [Test]
        public void GetUnits_WithoutPath()
        {
            DoTestGetUnits(8, false, false, false);
        }

        [Test]
        public void GetUnits_WithPath()
        {
            DoTestGetUnits(8, true, false, false);
        }

        [Test]
        public void GetUnits_WithPath_Relative()
        {
            DoTestGetUnits(8, true, true, false);
        }

        [Test]
        public void GetUnits_WithoutPaths_WithConditions()
        {
            DoTestGetUnits(8, false, false, true);
        }

        [Test]
        public void GetUnits_WitPaths_WithConditions()
        {
            DoTestGetUnits(8, true, false, true);
        }

        [Test]
        public void GetUnits_WithPaths_Relative_WithConditions()
        {
            DoTestGetUnits(8, true, true, true);
        }

        [Test]
        public void GetUnits_MultipleUnitsInConditionals()
        {
            DoTestGetUnits_MultipleUnitsInConditionals(2, 0, 1, false, false);
        }

        [Test]
        public void GetUnits_MultipleUnitsInConditionals_WithPaths()
        {
            DoTestGetUnits_MultipleUnitsInConditionals(2, 0, 1, true, false);
        }

        [Test]
        public void GetUnits_MultipleUnitsInConditionals_WithPaths_Relative()
        {
            DoTestGetUnits_MultipleUnitsInConditionals(2, 0, 1, true, true);
        }

        [Test]
        public void GetUnits_MultipleUnitsInConditionals_4()
        {
            DoTestGetUnits_MultipleUnitsInConditionals(4, 1, 2, false, false);
        }

        [Test]
        public void GetUnits_MultipleUnitsInConditionals_4_WithPaths()
        {
            DoTestGetUnits_MultipleUnitsInConditionals(4, 1, 2, true, false);
        }

        [Test]
        public void GetUnits_MultipleUnitsInConditionals_4_WithPaths_Relative()
        {
            DoTestGetUnits_MultipleUnitsInConditionals(4, 1, 2, true, true);
        }

        [Test]
        public void ExtractUses_2Sections_Simple()
        {
            DoTestExtractUses(true, true, false, false, false, false, false);
        }

        [Test]
        public void ExtractUses_2Sections_Simple_WithConditionals()
        {
            DoTestExtractUses(true, true, false, false, true, false, false);
        }

        [Test]
        public void ExtractUses_2Sections_Simple_WithPaths()
        {

            DoTestExtractUses(true, true, true, false, false, false, false);
        }

        [Test]
        public void ExtractUses_2Sections_Simple_WithPaths_Relative()
        {

            DoTestExtractUses(true, true, true, true, false, false, false);
        }

        [Test]
        public void ExtractUses_2Sections_Simple_WithPathsConditionals()
        {

            DoTestExtractUses(true, true, true, false, true, false, false);
        }

        [Test]
        public void ExtractUses_2Sections_Simple_WithPathsConditionals_Relative()
        {

            DoTestExtractUses(true, true, true, true, true, false, false);
        }

        [Test]
        public void ExtractUses_2Sections_WithTypes()
        {
            DoTestExtractUses(true, true, false, false, false, true, false);
        }

        [Test]
        public void ExtractUses_2Sections_WithTypes_WithConditionals()
        {
            DoTestExtractUses(true, true, false, false, true, true, false);
        }

        [Test]
        public void ExtractUses_2Sections_WithTypes_WithPaths()
        {

            DoTestExtractUses(true, true, true, false, false, true, false);
        }

        [Test]
        public void ExtractUses_2Sections_WithTypes_WithPaths_Relative()
        {

            DoTestExtractUses(true, true, true, true, false, true, false);
        }

        [Test]
        public void ExtractUses_2Sections_WithTypes_WithPathsConditionals()
        {

            DoTestExtractUses(true, true, true, false, true, true, false);
        }

        [Test]
        public void ExtractUses_2Sections_WithTypes_WithPathsConditionals_Relative()
        {

            DoTestExtractUses(true, true, true, true, true, true, false);
        }

        [Test]
        public void ExtractUses_2Sections_WithVars()
        {
            DoTestExtractUses(true, true, false, false, false, false, true);
        }

        [Test]
        public void ExtractUses_2Sections_WithVars_WithConditionals()
        {
            DoTestExtractUses(true, true, false, false, true, false, true);
        }

        [Test]
        public void ExtractUses_2Sections_WithVars_WithPaths()
        {

            DoTestExtractUses(true, true, true, false, false, false, true);
        }

        [Test]
        public void ExtractUses_2Sections_WithVars_WithPaths_Relative()
        {

            DoTestExtractUses(true, true, true, true, false, false, true);
        }

        [Test]
        public void ExtractUses_2Sections_WithVars_WithPathsConditionals()
        {

            DoTestExtractUses(true, true, true, false, true, false, true);
        }

        [Test]
        public void ExtractUses_2Sections_WithVars_WithPathsConditionals_Relative()
        {

            DoTestExtractUses(true, true, true, true, true, false, true);
        }

        [Test]
        public void ExtractUses_2Sections_WithTypesVars()
        {
            DoTestExtractUses(true, true, false, false, false, true, true);
        }

        [Test]
        public void ExtractUses_2Sections_WithTypesVars_WithConditionals()
        {
            DoTestExtractUses(true, true, false, false, true, true, true);
        }

        [Test]
        public void ExtractUses_2Sections_WithTypesVars_WithPaths()
        {

            DoTestExtractUses(true, true, true, false, false, true, true);
        }

        [Test]
        public void ExtractUses_2Sections_WithTypesVars_WithPaths_Relative()
        {

            DoTestExtractUses(true, true, true, true, false, true, true);
        }

        [Test]
        public void ExtractUses_2Sections_WithTypesVars_WithPathsConditionals()
        {

            DoTestExtractUses(true, true, true, false, true, true, true);
        }

        [Test]
        public void ExtractUses_2Sections_WithTypesVars_WithPathsConditionals_Relative()
        {

            DoTestExtractUses(true, true, true, true, true, true, true);
        }

        [Test]
        public void ExtractUses_InterfaceOnly()
        {
            DoTestExtractUses(true, false, false, false, false, true, true);
        }

        [Test]
        public void ExtractUses_ImplementationOnly()
        {
            DoTestExtractUses(false, true, false, false, false, true, true);
        }

        [Test]
        public void ExtractUses_Project()
        {
            #region (Test value)

            string vTestValue =
                @"{WXPFIB}" + "\n" +
                @"program WXPFIB;" + "\n" +
                @"" + "\n" +
                @"uses" + "\n" +
                @"    {$IFDEF WINRUNNER}" + "\n" +
                @"    TestSrvr ," + "\n" +
                @"    {$ENDIF}" + "\n" +
                @"    Forms ," + "\n" +
                @"    DBUNIT ," + "\n" +
                @"    GDECLARE ," + "\n" +
                @"    SIGNIN ," + "\n" +
                @"    ViewScrn ," + "\n" +
                @"    uWXPFIB ," + "\n" +
                @"    oWXPFIB in '..\FORMS\oWXPFIB.pas' {sWXPFIB}" + "\n" +
                @"    ;" + "\n" +
                @"{$R *.RES}" + "\n" +
                @"" + "\n" +
                @"begin" + "\n" +
                @"    Application.Initialize;" + "\n" +
                @"    Application.Title := 'Weekly World Wide Briefing Report';" + "\n" +
                @"    Application.CreateForm(TsWXPFIB, sWXPFIB);" + "\n" +
                @"    Application.CreateForm(TViewForm , ViewForm);" + "\n" +
                @"    Application.Run;" + "\n" +
                @"end." + "\n";

            #endregion

            string vExpected =
                @"uses" + "\n" +
                @"    {$IFDEF WINRUNNER}" + "\n" +
                @"    TestSrvr ," + "\n" +
                @"    {$ENDIF}" + "\n" +
                @"    Forms ," + "\n" +
                @"    DBUNIT ," + "\n" +
                @"    GDECLARE ," + "\n" +
                @"    SIGNIN ," + "\n" +
                @"    ViewScrn ," + "\n" +
                @"    uWXPFIB ," + "\n" +
                @"    oWXPFIB in '..\FORMS\oWXPFIB.pas' {sWXPFIB}" + "\n" +
                @"    ;";
            string vActual = UsesClauseReader.ExtractUses(vTestValue);
            Assert.AreEqual(vExpected, vActual);
        }

        [Test]
        public void ExtractUses_LibraryWithDefaultComments()
        {
            #region (Test value)

            string vTestValue = 
                @"library FlightInfo;" + "\n" +
                @"" + "\n" +
                @"{ Important note about DLL memory management: ShareMem must be the" + "\n" +
                @"  first unit in your library's USES clause AND your project's (select" + "\n" +
                @"  Project-View Source) USES clause if your DLL exports any procedures or" + "\n" +
                @"  functions that pass strings as parameters or function results. This" + "\n" +
                @"  applies to all strings passed to and from your DLL--even those that" + "\n" +
                @"  are nested in records and classes. ShareMem is the interface unit to" + "\n" +
                @"  the BORLNDMM.DLL shared memory manager, which must be deployed along" + "\n" +
                @"  with your DLL. To avoid using BORLNDMM.DLL, pass string information" + "\n" +
                @"  using PChar or ShortString parameters. }" + "\n" +
                @"" + "\n" +
                @"uses" + "\n" +
                @"  UpdateArrivalFlightInfoClass in '..\Web\FT\Server\UpdateArrivalFlightInfoClass.pas';" + "\n" +
                @"" + "\n" +
                @"function GetData(const iRequest : PChar; const iMetaData : PChar;" + "\n" +
                @"                 var iResponse: PChar) : Boolean; stdcall;" + "\n" +
                @"var" + "\n" +
                @"  vServiceProvider  : TRemoteServiceProvider;" + "\n" +
                @"  vLogger: TLogger;" + "\n" +
                @"begin" + "\n" +
                @"  Result := False;" + "\n" +
                @"  vLogger := TLogger.Create(LC_WEB_ENTRY_POINTS);" + "\n" +
                @"  vServiceProvider := TRemoteServiceProvider.Create;" + "\n" +
                @"  try" + "\n" +
                @"    try" + "\n" +
                @"      Result := vServiceProvider.GetData(iRequest, iMetaData, iResponse);" + "\n" +
                @"    except" + "\n" +
                @"      on e : Exception do" + "\n" +
                @"      vLogger.error('FlightInfo', 'GetData', e.Message);" + "\n" +
                @"    end;" + "\n" +
                @"  finally" + "\n" +
                @"    vServiceProvider.Free;" + "\n" +
                @"    vLogger.Free;" + "\n" +
                @"  end;" + "\n" +
                @"end;" + "\n" +
                @"" + "\n" +
                @"" + "\n" +
                @"procedure FreePChar(const p: PChar); stdcall;" + "\n" +
                @"begin" + "\n" +
                @"   TRemoteServiceProvider.FreePChar(p);" + "\n" +
                @"end;" + "\n" +
                @"" + "\n" +
                @"exports" + "\n" +
                @"  GetData," + "\n" +
                @"  FreePChar;" + "\n" +
                @"" + "\n" +
                @"var" + "\n" +
                @"  vRSP : TRemoteServiceProvider;" + "\n" +
                @"  vLogger: TLogger;" + "\n" +
                @"begin" + "\n" +
                @"  TLogger.Configure('log4d.properties');" + "\n" +
                @"  vRSP := TRemoteServiceProvider.Create;" + "\n" +
                @"  vLogger := TLogger.Create(LC_WEB_ENTRY_POINTS);" + "\n" +
                @"  try" + "\n" +
                @"    try" + "\n" +
                @"      vRSP.SetUpFromConfig();" + "\n" +
                @"    except" + "\n" +
                @"      on e : Exception do" + "\n" +
                @"        vLogger.error('FlightInfo', 'StartUp method', e.Message);" + "\n" +
                @"    end;" + "\n" +
                @"  finally" + "\n" +
                @"    vRSP.Free;" + "\n" +
                @"    vLogger.Free;" + "\n" +
                @"  end;" + "\n" +
                @"end.";

            #endregion

            string vExpected =
                @"uses" + "\n" +
                @"  UpdateArrivalFlightInfoClass in '..\Web\FT\Server\UpdateArrivalFlightInfoClass.pas';";

            string vActual = UsesClauseReader.ExtractUses(vTestValue);
            Assert.AreEqual(vExpected, vActual);
        }

        [Test]
        public void GetUnits_FormComments()
        {
            #region (Test value)

            string vTestValue = 
                @"library FlightInfo;" + "\n" +
                @"" + "\n" +
                @"{ Important note about DLL memory management: ShareMem must be the" + "\n" +
                @"  first unit in your library's USES clause AND your project's (select" + "\n" +
                @"  Project-View Source) USES clause if your DLL exports any procedures or" + "\n" +
                @"  functions that pass strings as parameters or function results. This" + "\n" +
                @"  applies to all strings passed to and from your DLL--even those that" + "\n" +
                @"  are nested in records and classes. ShareMem is the interface unit to" + "\n" +
                @"  the BORLNDMM.DLL shared memory manager, which must be deployed along" + "\n" +
                @"  with your DLL. To avoid using BORLNDMM.DLL, pass string information" + "\n" +
                @"  using PChar or ShortString parameters. }" + "\n" +
                @"" + "\n" +
                @"uses" + "\n" +
                @"  UpdateArrivalFlightInfoClass in '..\Web\FT\Server\UpdateArrivalFlightInfoClass.pas' {test_form_name};" + "\n" +
                @"" + "\n" +
                @"function GetData(const iRequest : PChar; const iMetaData : PChar;" + "\n" +
                @"                 var iResponse: PChar) : Boolean; stdcall;" + "\n" +
                @"var" + "\n" +
                @"  vServiceProvider  : TRemoteServiceProvider;" + "\n" +
                @"  vLogger: TLogger;" + "\n" +
                @"begin" + "\n" +
                @"  Result := False;" + "\n" +
                @"  vLogger := TLogger.Create(LC_WEB_ENTRY_POINTS);" + "\n" +
                @"  vServiceProvider := TRemoteServiceProvider.Create;" + "\n" +
                @"  try" + "\n" +
                @"    try" + "\n" +
                @"      Result := vServiceProvider.GetData(iRequest, iMetaData, iResponse);" + "\n" +
                @"    except" + "\n" +
                @"      on e : Exception do" + "\n" +
                @"      vLogger.error('FlightInfo', 'GetData', e.Message);" + "\n" +
                @"    end;" + "\n" +
                @"  finally" + "\n" +
                @"    vServiceProvider.Free;" + "\n" +
                @"    vLogger.Free;" + "\n" +
                @"  end;" + "\n" +
                @"end;" + "\n" +
                @"" + "\n" +
                @"" + "\n" +
                @"procedure FreePChar(const p: PChar); stdcall;" + "\n" +
                @"begin" + "\n" +
                @"   TRemoteServiceProvider.FreePChar(p);" + "\n" +
                @"end;" + "\n" +
                @"" + "\n" +
                @"exports" + "\n" +
                @"  GetData," + "\n" +
                @"  FreePChar;" + "\n" +
                @"" + "\n" +
                @"var" + "\n" +
                @"  vRSP : TRemoteServiceProvider;" + "\n" +
                @"  vLogger: TLogger;" + "\n" +
                @"begin" + "\n" +
                @"  TLogger.Configure('log4d.properties');" + "\n" +
                @"  vRSP := TRemoteServiceProvider.Create;" + "\n" +
                @"  vLogger := TLogger.Create(LC_WEB_ENTRY_POINTS);" + "\n" +
                @"  try" + "\n" +
                @"    try" + "\n" +
                @"      vRSP.SetUpFromConfig();" + "\n" +
                @"    except" + "\n" +
                @"      on e : Exception do" + "\n" +
                @"        vLogger.error('FlightInfo', 'StartUp method', e.Message);" + "\n" +
                @"    end;" + "\n" +
                @"  finally" + "\n" +
                @"    vRSP.Free;" + "\n" +
                @"    vLogger.Free;" + "\n" +
                @"  end;" + "\n" +
                @"end.";

            #endregion

            string vExpected =
                @"uses" + "\n" +
                @"  UpdateArrivalFlightInfoClass in '..\Web\FT\Server\UpdateArrivalFlightInfoClass.pas' {test_form_name};";

            string vActual = UsesClauseReader.ExtractUses(vTestValue);
            Assert.AreEqual(vExpected, vActual, "Wrong uses clause returned");
            UnitList vList = UsesClauseReader.GetUnits(vActual);
            Assert.AreEqual(1, vList.Count, "Wrong number of units returned");
            UnitItem vItem = vList[0];
            Assert.IsTrue(vItem.HasForm, "UnitItem should indicated to have form");
            Assert.AreEqual("test_form_name", vItem.Form, "Form name is wrong");
        }
        
        [Test]
        public void GetUnits_CommentsBetweenUnitNames()
        {
        	#region (Test value)
        	
        	string cACWGTSBroker =
	            "unit ACWGTSBroker;\r\n" +
	            "\r\n" +
	            "interface\r\n" +
	            "\r\n" +
	            "uses\r\n" +
	            "  oConsole,\r\n" +
	            "  // system units\r\n" +
	            "  Classes, SysUtils, Contnrs,\r\n" +
	            "  // DAL-specific units\r\n" +
	            "  // 2nd line comment,\r\n" +
	            "  // 3rd line comment; Part after comment\r\n" +
	            "  QueryInterface,\r\n" +
	            "  DMBaseBrokerClass,\r\n" +
	            "  DMBaseSQLBrokerClass,\r\n" +
	            "  DMBaseArchSQLBrokerClass,\r\n" +
	            "  DatabaseProviderFactoryClass,\r\n" +
	            "  DatabaseQueryFactoryClass,\r\n" +
	            "  AbstractQuery,\r\n" +
	            "  MetaTableAgentClass,\r\n" +
	            "  //io units\r\n" +
	            "  ACWGTSio\r\n" +
	            "  ;\r\n" +
	            "\r\n" +
	            "type\r\n" +
	            "  TACWGTSData = class(TObject)\r\n" +
	            "  private\r\n" +
	            "    FData: ACWGTSRecordType;\r\n" +
	            "  public\r\n" +
	            "    property Data: ACWGTSRecordType read FData;\r\n" +
	            "  end;\r\n" +
	            "\r\n" +
	            "  TACWGTSDataList = class(TObjectList)\r\n" +
	            "  private\r\n" +
	            "    function GetItem(Index: Integer): TACWGTSData;\r\n" +
	            "  public\r\n" +
	            "    property Items[Index: Integer]: TACWGTSData read GetItem; default;\r\n" +
	            "  end;\r\n" +
	            "\r\n" +
	            "  // Interface to access brokers\r\n" +
	            "  ILoadACWGTSQuery = interface(IQuery)\r\n" +
	            "    procedure GetACWGTSRecord(\r\n" +
	            "                TailNumber,\r\n" +
	            "                Config,\r\n" +
	            "                Comp: string;\r\n" +
	            "            var ACWGTSRecord: ACWGTSRecordType);\r\n" +
	            "  end;\r\n" +
	            "\r\n" +
	            "  // SQL broker\r\n" +
	            "  TACWGTSSqlBroker = class(TDMBaseArchSQLBroker, ILoadACWGTSQuery)\r\n" +
	            "  private\r\n" +
	            "    function GetACWGTSKey(\r\n" +
	            "                const iACWGTSRecord : ACWGTSRecordType): AnsiString;\r\n" +
	            "  protected\r\n" +
	            "    procedure MoveToObject(const iAbstractQuery: TAbstractQuery; const iMoveable: TObject); override;\r\n" +
	            "    function CreateMoveable(const iAbstractQuery: TAbstractQuery): TObject; override;\r\n" +
	            "    function GetTableName: string; override;\r\n" +
	            "  public\r\n" +
	            "    procedure GetACWGTSRecord(\r\n" +
	            "                TailNumber,\r\n" +
	            "                Config,\r\n" +
	            "                Comp: string;\r\n" +
	            "            var ACWGTSRecord: ACWGTSRecordType);\r\n" +
	            "    function ResolveInterface(const iType: TQueryType): IQuery; override;\r\n" +
	            "  end;\r\n" +
	            "\r\n" +
	            "  // Btrive broker\r\n" +
	            "  TACWGTSBtrvBroker = class(TDMBaseBroker, ILoadACWGTSQuery)\r\n" +
	            "  private\r\n" +
	            "    procedure GetACWGTSRecord(\r\n" +
	            "                TailNumber,\r\n" +
	            "                Config,\r\n" +
	            "                Comp: string;\r\n" +
	            "            var ACWGTSRecord: ACWGTSRecordType);\r\n" +
	            "  public\r\n" +
	            "    function ResolveInterface(const iType: TQueryType): IQuery; override;\r\n" +
	            "  end;\r\n" +
	            "\r\n" +
	            "  TACWGTSDataAcquisitor = class\r\n" +
	            "    class procedure GetACWGTSRecord(\r\n" +
	            "            TailNumber,\r\n" +
	            "            Config,\r\n" +
	            "            Comp: string;\r\n" +
	            "        var ACWGTSRecord: ACWGTSRecordType);\r\n" +
	            "  end;\r\n" +
	            "\r\n" +
	            "implementation\r\n" +
	            "\r\n" +
	            "uses\r\n" +
	            "  MyStd,\r\n" +
	            "  DMGlobals,\r\n" +
	            "  CommonConsts,\r\n" +
	            "  OracleKeywordClass,\r\n" +
	            "  DMSQLQueryFactory,\r\n" +
	            "  DMBtrvQueryFactory,\r\n" +
	            "  DMUtils,\r\n" +
	            "  gdeclare,\r\n" +
	            "  DBUNIT,\r\n" +
	            "  Datetime;\r\n" +
	            "\r\n";
        	
        	#endregion

        	TestGetUnits(cACWGTSBroker,
        	             new string[] {"oConsole", "Classes", "SysUtils", "Contnrs", "QueryInterface", 
        	             	"DMBaseBrokerClass", "DMBaseSQLBrokerClass", "DMBaseArchSQLBrokerClass", 
        	             	"DatabaseProviderFactoryClass", "DatabaseQueryFactoryClass", "AbstractQuery", 
        	             	"MetaTableAgentClass", "ACWGTSio"},
        	             new string[] {"MyStd", "DMGlobals", "CommonConsts", "OracleKeywordClass", "DMSQLQueryFactory",
        	             	"DMBtrvQueryFactory", "DMUtils", "gdeclare", "DBUNIT", "Datetime"});
        }
        
        [Test]
        public void GetUnits_DateTime()
        {
        	#region (Test value)
        	
        	const string cDateTime = 
"{ DATETIME PAS        68,928  05-31-96  6:10p DCO 119 }\r\n" +
                "unit Datetime;\r\n" +
                "{$O+,F+}\r\n" +
                "interface\r\n" +
                "\r\n" +
                "uses\r\n" +
                "  SYSUTILS,\r\n" +
                "  Classes,\r\n" +
                "  Math,\r\n" +
                "  Windows;\r\n" +
                "\r\n" +
                "const\r\n" +
                "    MIN_PER_HR                               = 60;\r\n" +
                "    MIN_PER_DAY                              = 1440; { 24 Hr/Day * 60 Min/Hr }\r\n" +
                "    SEC_PER_DAY                              = 86400; { 24 Hr/Day * 60 Min/Hr * 60 Sec/Min }\r\n" +
                "    SEC_PER_HR                               = 3600; { 60 Min/Hr * 60 Sec/Min }\r\n" +
                "    SEC_PER_MIN                              = 60;\r\n" +
                "\r\n" +
                "    MAX_EARLY_DEP_MINS                       = (4 * MIN_PER_HR);\r\n" +
                "    MAX_FLT_LENGTH_MINS                      = (20 * MIN_PER_HR);\r\n" +
                "\r\n" +
                "{ TODO : Copied from Drb\\DateTime.pas }\r\n" +
                "  coDatePartSize = 7;\r\n" +
                "  coTimePartSize = 4;\r\n" +
                "  coBlankDate : ShortString = '       ';\r\n" +
                "  coBlankTime : ShortString = '    ';\r\n" +
                "  coDRBMaxNumericDate = #99#99#05#15;\r\n" +
                "var\r\n" +
                "  coMaxDate : TDateTime;\r\n" +
                "\r\n" +
                "type\r\n" +
                "    FlightTimeType = record\r\n" +
                "        Date :array[1..8] of char;\r\n" +
                "        Time :smallint;\r\n" +
                "        UTCVariance :smallint;\r\n" +
                "    end;\r\n" +
                "\r\n" +
                "    EagleTimeType = string[4]; { HHMM }\r\n" +
                "    NumericDateType = string[4]; { chr(CC), chr (YY), chr (MM), chr (DD) }\r\n" +
                "    UpdateTimeType = string[4]; { HHMM }\r\n" +
                "    UTCVarianceType = string[5]; { +/-HHMM }\r\n" +
                "    UniversalTimeType = string[6]; { HHMMSS }\r\n" +
                "    UniversalTimeTypeMS = string[9]; { HHMMSSTTT }\r\n" +
                "    UniversalDateType = string[7]; { DDMMMYY }\r\n" +
                "    CCYYMMDDDateType = array[1..8] of Char; // was string[8]; { CCYYMMDD }\r\n" +
                "    CrewDateType =  CCYYMMDDDateType;\r\n" +
                "    USDateType = string[6]; { MMDDYY }\r\n" +
                "\r\n" +
                "function EagleDateToDateTime(const StringDateTime: ShortString): TDateTime; overload; // DMB\r\n" +
                "function EagleDateToDateTime(const StringDateTime: ShortString; var ivIsOK: Boolean): TDateTime; overload;\r\n" +
                "function DateTimetoEagleDate(const DateTimeIn :TDateTime) :shortstring;\r\n" +
                "function EagleDateTimeToDateTime(const iDate, iTime: ShortString): TDateTime; overload;\r\n" +
                "function EagleDateTimeToDateTime(const iDate, iTime: ShortString; var ivIsOK: Boolean): TDateTime; overload;\r\n" +
                "function EagleTimeToDateTime(const iTime: ShortString; const iFixTime : Boolean = False): TDateTime;\r\n" +
                "function DateTimeToEagleTime(const iDateTime: TDateTime) : ShortString;\r\n" +
                "\r\n" +
                "function FormatToHHNNDDMMYY(const iDateTime :TDateTime):AnsiString;\r\n" +
                "procedure GetUTCDates\r\n" +
                "              (const iUTCNumericDepartureDate: NumericDateType;\r\n" +
                "               const iDepartureTime: EagleTimeType;\r\n" +
                "               const iArrivalTime: EagleTimeType;\r\n" +
                "               var iUTCDeparture: TDateTime;\r\n" +
                "               var iUTCArrival: TDateTime);\r\n" +
                "function ConvertToDateTime(const iEagleTime: EagleTimeType; const iNumericDate: NumericDateType):TDateTime;\r\n" +
                "procedure DecodeEagleTime(iEagleTime:string; var Hour, Min: Word);\r\n" +
                "procedure DecodeNumericDate(const iNumDate :string; var Year, Month, Day: Word);\r\n" +
                "\r\n" +
                "function NumericToYYYYMMDD(iNumDate :string) :string;\r\n" +
                "\r\n" +
                "function YYYYMMDDtoNumericDate(iYYYYMMDD :string) :string;\r\n" +
                "\r\n" +
                "function ValidUSDate(\r\n" +
                "    USDate :USDateType\r\n" +
                "    )\r\n" +
                "    :boolean;\r\n" +
                "\r\n" +
                "function USToUniversalDate\r\n" +
                "    (\r\n" +
                "    USDate :USDateType;\r\n" +
                "    var UniversalDate :UniversalDateType\r\n" +
                "    )\r\n" +
                "    :boolean;\r\n" +
                "\r\n" +
                "function UniversalToUSDate\r\n" +
                "    (\r\n" +
                "    UniversalDate :UniversalDateType;\r\n" +
                "    var USDate :USDateType\r\n" +
                "    )\r\n" +
                "    :boolean;\r\n" +
                "\r\n" +
                "(**************************************************************************************************\r\n" +
                "Converts the DDMONYY date & the time to TDATETIME format\r\n" +
                "SB 09NOV1999\r\n" +
                "**************************************************************************************************)\r\n" +
                "function UniversalDateToTDATE(NumericDate :NumericDateType;\r\n" +
                "    APTime :integer;\r\n" +
                "    var DateTimeasSysFormat :TDateTime) :boolean;\r\n" +
                "\r\n" +
                "function ValidUniversalDate(\r\n" +
                "    ChkDate :UniversalDateType\r\n" +
                "    )\r\n" +
                "    :boolean;\r\n" +
                "\r\n" +
                "function ValidEagleTime(\r\n" +
                "    ChkTime :EagleTimeType\r\n" +
                "    )\r\n" +
                "    :boolean;\r\n" +
                "\r\n" +
                "function ValidUniversalTime(\r\n" +
                "    ChkTime :UniversalTimeType\r\n" +
                "    )\r\n" +
                "    :boolean;\r\n" +
                "\r\n" +
                "function GetUniversalDateTime\r\n" +
                "    (\r\n" +
                "    var CurrDate :UniversalDateType;\r\n" +
                "    var CurrTime :UniversalTimeType;\r\n" +
                "    var wSec100 :word\r\n" +
                "    )\r\n" +
                "    :boolean;\r\n" +
                "\r\n" +
                "procedure GetCurrentDateTime\r\n" +
                "    (\r\n" +
                "    var CurrDate :UniversalDateType;\r\n" +
                "    var wCurrHour :word;\r\n" +
                "    var wCurrMinute :word;\r\n" +
                "    var wCurrSecond :word;\r\n" +
                "    var wCurrSec100 :word\r\n" +
                "    );\r\n" +
                "\r\n" +
                "function ConvertEagleTime\r\n" +
                "    (\r\n" +
                "    EagleTime :EagleTimeType;\r\n" +
                "    var wHour :word;\r\n" +
                "    var wMinute :word\r\n" +
                "    )\r\n" +
                "    :boolean;\r\n" +
                "\r\n" +
                "function ConvertUniversalTime\r\n" +
                "    (\r\n" +
                "    UniversalTime :UniversalTimeType;\r\n" +
                "    var wHour :word;\r\n" +
                "    var wMinute :word;\r\n" +
                "    var wSecond :word\r\n" +
                "    )\r\n" +
                "    :boolean;\r\n" +
                "\r\n" +
                "function TimeDiff\r\n" +
                "    (\r\n" +
                "    var ErrorRet :integer;\r\n" +
                "    var Minutes :longint;\r\n" +
                "    FirstDate :UniversalDateType;\r\n" +
                "    FirstTime :EagleTimeType;\r\n" +
                "    SecondDate :UniversalDateType;\r\n" +
                "    SecondTime :EagleTimeType\r\n" +
                "    )\r\n" +
                "    :boolean;\r\n" +
                "\r\n" +
                "function UnivTimeDiff\r\n" +
                "    (\r\n" +
                "    var ErrorRet :integer;\r\n" +
                "    var Seconds :longint;\r\n" +
                "    FirstDate :UniversalDateType;\r\n" +
                "    FirstTime :UniversalTimeType;\r\n" +
                "    SecondDate :UniversalDateType;\r\n" +
                "    SecondTime :UniversalTimeType\r\n" +
                "    )\r\n" +
                "    :boolean;\r\n" +
                "\r\n" +
                "procedure SecSince01Jan1970\r\n" +
                "    (\r\n" +
                "    var Seconds :longint;\r\n" +
                "    var Sec1000 :longint\r\n" +
                "    );\r\n" +
                "\r\n" +
                "function CheckDDMMMYYDate\r\n" +
                "    (\r\n" +
                "    CheckDate :UniversalDateType\r\n" +
                "    )\r\n" +
                "    :boolean;\r\n" +
                "\r\n" +
                "function CompleteDate\r\n" +
                "    (\r\n" +
                "    var ReferenceDate :UniversalDateType;\r\n" +
                "    var PartialDate :UniversalDateType\r\n" +
                "    )\r\n" +
                "    :boolean;\r\n" +
                "\r\n" +
                "function SecondsSinceMidnight :longint;\r\n" +
                "\r\n" +
                "function GetUniversalTime :UniversalTimeType;\r\n" +
                "\r\n" +
                "function GetUniversalTimeMS :UniversalTimeTypeMS;\r\n" +
                "\r\n" +
                "function GetUniversalDate :UniversalDateType;\r\n" +
                "\r\n" +
                "function GetUniversalYear :string;\r\n" +
                "\r\n" +
                "function AddCenturyToYear(YearOfCentury :string) :string;\r\n" +
                "\r\n" +
                "function NumericDate\r\n" +
                "    (\r\n" +
                "    const iDate :UniversalDateType\r\n" +
                "    )\r\n" +
                "    :NumericDateType;\r\n" +
                "\r\n" +
                "function ValidateDate(var s :string) :boolean;\r\n" +
                "\r\n" +
                "function ValidateTime(s :string): Boolean;\r\n" +
                "\r\n" +
                "function ValidateGMTVariance(s :string): Boolean;\r\n" +
                "\r\n" +
                "function ValidateDateChange(s :string): Boolean;\r\n" +
                "\r\n" +
                "function ValidateDailyFrequency(s :string): Boolean;\r\n" +
                "\r\n" +
                "function JulianDay(DateNumeric :string) :longint;\r\n" +
                "\r\n" +
                "function DayAD(DateNumeric :string) :longint;\r\n" +
                "\r\n" +
                "function NumToDate(DateNumeric :string) :string;\r\n" +
                "\r\n" +
                "function DayOfWeekAD(DayAD :longint) :shortint;\r\n" +
                "\r\n" +
                "function DayOfWeekAlpha(DayOfWeek :shortint) :string;\r\n" +
                "\r\n" +
                "function NumericDayOfWeek(Date :TDateTime) :integer;\r\n" +
                "\r\n" +
                "function LeapYear(YearAD :integer) :boolean;\r\n" +
                "\r\n" +
                "function DayADToDate(DayAD :longint) :string;\r\n" +
                "\r\n" +
                "function NumericMonth(Month :string) :shortint;\r\n" +
                "\r\n" +
                "function AlphaMonth(MonthNumeric :shortint) :string;\r\n" +
                "\r\n" +
                "function TimeToMinutes(Time :string) :integer;\r\n" +
                "\r\n" +
                "function MinutesToTime(Minutes :integer) :string;\r\n" +
                "\r\n" +
                "function MinutesToTime5H(Minutes :LongInt) :string;\r\n" +
                "\r\n" +
                "function SecondsToTime(Seconds :integer) :string;\r\n" +
                "\r\n" +
                "function GMTMinuteAD(NumericLocalDate\r\n" +
                "    , LocalTime\r\n" +
                "    , GMTVariance\r\n" +
                "    , DateChange :string) :longint;\r\n" +
                "\r\n" +
                "function MinAdToTime(i: longint): string;\r\n" +
                "\r\n" +
                "function MinAdToDate(i: longint): string; overload;\r\n" +
                "function MinAdToDate(i : longint; var ivIsOK: Boolean): string; overload;\r\n" +
                "\r\n" +
                "procedure CalculateNextMonth(Date :string;\r\n" +
                "    var NextMonth , Year :string);\r\n" +
                "\r\n" +
                "function LocalToGMT(LocalTime , GMTVariance :string) :string;\r\n" +
                "\r\n" +
                "function GMTtoLocal(GMT , GMTVariance :string) :string;\r\n" +
                "\r\n" +
                "function TimeToAMPM(Time :string) :string;\r\n" +
                "\r\n" +
                "function TimeToAMPM1(Time :string) :string;\r\n" +
                "\r\n" +
                "function TimeToAMPM2(Time :string) :string;\r\n" +
                "\r\n" +
                "function TimeAMToTime(Time :string) :string;\r\n" +
                "\r\n" +
                "function TimeToTimeMaintenance(iTime :integer) :string;\r\n" +
                "\r\n" +
                "function EagleToCCYYMMDDDate(EagleDate :UniversalDateType) : CCYYMMDDDateType;\r\n" +
                "\r\n" +
                "function GetCrewDate(RefDateTime: TDateTime = 0) :CrewDateType;\r\n" +
                "\r\n" +
                "procedure EagleToCrewTracDate\r\n" +
                "    (EagleDate :UniversalDateType;\r\n" +
                "    var StartCrewTracDate :CrewDateType);\r\n" +
                "\r\n" +
                "procedure FlightEventUTCToLt\r\n" +
                "    (\r\n" +
                "    sNumFltDateUTC :NumericDateType;\r\n" +
                "    sSTDUTC :EagleTimeType;\r\n" +
                "    sFltEventUTC :EagleTimeType;\r\n" +
                "    sUTCVariance :UTCVarianceType;\r\n" +
                "    var stFltEventLt :FlightTimeType\r\n" +
                "    );\r\n" +
                "\r\n" +
                "procedure GMTtoOUTOFFONINMVTCrewTracTime\r\n" +
                "    (EagleNumGMTDate ,\r\n" +
                "    EagleSTDudt ,\r\n" +
                "    EagleETDudt ,\r\n" +
                "    EagleGMTTime ,\r\n" +
                "    EagleGMTVariance :string;\r\n" +
                "    var CrewTracTime :FlightTimeType);\r\n" +
                "\r\n" +
                "function YMD2DMYEagle(inDate :string) :string;\r\n" +
                "\r\n" +
                "function ZIPDate(iNumDate :string) :string;\r\n" +
                "\r\n" +
                "function ComputeElapsedMinutes(Time1udt , Time2udt :string) :integer;\r\n" +
                "\r\n" +
                "procedure Delay(DelayAmount :word);\r\n" +
                "\r\n" +
                "function GetUTCDate :TDateTime;\r\n" +
                "\r\n" +
                "function GetUTCTime :TDateTime;\r\n" +
                "\r\n" +
                "function GetUTCDateTime :TDateTime;\r\n" +
                "\r\n" +
                "function NowUTC :TDateTime;\r\n" +
                "\r\n" +
                "function NumericYesterday :string;\r\n" +
                "\r\n" +
                "function nowUTCMinute: TDateTime;\r\n" +
                "\r\n" +
                "procedure TimeIsUTCBased(Value :boolean); // vs. system time\r\n" +
                "\r\n" +
                "function ConvertDateTimeToUniversalDate(DateTime :TDateTime) :UniversalDateType;\r\n" +
                "\r\n" +
                "function NumericDateToUniversalDate(iNumDate :string) :UniversalDateType;\r\n" +
                "\r\n" +
                "function UniversalDateTimeToTDateTime\r\n" +
                "    (    UniversalDate: UniversalDateType;\r\n" +
                "         HHMM         : EagleTimeType;\r\n" +
                "     var iTDateTime   : TDateTime          ) : boolean;\r\n" +
                "\r\n" +
                "function TDateTimeToUniversalDateTime\r\n" +
                "    (    iTDateTime   : TDateTime;\r\n" +
                "     var UniversalDate: UniversalDateType;\r\n" +
                "     var HHMM         : EagleTimeType ) : boolean;\r\n" +
                "\r\n" +
                "function TDateTimeToDDHHMM (iTDateTime   : TDateTime) :string;\r\n" +
                "\r\n" +
                "function DDHHMMToTDateTime (iDDHHMM   : string; iRefTimestamp: TDateTime = -1) :TDateTime;\r\n" +
                "\r\n" +
                "Function NumericDateToDDMMMYY (NumericDateIn : String) : String;\r\n" +
                "\r\n" +
                "Function ValidNumericDate (Const NumericDateIn : String) : Boolean;\r\n" +
                "\r\n" +
                "function  DateTimeToDDMMMYY( NewDate : TDateTime ) : string;\r\n" +
                "\r\n" +
                "function NumericDateToDateTime(const iNumericDate : string): TDatetime;\r\n" +
                "\r\n" +
                "function  DDMMMYYDateToDateTime( DDMMMYYDate  : string ) : TDateTime;\r\n" +
                "\r\n" +
                "function NumericDateWithTimeToDateTime(iNumericDateTime: ShortString): TDateTime;\r\n" +
                "\r\n" +
                "function DateTimeToNumericDateWithTime(iValue: TDateTime): ShortString;\r\n" +
                "\r\n" +
                "function DateTimeToNumericDate(const DateTimeIn :TDateTime) :NumericDateType;\r\n" +
                "\r\n" +
                "function YYYYMMDDToTDateTime(iYYYYMMDD: AnsiString): TDateTime;\r\n" +
                "\r\n" +
                "function HHMMToTTime(iHHMM: AnsiString): TDateTime;\r\n" +
                "\r\n" +
                "implementation\r\n" +
                "\r\n" +
                "uses\r\n" +
                "    Mystd, Forms, Dialogs, DateUtils;\r\n" +
                "\r\n" +
                "const\r\n" +
                "    AnnualDaysToMonth                        :array[1..12] of integer\r\n" +
                "                                             = (0 , 31 , 59 , 90 , 120 , 151 , 181 , 212 , 243 , 273 , 304 , 334);\r\n" +
                "    AlphaMonths                              :array[1..12] of string[3]\r\n" +
                "                                             = ('JAN' , 'FEB' , 'MAR' , 'APR' , 'MAY' , 'JUN' , 'JUL' ,\r\n" +
                "        'AUG' , 'SEP' , 'OCT' , 'NOV' , 'DEC');\r\n" +
                "    AlphaDaysOfWeek                          :array[1..7] of string[3]\r\n" +
                "                                             = ('MON' , 'TUE' , 'WED' , 'THU' , 'FRI' , 'SAT' , 'SUN');\r\n" +
                "var\r\n" +
                "    AllTimeIsUTCBased                        :boolean;\r\n" +
                "    FTestDate: string[7];\r\n" +
                "{** 07-30-98 * ConvertDateTimeToUniversalDate *********************************}\r\n" +
                "\r\n" +
                "function ConvertDateTimeToUniversalDate(DateTime :TDateTime) :UniversalDateType;\r\n" +
                "\r\n" +
                "const\r\n" +
                "    MonthId                                  :array[1..12] of string[3] =\r\n" +
                "        ('JAN' , 'FEB' , 'MAR' , 'APR' , 'MAY' , 'JUN' , 'JUL' , 'AUG' , 'SEP' , 'OCT' , 'NOV' , 'DEC');\r\n" +
                "\r\n" +
                "var\r\n" +
                "    Day                                      :string[4];\r\n" +
                "    Year                                     :string[4];\r\n" +
                "    wDay                                     :word;\r\n" +
                "    wMonth                                   :word;\r\n" +
                "    wYear                                    :word;\r\n" +
                "\r\n" +
                "begin\r\n" +
                "    decodedate(DateTime , wYear , wMonth , wDay);\r\n" +
                "    str(wYear , Year);\r\n" +
                "    str((wDay + 1000) :4 , Day);\r\n" +
                "    Result := copy(Day , 3 , 2)\r\n" +
                "        + MonthId[wMonth]\r\n" +
                "        + copy(Year , 3 , 2);\r\n" +
                "end; //ConvertDateTimeToUniversalDate\r\n" +
                "\r\n" +
                "function FormatToHHNNDDMMYY(const iDateTime :TDateTime): AnsiString;\r\n" +
                "begin\r\n" +
                "  result := FormatDateTime('hhnnddmmyy', iDateTime);\r\n" +
                "end;\r\n" +
                "\r\n" +
                "function ConvertToDateTime(const iEagleTime: EagleTimeType; const iNumericDate: NumericDateType):TDateTime;\r\n" +
                "var\r\n" +
                "  Year, Month, Day  :Word;\r\n" +
                "  Hour, Minute      :Word;\r\n" +
                "begin\r\n" +
                "  DateTime.DecodeNumericDate(iNumericDate, Year, Month, Day);\r\n" +
                "  DateTime.DecodeEagleTime(iEagleTime, Hour, Minute);\r\n" +
                "  Result := EncodeDateTime(Year, Month, Day, Hour, Minute, 0{Second}, 0{MilliSecond});\r\n" +
                "end;\r\n" +
                "\r\n" +
                "procedure GetUTCDates\r\n" +
                "              (const iUTCNumericDepartureDate: NumericDateType;\r\n" +
                "               const iDepartureTime: EagleTimeType;\r\n" +
                "               const iArrivalTime: EagleTimeType;\r\n" +
                "               var iUTCDeparture: TDateTime;\r\n" +
                "               var iUTCArrival: TDateTime);\r\n" +
                "begin\r\n" +
                "  iUTCDeparture := ConvertToDateTime(iDepartureTime, iUTCNumericDepartureDate);\r\n" +
                "  iUTCArrival   := ConvertToDateTime(iArrivalTime,   iUTCNumericDepartureDate);\r\n" +
                "\r\n" +
                "  if (iUTCArrival > iUTCDeparture) then\r\n" +
                "  begin\r\n" +
                "    // ok\r\n" +
                "  end\r\n" +
                "  else begin\r\n" +
                "    iUTCArrival := IncDay(iUTCArrival, 1);\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "end;\r\n" +
                "\r\n" +
                "\r\n" +
                "procedure DecodeNumericDate(const iNumDate :string; var Year, Month, Day: Word);\r\n" +
                "var\r\n" +
                "  vDateTime: TDateTime;\r\n" +
                "begin\r\n" +
                "  vDateTime := NumericDateToDateTime(iNumDate);\r\n" +
                "  DecodeDate(vDateTime, Year, Month, Day);\r\n" +
                "end;\r\n" +
                "\r\n" +
                "procedure DecodeEagleTime(iEagleTime:string; var Hour, Min: Word);\r\n" +
                "\r\n" +
                "  function StrToWord(const str: string): word;\r\n" +
                "  var\r\n" +
                "    i: integer;\r\n" +
                "  begin\r\n" +
                "    result := 0;\r\n" +
                "    for i := 1 to 2 do // HH or MM\r\n" +
                "    begin\r\n" +
                "      if not (str[i] in ['0'..'9']) then\r\n" +
                "      begin\r\n" +
                "        result := 0;\r\n" +
                "        Break;\r\n" +
                "      end;\r\n" +
                "      result := (result*10) + ord(str[i]) - ord('0');\r\n" +
                "    end;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "var\r\n" +
                "  vHour      :string[2];\r\n" +
                "  vMin       :string[2];\r\n" +
                "begin\r\n" +
                "  iEagleTime := iEagleTime + '    ';\r\n" +
                "\r\n" +
                "  vHour := copy(iEagleTime, 1, 2);\r\n" +
                "  vMin  := copy(iEagleTime, 3, 2);\r\n" +
                "\r\n" +
                "  Hour := StrToWord(vHour);\r\n" +
                "  if (Hour > 23) then\r\n" +
                "  begin\r\n" +
                "    Hour := 0;\r\n" +
                "  end;\r\n" +
                "  Min  := StrToWord(vMin);\r\n" +
                "  if (Min > 59) then\r\n" +
                "  begin\r\n" +
                "    Min := 0;\r\n" +
                "  end;\r\n" +
                "end;\r\n" +
                "\r\n" +
                "function NumericToYYYYMMDD(iNumDate :string) :string;\r\n" +
                "var\r\n" +
                "    i                                        :integer;\r\n" +
                "    Century ,\r\n" +
                "        Day , Month , Year                   :string[2];\r\n" +
                "    Date                                     :string[8];\r\n" +
                "begin\r\n" +
                "    str(ord(iNumDate[4]) :2 , Day);\r\n" +
                "    str(ord(iNumDate[3]) :2 , Month);\r\n" +
                "    str(ord(iNumDate[2]) :2 , Year);\r\n" +
                "    str(ord(iNumDate[1]) :2 , Century);\r\n" +
                "    Date := Century + Year + Month + Day;\r\n" +
                "    for i := 1 to 8 do\r\n" +
                "    begin\r\n" +
                "        if Date[i] = ' ' then\r\n" +
                "            Date[i] := '0';\r\n" +
                "    end;\r\n" +
                "    NumericToYYYYMMDD := Date;\r\n" +
                "end;\r\n" +
                "\r\n" +
                "function YYYYMMDDtoNumericDate(iYYYYMMDD :string) :string;\r\n" +
                "var\r\n" +
                "    i                                        :integer;\r\n" +
                "    NCentury , NYear , NMonth , NDay         :ShortInt;\r\n" +
                "begin\r\n" +
                "    val(copy(iYYYYMMDD , 1 , 2) , NCentury , i);\r\n" +
                "    val(copy(iYYYYMMDD , 3 , 2) , NYear , i);\r\n" +
                "    val(copy(iYYYYMMDD , 5 , 2) , NMonth , i);\r\n" +
                "    val(copy(iYYYYMMDD , 7 , 2) , NDay , i);\r\n" +
                "    YYYYMMDDtoNumericDate := chr(NCentury)\r\n" +
                "        + chr(NYear)\r\n" +
                "        + chr(NMonth)\r\n" +
                "        + chr(NDay);\r\n" +
                "end;\r\n" +
                "\r\n" +
                "{** 05-24-96 * ValidUSDate ***********************************************}\r\n" +
                "\r\n" +
                "function ValidUSDate(\r\n" +
                "    USDate :USDateType\r\n" +
                "    )\r\n" +
                "    :boolean;\r\n" +
                "\r\n" +
                "var\r\n" +
                "    valid                                    :boolean;\r\n" +
                "    mm                                       :integer;\r\n" +
                "    dd                                       :integer;\r\n" +
                "    yy                                       :integer;\r\n" +
                "    i                                        :integer;\r\n" +
                "\r\n" +
                "begin { function ValidUSDate }\r\n" +
                "    valid := true;\r\n" +
                "    val(copy(USDate , 1 , 2) , mm , i);\r\n" +
                "    if i <> 0 then\r\n" +
                "        valid := false;\r\n" +
                "    if (valid) then\r\n" +
                "    begin\r\n" +
                "        val(copy(USDate , 3 , 2) , dd , i);\r\n" +
                "        if i <> 0 then\r\n" +
                "            valid := false\r\n" +
                "        else if (dd < 1) then\r\n" +
                "            valid := false;\r\n" +
                "        if (valid) then\r\n" +
                "        begin\r\n" +
                "            val(copy(USDate , 5 , 2) , yy , i);\r\n" +
                "            if i <> 0 then\r\n" +
                "                valid := false\r\n" +
                "            else if (yy < 0) then\r\n" +
                "                valid := false;\r\n" +
                "            if (valid) then\r\n" +
                "            begin\r\n" +
                "                if (mm >= 1) and (mm <= 12) then\r\n" +
                "                begin\r\n" +
                "                    case mm of\r\n" +
                "                        1 , 3 , 5 , 7 , 8 , 10 , 12\r\n" +
                "                            :\r\n" +
                "                            begin\r\n" +
                "                                if dd > 31 then\r\n" +
                "                                    valid := false;\r\n" +
                "                            end;\r\n" +
                "                        4 , 6 , 9 , 11\r\n" +
                "                            :\r\n" +
                "                            begin\r\n" +
                "                                if dd > 30 then\r\n" +
                "                                    valid := false;\r\n" +
                "                            end;\r\n" +
                "                        2\r\n" +
                "                            :\r\n" +
                "                            begin\r\n" +
                "                                if (LeapYear(yy)) then\r\n" +
                "                                begin\r\n" +
                "                                    if dd > 29 then\r\n" +
                "                                        valid := false;\r\n" +
                "                                end\r\n" +
                "                                else\r\n" +
                "                                begin\r\n" +
                "                                    if dd > 28 then\r\n" +
                "                                        valid := false;\r\n" +
                "                                end;\r\n" +
                "                            end;\r\n" +
                "                    end; { case }\r\n" +
                "                end { if }\r\n" +
                "                else\r\n" +
                "                    valid := false;\r\n" +
                "            end; { if }\r\n" +
                "        end; { if }\r\n" +
                "    end; { if }\r\n" +
                "    ValidUSDate := valid;\r\n" +
                "end; { function ValidUSDate }\r\n" +
                "\r\n" +
                "{** 05-24-96 * UniversalToUSDate *****************************************}\r\n" +
                "\r\n" +
                "{ FILENAME:    datetime.pas\r\n" +
                "\r\n" +
                "  DESCRIPTION: Converts DDMMMYY to MMDDYY.\r\n" +
                "\r\n" +
                "  PARAMETERS:  UniversalDate       Date in DDMMMYY format\r\n" +
                "               USDate              Date in MMDDYY format\r\n" +
                "\r\n" +
                "  RETURNS:     TRUE                Success\r\n" +
                "               FALSE               Failure\r\n" +
                "\r\n" +
                "  LOCAL:       Valid               Local flag\r\n" +
                "\r\n" +
                "--------------------------------------------------------------------------}\r\n" +
                "\r\n" +
                "function UniversalToUSDate\r\n" +
                "    (\r\n" +
                "    UniversalDate :UniversalDateType;\r\n" +
                "    var USDate :USDateType\r\n" +
                "    )\r\n" +
                "    :boolean;\r\n" +
                "\r\n" +
                "var\r\n" +
                "    i                                        :integer;\r\n" +
                "    valid                                    :boolean;\r\n" +
                "    sMonth                                   :string;\r\n" +
                "\r\n" +
                "begin { function UniversalToUSDate }\r\n" +
                "    valid := true;\r\n" +
                "    if (ValidUniversalDate(UniversalDate)) then\r\n" +
                "    begin\r\n" +
                "        sMonth := Copy(UniversalDate , 3 , 3);\r\n" +
                "        i := 0;\r\n" +
                "        repeat\r\n" +
                "            Inc(i);\r\n" +
                "        until (i > 12) or (sMonth = AlphaMonths[i]);\r\n" +
                "        if (i <= 12) then\r\n" +
                "        begin\r\n" +
                "            Str(i , sMonth);\r\n" +
                "            if (length(sMonth) = 1) then\r\n" +
                "                sMonth := '0' + sMonth;\r\n" +
                "            USDate := sMonth;\r\n" +
                "            USDate := USDate + Copy(UniversalDate , 1 , 2);\r\n" +
                "            USDate := USDate + Copy(UniversalDate , 6 , 2);\r\n" +
                "        end { if }\r\n" +
                "        else\r\n" +
                "            valid := false;\r\n" +
                "    end { if }\r\n" +
                "    else\r\n" +
                "        valid := false;\r\n" +
                "    UniversalToUSDate := valid;\r\n" +
                "end; { function UniversalToUSDate }\r\n" +
                "\r\n" +
                "(**************************************************************************************************\r\n" +
                "Converts the DDMONYY date & the time to TDATETIME format\r\n" +
                "SB 09NOV1999\r\n" +
                "**************************************************************************************************)\r\n" +
                "\r\n" +
                "function UniversalDateToTDATE(NumericDate :NumericDateType;\r\n" +
                "    APTime :integer;\r\n" +
                "    var DateTimeAsSysFormat :TDateTime) :boolean;\r\n" +
                "var\r\n" +
                "  vDateIncrement  :Integer;\r\n" +
                "  vCentury        : Word;\r\n" +
                "  vYear           : Word;\r\n" +
                "  vMonth          : Word;\r\n" +
                "  vDay            : Word;\r\n" +
                "  vHour           : Word;\r\n" +
                "  vMinute         : Word;\r\n" +
                "begin\r\n" +
                "  vDateIncrement := 0;\r\n" +
                "\r\n" +
                "  vHour   := 0;\r\n" +
                "  vMinute := 0;\r\n" +
                "  if APTime <> 0 then\r\n" +
                "  begin\r\n" +
                "      vDateIncrement := APTime div 1440;\r\n" +
                "      APTime  := APTime - (1440 * vDateIncrement);\r\n" +
                "      vHour   := APTime div 60;\r\n" +
                "      vMinute := APTime - (60 * vHour);\r\n" +
                "  end;\r\n" +
                "  try\r\n" +
                "      vCentury := Ord(NumericDate[1]);\r\n" +
                "      vYear    := Ord(NumericDate[2]);\r\n" +
                "      vMonth   := Ord(NumericDate[3]);\r\n" +
                "      vDay     := Ord(NumericDate[4]);\r\n" +
                "\r\n" +
                "      DateTimeAsSysFormat := EncodeDateTime((vCentury * 100) + vYear, vMonth, vDay, vHour, vMinute, 0 , 0);\r\n" +
                "      if vDateIncrement <> 0 then\r\n" +
                "          DateTimeAsSysFormat := DateTimeAsSysFormat + vDateIncrement;\r\n" +
                "      Result := True;\r\n" +
                "  except on EConvertError do\r\n" +
                "      begin\r\n" +
                "          Result := False;\r\n" +
                "      end;\r\n" +
                "  end;\r\n" +
                "end;\r\n" +
                "\r\n" +
                "{** 05-24-96 * USToUniversalDate *****************************************}\r\n" +
                "\r\n" +
                "{ FILENAME:    datetime.pas\r\n" +
                "\r\n" +
                "  DESCRIPTION: Converts MMDDYY to DDMMMYY.\r\n" +
                "\r\n" +
                "  PARAMETERS:  USDate              Date in MMDDYY format\r\n" +
                "               UniversalDate       Date in DDMMMYY format\r\n" +
                "\r\n" +
                "  RETURNS:     TRUE                Success\r\n" +
                "               FALSE               Failure\r\n" +
                "\r\n" +
                "  LOCAL:       Valid               Local flag\r\n" +
                "\r\n" +
                "--------------------------------------------------------------------------}\r\n" +
                "\r\n" +
                "function USToUniversalDate\r\n" +
                "    (\r\n" +
                "    USDate :USDateType;\r\n" +
                "    var UniversalDate :UniversalDateType\r\n" +
                "    )\r\n" +
                "    :boolean;\r\n" +
                "\r\n" +
                "var\r\n" +
                "    ErrorRet                                 :integer;\r\n" +
                "    iMonth                                   :integer;\r\n" +
                "    valid                                    :boolean;\r\n" +
                "\r\n" +
                "begin { function USToUniversalDate }\r\n" +
                "    valid := ValidUSDate(USDate);\r\n" +
                "    if valid then\r\n" +
                "    begin\r\n" +
                "    { Convert to Input Date/Time to Universal Formats }\r\n" +
                "        val(Copy(USDate , 1 , 2) , iMonth , ErrorRet);\r\n" +
                "        if (ErrorRet <> 0) then\r\n" +
                "            valid := false\r\n" +
                "        else if (iMonth < 1) or (iMonth > 12) then\r\n" +
                "            valid := false\r\n" +
                "        else {  valid }\r\n" +
                "        begin\r\n" +
                "            UniversalDate := Copy(USDate , 3 , 2);\r\n" +
                "            UniversalDate := UniversalDate + AlphaMonths[iMonth];\r\n" +
                "            UniversalDate := UniversalDate + Copy(USDate , 5 , 2);\r\n" +
                "        end; { else }\r\n" +
                "    end; { if }\r\n" +
                "    USToUniversalDate := valid;\r\n" +
                "end; { function USToUniversalDate }\r\n" +
                "\r\n" +
                "{** 05-31-96 * ValidUniversalDate ****************************************}\r\n" +
                "\r\n" +
                "function ValidUniversalDate(\r\n" +
                "    ChkDate :UniversalDateType\r\n" +
                "    )\r\n" +
                "    :boolean;\r\n" +
                "\r\n" +
                "var\r\n" +
                "    valid                                    :boolean;\r\n" +
                "    mm                                       :integer;\r\n" +
                "    dd                                       :integer;\r\n" +
                "    yy                                       :integer;\r\n" +
                "    i                                        :integer;\r\n" +
                "\r\n" +
                "begin { function ValidUniversalDate }\r\n" +
                "    valid := true;\r\n" +
                "    mm := 0;\r\n" +
                "    yy := 0;\r\n" +
                "    for i := 1 to 12 do\r\n" +
                "        if copy(ChkDate , 3 , 3) = AlphaMonths[i] then\r\n" +
                "            mm := i;\r\n" +
                "    if mm = 0 then\r\n" +
                "        valid := false;\r\n" +
                "    if (valid) then\r\n" +
                "    begin\r\n" +
                "        val(copy(ChkDate , 1 , 2) , dd , i);\r\n" +
                "        if i <> 0 then\r\n" +
                "            valid := false\r\n" +
                "        else if dd < 1 then\r\n" +
                "            valid := false;\r\n" +
                "        if (valid) then\r\n" +
                "        begin\r\n" +
                "            val(copy(ChkDate , 6 , 2) , yy , i);\r\n" +
                "            if i <> 0 then\r\n" +
                "                valid := false\r\n" +
                "            else if yy < 0 then\r\n" +
                "                valid := false;\r\n" +
                "        end; { if }\r\n" +
                "        if (valid) then\r\n" +
                "        begin\r\n" +
                "            case mm of\r\n" +
                "                1 , 3 , 5 , 7 , 8 , 10 , 12\r\n" +
                "                    :\r\n" +
                "                    begin\r\n" +
                "                        if dd > 31 then\r\n" +
                "                            valid := false;\r\n" +
                "                    end;\r\n" +
                "                4 , 6 , 9 , 11\r\n" +
                "                    :\r\n" +
                "                    begin\r\n" +
                "                        if dd > 30 then\r\n" +
                "                            valid := false;\r\n" +
                "                    end;\r\n" +
                "                2\r\n" +
                "                    :\r\n" +
                "                    begin\r\n" +
                "                        if (LeapYear(yy)) then\r\n" +
                "                        begin\r\n" +
                "                            if dd > 29 then\r\n" +
                "                                valid := false;\r\n" +
                "                        end\r\n" +
                "                        else\r\n" +
                "                        begin\r\n" +
                "                            if dd > 28 then\r\n" +
                "                                valid := false;\r\n" +
                "                        end;\r\n" +
                "                    end;\r\n" +
                "            end; { case }\r\n" +
                "        end; { if }\r\n" +
                "    end; { if }\r\n" +
                "    ValidUniversalDate := valid;\r\n" +
                "end; { function ValidUniveralDate }\r\n" +
                "\r\n" +
                "{** 05-01-96 * ValidEagleTime ********************************************}\r\n" +
                "\r\n" +
                "function ValidEagleTime(\r\n" +
                "    ChkTime :EagleTimeType\r\n" +
                "    )\r\n" +
                "    :boolean;\r\n" +
                "\r\n" +
                "var\r\n" +
                "    valid                                    :boolean;\r\n" +
                "\r\n" +
                "begin { function ValidEagleTime }\r\n" +
                "    valid := true;\r\n" +
                "    if (ChkTime[1] < '0') or\r\n" +
                "        (ChkTime[1] > '2') then\r\n" +
                "        valid := false;\r\n" +
                "    if (ChkTime[2] < '0') or\r\n" +
                "        (ChkTime[2] > '9') then\r\n" +
                "        valid := false;\r\n" +
                "    if (ChkTime[1] = '2') and\r\n" +
                "        (ChkTime[2] > '3') then\r\n" +
                "        valid := false;\r\n" +
                "    if (ChkTime[3] < '0') or\r\n" +
                "        (ChkTime[3] > '5') then\r\n" +
                "        valid := false;\r\n" +
                "    if (ChkTime[4] < '0') or\r\n" +
                "        (ChkTime[4] > '9') then\r\n" +
                "        valid := false;\r\n" +
                "    ValidEagleTime := valid;\r\n" +
                "end; { function ValidEagleTime }\r\n" +
                "\r\n" +
                "{** 05-01-96 * ValidUniversalTime ****************************************}\r\n" +
                "\r\n" +
                "function ValidUniversalTime(\r\n" +
                "    ChkTime :UniversalTimeType\r\n" +
                "    )\r\n" +
                "    :boolean;\r\n" +
                "\r\n" +
                "var\r\n" +
                "    valid                                    :boolean;\r\n" +
                "\r\n" +
                "begin { function ValidUniversalTime }\r\n" +
                "    valid := true;\r\n" +
                "    if (ChkTime[1] < '0') or\r\n" +
                "        (ChkTime[1] > '2') then\r\n" +
                "        valid := false;\r\n" +
                "    if (ChkTime[2] < '0') or\r\n" +
                "        (ChkTime[2] > '9') then\r\n" +
                "        valid := false;\r\n" +
                "    if (ChkTime[1] = '2') and\r\n" +
                "        (ChkTime[2] > '3') then\r\n" +
                "        valid := false;\r\n" +
                "    if (ChkTime[3] < '0') or\r\n" +
                "        (ChkTime[3] > '5') then\r\n" +
                "        valid := false;\r\n" +
                "    if (ChkTime[4] < '0') or\r\n" +
                "        (ChkTime[4] > '9') then\r\n" +
                "        valid := false;\r\n" +
                "    if (ChkTime[5] < '0') or\r\n" +
                "        (ChkTime[5] > '5') then\r\n" +
                "        valid := false;\r\n" +
                "    if (ChkTime[6] < '0') or\r\n" +
                "        (ChkTime[6] > '9') then\r\n" +
                "        valid := false;\r\n" +
                "    ValidUniversalTime := valid;\r\n" +
                "end; { function ValidUniversalTime }\r\n" +
                "\r\n" +
                "{** GetUniversalDateTime * 07-30-98 **************************************}\r\n" +
                "\r\n" +
                "function GetUniversalDateTime\r\n" +
                "    (\r\n" +
                "    var CurrDate :UniversalDateType;\r\n" +
                "    var CurrTime :UniversalTimeType;\r\n" +
                "    var wSec100 :word\r\n" +
                "    )\r\n" +
                "    :boolean;\r\n" +
                "\r\n" +
                "{ FILENAME:    datetime.pas\r\n" +
                "\r\n" +
                "  DESCRIPTION: GetUniversalDateTime returns the current DDMMMYY date,\r\n" +
                "               HHMMMSS time and hundreths of a second.\r\n" +
                "\r\n" +
                "  PARAMETERS:  CurrDate            Current date in DDMMMYY format\r\n" +
                "               CurrTime            Current time in HHMMSS format\r\n" +
                "               wSec100             Current hundredths of a second (0..99)\r\n" +
                "\r\n" +
                "  RETURNS:     TRUE                Success\r\n" +
                "               FALSE               Failure\r\n" +
                "\r\n" +
                "  LOCAL:       wHour               Hour (0..23)\r\n" +
                "               wMinute             Minute (0..59)\r\n" +
                "               wSecond             Second (0..59)\r\n" +
                "               wSec100             Hundredth of second (0..99)\r\n" +
                "               Hour                Hour ('000'..'023');\r\n" +
                "               Minute              Minute ('000'..'059');\r\n" +
                "               Second              Second ('000'..'059');\r\n" +
                "\r\n" +
                "--------------------------------------------------------------------------}\r\n" +
                "\r\n" +
                "var\r\n" +
                "    CurrentDateTime                          :TDateTime;\r\n" +
                "    wHour                                    :word;\r\n" +
                "    wMinute                                  :word;\r\n" +
                "    wSecond                                  :word;\r\n" +
                "    wSec1000                                 :word;\r\n" +
                "    Hour                                     :string[3];\r\n" +
                "    Minute                                   :string[3];\r\n" +
                "    Second                                   :string[3];\r\n" +
                "\r\n" +
                "begin { function GetUniversalDateTime }\r\n" +
                "\r\n" +
                "    CurrentDateTime := GetUTCDateTime;\r\n" +
                "    CurrDate := ConvertDateTimeToUniversalDate(CurrentDateTime);\r\n" +
                "\r\n" +
                "    decodetime(CurrentDateTime - Trunc(CurrentDateTime) , wHour , wMinute , wSecond , wSec1000);\r\n" +
                "    str((wHour + 100) :3 , Hour);\r\n" +
                "    str((wMinute + 100) :3 , Minute);\r\n" +
                "    str((wSecond + 100) :3 , Second);\r\n" +
                "    CurrTime := copy(Hour , 2 , 2) + copy(Minute , 2 , 2) +\r\n" +
                "        copy(Second , 2 , 2);\r\n" +
                "\r\n" +
                "    if (ValidUniversalDate(CurrDate)) and\r\n" +
                "        (ValidUniversalTime(CurrTime)) then\r\n" +
                "        GetUniversalDateTime := true\r\n" +
                "    else\r\n" +
                "        GetUniversalDateTime := false;\r\n" +
                "\r\n" +
                "end; { function GetUniversalDateTime }\r\n" +
                "\r\n" +
                "{** GetCurrentDateTime * 07-30-98 ****************************************}\r\n" +
                "\r\n" +
                "procedure GetCurrentDateTime\r\n" +
                "    (\r\n" +
                "    var CurrDate :UniversalDateType;\r\n" +
                "    var wCurrHour :word;\r\n" +
                "    var wCurrMinute :word;\r\n" +
                "    var wCurrSecond :word;\r\n" +
                "    var wCurrSec100 :word\r\n" +
                "    );\r\n" +
                "\r\n" +
                "{ FILENAME:    datetime.pas\r\n" +
                "\r\n" +
                "  DESCRIPTION: GetCurrentDateTime returns the current DDMMMYY date,\r\n" +
                "               hours, minutes, seconds and hundredths of seconds.\r\n" +
                "\r\n" +
                "  PARAMETERS:  CurrDate            Current date in DDMMMYY format\r\n" +
                "               wCurrHour           Current hour (0..23)\r\n" +
                "               wCurrMinute         Current minute (0..59)\r\n" +
                "               wCurrSecond         Current second (0..59)\r\n" +
                "               wCurrSec100         Current hundredth of second (0..99)\r\n" +
                "\r\n" +
                "  RETURNS:     None                ...\r\n" +
                "\r\n" +
                "\r\n" +
                "--------------------------------------------------------------------------}\r\n" +
                "\r\n" +
                "var\r\n" +
                "    CurrentDateTime                          :TDateTime;\r\n" +
                "    wSec1000                                 :word;\r\n" +
                "\r\n" +
                "begin { procedure GetCurrentDateTime }\r\n" +
                "\r\n" +
                "    CurrentDateTime := GetUTCDateTime;\r\n" +
                "    decodetime(CurrentDateTime - Trunc(CurrentDateTime) ,\r\n" +
                "        wCurrHour , wCurrMinute , wCurrSecond , wSec1000);\r\n" +
                "\r\n" +
                "    wCurrSec100 := wSec1000 div 10;\r\n" +
                "    CurrDate := ConvertDateTimeToUniversalDate(CurrentDateTime);\r\n" +
                "end; { procedure GetCurrentDateTime }\r\n" +
                "\r\n" +
                "{** 04-30-96 * ConvertUniversalTime **************************************}\r\n" +
                "\r\n" +
                "function ConvertUniversalTime\r\n" +
                "    (\r\n" +
                "    UniversalTime :UniversalTimeType;\r\n" +
                "    var wHour :word;\r\n" +
                "    var wMinute :word;\r\n" +
                "    var wSecond :word\r\n" +
                "    )\r\n" +
                "    :boolean;\r\n" +
                "\r\n" +
                "{ FILENAME:    datetime.pas\r\n" +
                "\r\n" +
                "  DESCRIPTION: ConvertUniversalTime converts a universal time HHMMSS to\r\n" +
                "               the component word hour, minute and second.\r\n" +
                "\r\n" +
                "  PARAMETERS:  UniversalTime       UniversalTime (HHMMSS)\r\n" +
                "               wHour               Hour (0..23)\r\n" +
                "               wMinute             Minute (0..59)\r\n" +
                "               wSecond             Second (0..59)\r\n" +
                "\r\n" +
                "  RETURNS:     TRUE                Routine succeeded\r\n" +
                "               FALSE               Routine failed (invalid time)\r\n" +
                "\r\n" +
                "  LOCAL:       err                 Hour ( hour: 0-23 )\r\n" +
                "               valid               Minute ( minute: 0-59 )\r\n" +
                "\r\n" +
                "--------------------------------------------------------------------------}\r\n" +
                "\r\n" +
                "var\r\n" +
                "    err                                      :integer;\r\n" +
                "\r\n" +
                "begin { function ConvertUniversalTime }\r\n" +
                "    Result := true;\r\n" +
                "    val(copy(UniversalTime , 1 , 2) , wHour , err);\r\n" +
                "    if (err <> 0) then\r\n" +
                "        Result := false;\r\n" +
                "    val(copy(UniversalTime , 3 , 2) , wMinute , err);\r\n" +
                "    if (err <> 0) then\r\n" +
                "        Result := false;\r\n" +
                "    val(copy(UniversalTime , 5 , 2) , wSecond , err);\r\n" +
                "    if (err <> 0) then\r\n" +
                "        Result := false;\r\n" +
                "end; { function ConvertUniversalTime }\r\n" +
                "\r\n" +
                "{** UnivTimeDiff * 05-01-96 **********************************************}\r\n" +
                "\r\n" +
                "function UnivTimeDiff\r\n" +
                "    (\r\n" +
                "    var ErrorRet :integer;\r\n" +
                "    var Seconds :longint;\r\n" +
                "    FirstDate :UniversalDateType;\r\n" +
                "    FirstTime :UniversalTimeType;\r\n" +
                "    SecondDate :UniversalDateType;\r\n" +
                "    SecondTime :UniversalTimeType\r\n" +
                "    )\r\n" +
                "    :boolean;\r\n" +
                "\r\n" +
                "{ FILENAME:    datetime.pas\r\n" +
                "\r\n" +
                "  DESCRIPTION: UnivTimeDiff returns false if any of the passed date/time\r\n" +
                "               values are blank.  If all values are not blank the routine\r\n" +
                "               calls standard routines to determine the time (in seconds)\r\n" +
                "               between the two passed date/times.  By convention the\r\n" +
                "               second time should be after the first time.\r\n" +
                "\r\n" +
                "               NOTES: 1. This routine uses library routines which work only\r\n" +
                "                         in an assumed date range of 1950 to 2049.\r\n" +
                "\r\n" +
                "                      2. The month in the the universal date (DDMMMYY) must\r\n" +
                "                         be upper case.\r\n" +
                "\r\n" +
                "                      3. The number of seconds in 68 years from the base\r\n" +
                "                         date is too large to fit in a long integer - so\r\n" +
                "                         this routine will only work on dates from 1950 to\r\n" +
                "                         2018 (1950+68).\r\n" +
                "\r\n" +
                "  PARAMETERS:  ErrorRet            Error return code - unsed at this time\r\n" +
                "               Seconds             Seconds difference between date/times\r\n" +
                "               FirstDate           First date (DDMMMYY)\r\n" +
                "               FirstTime           First time (HHMMSS)\r\n" +
                "               SecondDate          Second date (DDMMMYY)\r\n" +
                "               SecondTime          Second time (HHMMSS)\r\n" +
                "\r\n" +
                "  RETURNS:     TRUE                Routine succeeded\r\n" +
                "               FALSE               Routine failed (invalid date or time)\r\n" +
                "\r\n" +
                "  LOCAL:       valid               Local function success\r\n" +
                "               wHour               Hour ( hour: 0-23 )\r\n" +
                "               wMinute             Minute ( minute: 0-59 )\r\n" +
                "               wSecond             Second ( second: 0-59 )\r\n" +
                "               Days                Number of days between dates\r\n" +
                "               FirstDayAD          Days after death to first date\r\n" +
                "               SecondDayAD         Days after death to second date\r\n" +
                "               NumericFirstDate    First date in numeric format\r\n" +
                "               NumericSecondDate   Second date in numeric format\r\n" +
                "\r\n" +
                "--------------------------------------------------------------------------}\r\n" +
                "\r\n" +
                "var\r\n" +
                "    valid                                    :boolean;\r\n" +
                "    wHour                                    :word;\r\n" +
                "    wMinute                                  :word;\r\n" +
                "    wSecond                                  :word;\r\n" +
                "    Days                                     :longint;\r\n" +
                "    FirstDayAD                               :longint;\r\n" +
                "    SecondDayAD                              :longint;\r\n" +
                "    NumericFirstDate                         :NumericDateType;\r\n" +
                "    NumericSecondDate                        :NumericDateType;\r\n" +
                "\r\n" +
                "begin { function UnivTimeDiff }\r\n" +
                "    valid := false;\r\n" +
                "    if (ValidUniversalDate(FirstDate) and\r\n" +
                "        ValidUniversalDate(SecondDate) and\r\n" +
                "        ValidUniversalTime(FirstTime) and\r\n" +
                "        ValidUniversalTime(SecondTime)) then\r\n" +
                "    begin\r\n" +
                "        NumericFirstDate := NumericDate(FirstDate);\r\n" +
                "        NumericSecondDate := NumericDate(SecondDate);\r\n" +
                "        FirstDayAD := DayAD(NumericFirstDate);\r\n" +
                "        SecondDayAD := DayAD(NumericSecondDate);\r\n" +
                "        Days := SecondDayAD - FirstDayAD;\r\n" +
                "        Seconds := Days * SEC_PER_DAY;\r\n" +
                "\r\n" +
                "        if (ConvertUniversalTime(SecondTime , wHour , wMinute , wSecond)) then\r\n" +
                "        begin\r\n" +
                "            Seconds := Seconds + (longint(wHour) * 3600) + (wMinute * 60) +\r\n" +
                "                wSecond;\r\n" +
                "            if (ConvertUniversalTime(FirstTime , wHour , wMinute , wSecond)) then\r\n" +
                "            begin\r\n" +
                "                Seconds := Seconds - (longint(wHour) * 3600) - (wMinute * 60) -\r\n" +
                "                    wSecond;\r\n" +
                "                valid := true;\r\n" +
                "            end; { if }\r\n" +
                "        end; { if }\r\n" +
                "    end { if }\r\n" +
                "    else\r\n" +
                "        valid := false;\r\n" +
                "    UnivTimeDiff := valid;\r\n" +
                "end; { function UnivTimeDiff }\r\n" +
                "\r\n" +
                "{** 01-15-97 * ConvertEagleTime ******************************************}\r\n" +
                "\r\n" +
                "function ConvertEagleTime\r\n" +
                "    (\r\n" +
                "    EagleTime :EagleTimeType;\r\n" +
                "    var wHour :word;\r\n" +
                "    var wMinute :word\r\n" +
                "    )\r\n" +
                "    :boolean;\r\n" +
                "\r\n" +
                "{ FILENAME:    datetime.pas\r\n" +
                "\r\n" +
                "  DESCRIPTION: ConvertEagleTime converts an Eagle time HHMM to the\r\n" +
                "               the component word hour and minute.\r\n" +
                "\r\n" +
                "  PARAMETERS:  EagleTime           UniversalTime (HHMM)\r\n" +
                "               wHour               Hour (0..23)\r\n" +
                "               wMinute             Minute (0..59)\r\n" +
                "\r\n" +
                "  RETURNS:     TRUE                Routine succeeded\r\n" +
                "               FALSE               Routine failed (invalid time)\r\n" +
                "\r\n" +
                "  LOCAL:       err                 Hour ( hour: 0-23 )\r\n" +
                "               valid               Minute ( minute: 0-59 )\r\n" +
                "\r\n" +
                "--------------------------------------------------------------------------}\r\n" +
                "\r\n" +
                "var\r\n" +
                "    err                                      :integer;\r\n" +
                "\r\n" +
                "begin { function ConvertEagleTime }\r\n" +
                "    Result := true;\r\n" +
                "    Val(Copy(EagleTime , 1 , 2) , wHour , err);\r\n" +
                "    if (err <> 0) then\r\n" +
                "        Result := false;\r\n" +
                "    Val(Copy(EagleTime , 3 , 2) , wMinute , err);\r\n" +
                "    if (err <> 0) then\r\n" +
                "        Result := false;\r\n" +
                "end; { function ConvertEagleTime }\r\n" +
                "\r\n" +
                "{** TimeDiff * 05-01-96 **************************************************}\r\n" +
                "\r\n" +
                "function TimeDiff\r\n" +
                "    (\r\n" +
                "    var ErrorRet :integer;\r\n" +
                "    var Minutes :longint;\r\n" +
                "    FirstDate :UniversalDateType;\r\n" +
                "    FirstTime :EagleTimeType;\r\n" +
                "    SecondDate :UniversalDateType;\r\n" +
                "    SecondTime :EagleTimeType\r\n" +
                "    )\r\n" +
                "    :boolean;\r\n" +
                "\r\n" +
                "{ FILENAME:    datetime.pas\r\n" +
                "\r\n" +
                "  DESCRIPTION: TimeDiff returns false if any of the passed date/time values\r\n" +
                "               are blank.  If all values are not blank the routine calls\r\n" +
                "               standard routines to determine the time (in seconds) between\r\n" +
                "               the two passed date/times.  By convention the second time\r\n" +
                "               should be after the first time.\r\n" +
                "\r\n" +
                "               NOTES: 1. This routine uses library routines which work only\r\n" +
                "                         in an assumed date range of 1950 to 2049.\r\n" +
                "\r\n" +
                "                      2. The month in the the universal date (DDMMMYY) must\r\n" +
                "                         be upper case.\r\n" +
                "\r\n" +
                "  PARAMETERS:  ErrorRet            Error return code - unsed at this time\r\n" +
                "               Minutes             Minutes difference between date/times\r\n" +
                "               FirstDate           First date (DDMMMYY)\r\n" +
                "               FirstTime           First time (HHMM)\r\n" +
                "               SecondDate          Second date (DDMMMYY)\r\n" +
                "               SecondTime          Second time (HHMM)\r\n" +
                "\r\n" +
                "  RETURNS:     TRUE                Routine succeeded\r\n" +
                "               FALSE               Routine failed (invalid date or time)\r\n" +
                "\r\n" +
                "  LOCAL:       valid               Local function success\r\n" +
                "               wHour               Hour ( hour: 0-23 )\r\n" +
                "               wMinute             Minute ( minute: 0-59 )\r\n" +
                "               Days                Number of days between dates\r\n" +
                "               FirstDayAD          Days after death to first date\r\n" +
                "               SecondDayAD         Days after death to second date\r\n" +
                "               NumericFirstDate    First date in numeric format\r\n" +
                "               NumericSecondDate   Second date in numeric format\r\n" +
                "\r\n" +
                "--------------------------------------------------------------------------}\r\n" +
                "\r\n" +
                "var\r\n" +
                "    valid                                    :boolean;\r\n" +
                "    wHour                                    :word;\r\n" +
                "    wMinute                                  :word;\r\n" +
                "    Days                                     :longint;\r\n" +
                "    FirstDayAD                               :longint;\r\n" +
                "    SecondDayAD                              :longint;\r\n" +
                "    NumericFirstDate                         :NumericDateType;\r\n" +
                "    NumericSecondDate                        :NumericDateType;\r\n" +
                "\r\n" +
                "begin { function TimeDiff }\r\n" +
                "    valid := false;\r\n" +
                "    if (ValidUniversalDate(FirstDate) and\r\n" +
                "        ValidUniversalDate(SecondDate) and\r\n" +
                "        ValidEagleTime(FirstTime) and\r\n" +
                "        ValidEagleTime(SecondTime)) then\r\n" +
                "    begin\r\n" +
                "        NumericFirstDate := NumericDate(FirstDate);\r\n" +
                "        NumericSecondDate := NumericDate(SecondDate);\r\n" +
                "        FirstDayAD := DayAD(NumericFirstDate);\r\n" +
                "        SecondDayAD := DayAD(NumericSecondDate);\r\n" +
                "        Days := SecondDayAD - FirstDayAD;\r\n" +
                "        Minutes := Days * MIN_PER_DAY;\r\n" +
                "\r\n" +
                "        if (ConvertEagleTime(SecondTime , wHour , wMinute)) then\r\n" +
                "        begin\r\n" +
                "            Minutes := Minutes + (wHour * 60) + wMinute;\r\n" +
                "            if (ConvertEagleTime(FirstTime , wHour , wMinute)) then\r\n" +
                "            begin\r\n" +
                "                Minutes := Minutes - (wHour * 60) - wMinute;\r\n" +
                "                valid := true;\r\n" +
                "            end; { if }\r\n" +
                "        end; { if }\r\n" +
                "\r\n" +
                "    end { if }\r\n" +
                "    else\r\n" +
                "        valid := false;\r\n" +
                "    TimeDiff := valid;\r\n" +
                "end; { function TimeDiff }\r\n" +
                "\r\n" +
                "{** SecSince01Jan1970 * 04-20-96 *****************************************}\r\n" +
                "\r\n" +
                "procedure SecSince01Jan1970\r\n" +
                "    (\r\n" +
                "    var Seconds :longint;\r\n" +
                "    var Sec1000 :longint\r\n" +
                "    );\r\n" +
                "\r\n" +
                "{ FILENAME:    datetime.pas\r\n" +
                "\r\n" +
                "  DESCRIPTION: SecSince01Jan1970 returns the number of seconds elapsed\r\n" +
                "               since Jan 01, 1970 at Midnight UTC.  This routine was\r\n" +
                "               created to mimic the TIME function standard to most C\r\n" +
                "               implementation\r\n" +
                "\r\n" +
                "uses\r\n" +
                "  SYSUTILS,\r\n" +
                "  Classes;s.  This function assumes that the\r\n" +
                "               workstations local, system-time is set for UTC.\r\n" +
                "\r\n" +
                "  PARAMETERS:  Seconds             Seconds elapsed since Jan 1, 1970.\r\n" +
                "               Sec1000             Local system time in milliseconds to\r\n" +
                "                                   the nearest 10 milliseconds\r\n" +
                "\r\n" +
                "  RETURNS:     None\r\n" +
                "\r\n" +
                "  LOCAL:       wCurrHour           Local system time ( hour: 0-23 )\r\n" +
                "               wCurrMinute         Local system time ( minute: 0-59 )\r\n" +
                "               wCurrSecond         Local system time ( second: 0-59 )\r\n" +
                "               wCurrSec100         Local system time ( hundredths of\r\n" +
                "                                   a second: 0-99 )\r\n" +
                "               Days                Days since Jan 1, 1970\r\n" +
                "               BaseDateAD          Days after death to Jan 1, 1970\r\n" +
                "               TodayAD             Days after death to today\r\n" +
                "               Today               Today's date in \"universal date\" format\r\n" +
                "               NumericBaseDate     Base date Jan 1, 1970 in numeric format\r\n" +
                "               NumericToday        Today's date in numeric format\r\n" +
                "\r\n" +
                "--------------------------------------------------------------------------}\r\n" +
                "\r\n" +
                "var\r\n" +
                "    wCurrHour                                :word;\r\n" +
                "    wCurrMinute                              :word;\r\n" +
                "    wCurrSecond                              :word;\r\n" +
                "    wCurrSec100                              :word;\r\n" +
                "    Days                                     :longint;\r\n" +
                "    BaseDateAD                               :longint;\r\n" +
                "    TodayAD                                  :longint;\r\n" +
                "    CurrDate                                 :UniversalDateType;\r\n" +
                "    NumericBaseDate                          :NumericDateType;\r\n" +
                "    NumericToday                             :NumericDateType;\r\n" +
                "\r\n" +
                "begin { procedure SecSince01Jan1970 }\r\n" +
                "\r\n" +
                "    GetCurrentDateTime(CurrDate , wCurrHour , wCurrMinute , wCurrSecond ,\r\n" +
                "        wCurrSec100);\r\n" +
                "    NumericToday := NumericDate(CurrDate);\r\n" +
                "    TodayAD := DayAD(NumericToday);\r\n" +
                "    NumericBaseDate := chr(19) + chr(70) + chr(1) + chr(1);\r\n" +
                "    BaseDateAD := DayAD(NumericBaseDate);\r\n" +
                "    Days := TodayAD - BaseDateAD;\r\n" +
                "    Seconds := Days * SEC_PER_DAY;\r\n" +
                "    Seconds := Seconds + (Longint(wCurrHour) * 3600) +\r\n" +
                "        (wCurrMinute * 60) + wCurrSecond;\r\n" +
                "    Sec1000 := wCurrSec100 * 10;\r\n" +
                "\r\n" +
                "end; { procedure SecSince01Jan1970 }\r\n" +
                "\r\n" +
                "{** 03-22-97 * CheckDDMMMYYDate ******************************************}\r\n" +
                "\r\n" +
                "function CheckDDMMMYYDate\r\n" +
                "    (\r\n" +
                "    CheckDate :UniversalDateType\r\n" +
                "    )\r\n" +
                "    :boolean;\r\n" +
                "\r\n" +
                "{ FILENAME:    datetime.pas\r\n" +
                "\r\n" +
                "  DESCRIPTION: CheckDDMMMYYDate determines if a date in the format\r\n" +
                "               DDMMMYY is valid with all values in appropriate ranges.\r\n" +
                "\r\n" +
                "  PARAMETERS:  CheckDate           Date to check - with format DDMMMYY\r\n" +
                "\r\n" +
                "  RETURNS:     Function            True for valid date, false for an\r\n" +
                "                                   invalid date\r\n" +
                "\r\n" +
                "  LOCAL:       MaxStep             Number of step in process\r\n" +
                "               iStep               Step in progress\r\n" +
                "               Code                Return code: string to int conversion\r\n" +
                "               Days31              Months with 31 days\r\n" +
                "               Days30              Months with 30 days\r\n" +
                "               Days29              Months with 28 or 29 days\r\n" +
                "               iDay                Integer day\r\n" +
                "               iMonth              Integer month\r\n" +
                "               iYear               Integer year\r\n" +
                "               strMonth            Date month - 3 char string format\r\n" +
                "               enumMonth           Date month - enumerated: 0-11\r\n" +
                "               IsValid             Local flag for date validity\r\n" +
                "\r\n" +
                "--------------------------------------------------------------------------}\r\n" +
                "\r\n" +
                "type\r\n" +
                "    Months = (JAN , FEB , MAR , APR , MAY , JUN ,\r\n" +
                "        JUL , AUG , SEP , OCT , NOV , DEC);\r\n" +
                "\r\n" +
                "    MonthSet = set of Months;\r\n" +
                "\r\n" +
                "const\r\n" +
                "    MaxStep                                  = 6;\r\n" +
                "\r\n" +
                "    Days31                                   :MonthSet = [JAN , MAR , MAY , JUL , AUG , OCT , DEC];\r\n" +
                "    Days30                                   :MonthSet = [APR , JUN , SEP , NOV];\r\n" +
                "    Days29                                   :MonthSet = [FEB];\r\n" +
                "\r\n" +
                "var\r\n" +
                "    i                                        :integer;\r\n" +
                "    iStep                                    :integer;\r\n" +
                "    Code                                     :integer;\r\n" +
                "    iDay                                     :integer;\r\n" +
                "    iMonth                                   :integer;\r\n" +
                "    iYear                                    :integer;\r\n" +
                "    strMonth                                 :string[3];\r\n" +
                "    enumMonth                                :Months;\r\n" +
                "    IsValid                                  :boolean;\r\n" +
                "\r\n" +
                "begin { function CheckDDMMMYYDate }\r\n" +
                "    IsValid := true;\r\n" +
                "    iStep := 1;\r\n" +
                "    iDay := 0;\r\n" +
                "    iYear := 0;\r\n" +
                "    enumMonth := Months(0); { JAN }\r\n" +
                "    while (iStep <= MaxStep) and (IsValid) do\r\n" +
                "    begin\r\n" +
                "        case iStep of\r\n" +
                "            1 :\r\n" +
                "                begin\r\n" +
                "                    iMonth := 0;\r\n" +
                "                    strMonth := copy(CheckDate , 3 , 3);\r\n" +
                "                    for i := 1 to 12 do\r\n" +
                "                        if (strMonth = AlphaMonths[i]) then\r\n" +
                "                            iMonth := i;\r\n" +
                "                    if (iMonth > 0) then\r\n" +
                "                        enumMonth := Months(iMonth - 1)\r\n" +
                "                    else\r\n" +
                "                        IsValid := false;\r\n" +
                "                end;\r\n" +
                "            2 :\r\n" +
                "                begin\r\n" +
                "                    val(copy(CheckDate , 1 , 2) , iDay , Code);\r\n" +
                "                    if (Code <> 0) or (iDay < 1) then\r\n" +
                "                        IsValid := false;\r\n" +
                "                end;\r\n" +
                "            3 :\r\n" +
                "                begin\r\n" +
                "                    val(copy(CheckDate , 6 , 2) , iYear , Code);\r\n" +
                "                    if (Code <> 0) or (iYear < 0) then\r\n" +
                "                        IsValid := false;\r\n" +
                "                end;\r\n" +
                "            4 :if (enumMonth in Days31) and (iDay > 31) then\r\n" +
                "                    IsValid := false;\r\n" +
                "            5 :if (enumMonth in Days30) and (iDay > 30) then\r\n" +
                "                    IsValid := false;\r\n" +
                "            6 :if (enumMonth in Days29) then\r\n" +
                "                begin\r\n" +
                "                    if (LeapYear(iYear)) then\r\n" +
                "                    begin\r\n" +
                "                        if (iDay > 29) then\r\n" +
                "                            IsValid := false;\r\n" +
                "                    end\r\n" +
                "                    else if (iDay > 28) then\r\n" +
                "                        IsValid := false;\r\n" +
                "                end; // if\r\n" +
                "        end; { case }\r\n" +
                "        Inc(iStep);\r\n" +
                "    end; { while }\r\n" +
                "    CheckDDMMMYYDate := IsValid;\r\n" +
                "end; { function CheckDDMMMYYDate }\r\n" +
                "\r\n" +
                "{** DivideUniversalDate * 02-17-95 ***************************************}\r\n" +
                "\r\n" +
                "function DivideUniversalDate\r\n" +
                "    (\r\n" +
                "    var UniversalDate :UniversalDateType;\r\n" +
                "    var SubYear :integer;\r\n" +
                "    var SubMonth :integer;\r\n" +
                "    var SubDay :integer\r\n" +
                "    )\r\n" +
                "    :boolean;\r\n" +
                "\r\n" +
                "{ FILENAME:     datetime.pas\r\n" +
                "\r\n" +
                "  DESCRIPTION: DivideUniversalDate converts a date in the format 05JAN95 to\r\n" +
                "               and integer components - in this case 95, 1, and 5.  If the\r\n" +
                "               date does not include one of the three components the value\r\n" +
                "               will default to -1.  The routine cannot identify invalid\r\n" +
                "               dates where the number of days exceeds a month maximum.\r\n" +
                "\r\n" +
                "  PARAMETERS:  UniversalDate       Date in the format DDMMMYY\r\n" +
                "               SubYear             0 to 99 or -1 for error\r\n" +
                "               SubMonth            1 to 12 or -1 for error\r\n" +
                "               SubDay              1 to 99 or -1 for error\r\n" +
                "\r\n" +
                "  RETURNS:     FUNCTION            Returns true if the date is successfully\r\n" +
                "                                   divided or false if a DOS error occurs\r\n" +
                "                                   or a value is \"grossly\" out of range\r\n" +
                "\r\n" +
                "  LOCAL:       SubDayStr           Local string used to identify day\r\n" +
                "               SubMonthStr         Local string used to identify month\r\n" +
                "               SubYearStr          Local string used to identify year\r\n" +
                "               Code                Error return for string to integer\r\n" +
                "                                   conversion.  Any non-zero value\r\n" +
                "                                   indicates an error\r\n" +
                "               IsValid             Local flag for date validity\r\n" +
                "\r\n" +
                "--------------------------------------------------------------------------}\r\n" +
                "\r\n" +
                "var\r\n" +
                "    SubDayStr                                :string[2];\r\n" +
                "    SubMonthStr                              :string[3];\r\n" +
                "    SubYearStr                               :string[2];\r\n" +
                "    Code                                     :integer;\r\n" +
                "    IsValid                                  :boolean;\r\n" +
                "\r\n" +
                "begin { function DivideUniversalDate }\r\n" +
                "    IsValid := true;\r\n" +
                "    SubYearStr := copy(UniversalDate , 6 , 2);\r\n" +
                "    if (SubYearStr = '  ') then\r\n" +
                "        SubYear := -1\r\n" +
                "    else\r\n" +
                "    begin\r\n" +
                "        val(SubYearStr , SubYear , Code);\r\n" +
                "        if (Code <> 0) or (SubYear < 0) then\r\n" +
                "            IsValid := false;\r\n" +
                "    end; { else }\r\n" +
                "    SubMonthStr := copy(UniversalDate , 3 , 3);\r\n" +
                "    if (SubMonthStr = '   ') then\r\n" +
                "        SubMonth := -1\r\n" +
                "    else if (SubMonthStr = 'JAN') then\r\n" +
                "        SubMonth := 1\r\n" +
                "    else if (SubMonthStr = 'FEB') then\r\n" +
                "        SubMonth := 2\r\n" +
                "    else if (SubMonthStr = 'MAR') then\r\n" +
                "        SubMonth := 3\r\n" +
                "    else if (SubMonthStr = 'APR') then\r\n" +
                "        SubMonth := 4\r\n" +
                "    else if (SubMonthStr = 'MAY') then\r\n" +
                "        SubMonth := 5\r\n" +
                "    else if (SubMonthStr = 'JUN') then\r\n" +
                "        SubMonth := 6\r\n" +
                "    else if (SubMonthStr = 'JUL') then\r\n" +
                "        SubMonth := 7\r\n" +
                "    else if (SubMonthStr = 'AUG') then\r\n" +
                "        SubMonth := 8\r\n" +
                "    else if (SubMonthStr = 'SEP') then\r\n" +
                "        SubMonth := 9\r\n" +
                "    else if (SubMonthStr = 'OCT') then\r\n" +
                "        SubMonth := 10\r\n" +
                "    else if (SubMonthStr = 'NOV') then\r\n" +
                "        SubMonth := 11\r\n" +
                "    else if (SubMonthStr = 'DEC') then\r\n" +
                "        SubMonth := 12\r\n" +
                "    else\r\n" +
                "        IsValid := false;\r\n" +
                "    SubDayStr := copy(UniversalDate , 1 , 2);\r\n" +
                "    if (SubDayStr = '  ') then\r\n" +
                "        IsValid := false\r\n" +
                "    else\r\n" +
                "    begin\r\n" +
                "        val(SubDayStr , SubDay , Code);\r\n" +
                "        if (Code <> 0) or (SubDay < 1) or (SubDay > 31) then\r\n" +
                "            IsValid := false;\r\n" +
                "    end; { else }\r\n" +
                "    DivideUniversalDate := IsValid;\r\n" +
                "end; { function DivideUniversalDate }\r\n" +
                "\r\n" +
                "{** BuildUniversalDate * 02-17-95 ****************************************}\r\n" +
                "\r\n" +
                "function BuildUniversalDate\r\n" +
                "    (\r\n" +
                "    var UniversalDate :UniversalDateType;\r\n" +
                "    var SubYear :integer;\r\n" +
                "    var SubMonth :integer;\r\n" +
                "    var SubDay :integer\r\n" +
                "    )\r\n" +
                "    :boolean;\r\n" +
                "\r\n" +
                "{ FILENAME:     datetime.pas\r\n" +
                "\r\n" +
                "  DESCRIPTION: BuildUniversalDate converts a date in the form of three\r\n" +
                "               integer components (95, 1, 5) into the date '05JAN95'.\r\n" +
                "               The procedure does not perform any error checking and\r\n" +
                "               expects all three passed integer fields to be included\r\n" +
                "               and valid.\r\n" +
                "\r\n" +
                "  PARAMETERS:  UniversalDate       Date in the format DDMMMYY - '05JAN95'\r\n" +
                "               SubYear             0 to 99 - no check performed\r\n" +
                "               SubMonth            1 to 12 - no check performed\r\n" +
                "               SubDay              1 to 31 - no check performed\r\n" +
                "\r\n" +
                "  RETURNS:     FUNCTION            True for valid date, false for invalid\r\n" +
                "                                   date\r\n" +
                "\r\n" +
                "  LOCAL:       SubYearStr          String version of year - '95'\r\n" +
                "               SubMonthStr         String version of month - 'JAN'\r\n" +
                "               SubDayStr           String version of day - '05'\r\n" +
                "\r\n" +
                "--------------------------------------------------------------------------}\r\n" +
                "\r\n" +
                "var\r\n" +
                "    SubYearStr                               :string;\r\n" +
                "    SubMonthStr                              :string;\r\n" +
                "    SubDayStr                                :string;\r\n" +
                "\r\n" +
                "begin { function BuildUniversalDate }\r\n" +
                "    str(SubYear :2 , SubYearStr);\r\n" +
                "    if (SubYearStr[1] = ' ') then\r\n" +
                "        SubYearStr[1] := '0';\r\n" +
                "    case (SubMonth) of\r\n" +
                "        1 :SubMonthStr := 'JAN';\r\n" +
                "        2 :SubMonthStr := 'FEB';\r\n" +
                "        3 :SubMonthStr := 'MAR';\r\n" +
                "        4 :SubMonthStr := 'APR';\r\n" +
                "        5 :SubMonthStr := 'MAY';\r\n" +
                "        6 :SubMonthStr := 'JUN';\r\n" +
                "        7 :SubMonthStr := 'JUL';\r\n" +
                "        8 :SubMonthStr := 'AUG';\r\n" +
                "        9 :SubMonthStr := 'SEP';\r\n" +
                "        10 :SubMonthStr := 'OCT';\r\n" +
                "        11 :SubMonthStr := 'NOV';\r\n" +
                "        12 :SubMonthStr := 'DEC';\r\n" +
                "    end; { case }\r\n" +
                "    str(SubDay :2 , SubDayStr);\r\n" +
                "    if (SubDayStr[1] = ' ') then\r\n" +
                "        SubDayStr[1] := '0';\r\n" +
                "    UniversalDate := SubDayStr + SubMonthStr + SubYearStr;\r\n" +
                "    BuildUniversalDate := CheckDDMMMYYDate(UniversalDate);\r\n" +
                "end; { function BuildUniversalDate }\r\n" +
                "\r\n" +
                "{** 11-04-98 * CompleteDate **********************************************}\r\n" +
                "\r\n" +
                "function CompleteDate\r\n" +
                "    (\r\n" +
                "    var ReferenceDate :UniversalDateType;\r\n" +
                "    var PartialDate :UniversalDateType\r\n" +
                "    )\r\n" +
                "    :boolean;\r\n" +
                "\r\n" +
                "{ FILENAME:     datetime.pas\r\n" +
                "\r\n" +
                "  DESCRIPTION: CompleteDate compares information from a partial date to\r\n" +
                "               a reference date which is expected to be near the partial\r\n" +
                "               date.  The procedure then makes a best guess as the\r\n" +
                "               complete form of the partial date.  For example, a partial\r\n" +
                "               date of '26     ' with a reference date of '05JAN95' will\r\n" +
                "               produce a date of 26DEC94.\r\n" +
                "\r\n" +
                "               Notes: The reference date must be a complete valid date.\r\n" +
                "                      The partial date must include at least a day.\r\n" +
                "\r\n" +
                "  PARAMETERS:  ReferenceDate       Date in the format DDMMMYY - '05JAN95'\r\n" +
                "               PartialDate         Partial date, format DDMMMYY - '04JAN  '\r\n" +
                "\r\n" +
                "  RETURNS:     FUNCTION            The function returns true if a\r\n" +
                "                                   valid (though not necessarily\r\n" +
                "                                   correct) date is created from\r\n" +
                "                                   the partial and reference dates.\r\n" +
                "\r\n" +
                "  LOCAL:       ReferenceYear       Integer year for reference date\r\n" +
                "               ReferenceMonth      Integer month for reference date\r\n" +
                "               ReferenceDay        Integer day for reference date\r\n" +
                "               PartialYear         Integer year for partial date\r\n" +
                "               PartialMonth        Integer month for partial date\r\n" +
                "               PartailDay          Integer day for partial date\r\n" +
                "               IsValid             Local funciton success flag\r\n" +
                "\r\n" +
                "--------------------------------------------------------------------------}\r\n" +
                "\r\n" +
                "var\r\n" +
                "    ReferenceYear                            :integer;\r\n" +
                "    ReferenceMonth                           :integer;\r\n" +
                "    ReferenceDay                             :integer;\r\n" +
                "    PartialYear                              :integer;\r\n" +
                "    PartialMonth                             :integer;\r\n" +
                "    PartialDay                               :integer;\r\n" +
                "    IsValid                                  :boolean;\r\n" +
                "\r\n" +
                "begin { function CompleteDate }\r\n" +
                "\r\n" +
                "    IsValid := true;\r\n" +
                "\r\n" +
                "    if (not CheckDDMMMYYDate(PartialDate)) then\r\n" +
                "        if (CheckDDMMMYYDate(ReferenceDate)) then\r\n" +
                "        begin\r\n" +
                "            if (DivideUniversalDate(ReferenceDate , ReferenceYear ,\r\n" +
                "                ReferenceMonth , ReferenceDay)) and\r\n" +
                "                (DivideUniversalDate(PartialDate , PartialYear ,\r\n" +
                "                PartialMonth , PartialDay)) then\r\n" +
                "            begin\r\n" +
                "                if (ReferenceDay = PartialDay) and\r\n" +
                "                    (PartialMonth = -1) then\r\n" +
                "                begin\r\n" +
                "                    PartialYear := ReferenceYear;\r\n" +
                "                    PartialMonth := ReferenceMonth;\r\n" +
                "                end { if }\r\n" +
                "                else if (ReferenceMonth = 12) and (PartialMonth = 1) then\r\n" +
                "                    PartialYear := ReferenceYear + 1\r\n" +
                "                else if (ReferenceMonth = 1) and (PartialMonth = 12) then\r\n" +
                "                    PartialYear := ReferenceYear - 1\r\n" +
                "                else if (PartialMonth = -1) then\r\n" +
                "                begin\r\n" +
                "                    if (Abs(ReferenceDay - PartialDay) <= 14) then\r\n" +
                "                    begin\r\n" +
                "                        PartialMonth := ReferenceMonth;\r\n" +
                "                        PartialYear := ReferenceYear;\r\n" +
                "                    end { if }\r\n" +
                "                    else if (ReferenceDay > PartialDay) then\r\n" +
                "                    begin\r\n" +
                "                        PartialMonth := ReferenceMonth + 1;\r\n" +
                "                        if (PartialMonth > 12) then\r\n" +
                "                        begin\r\n" +
                "                            PartialMonth := 1;\r\n" +
                "                            PartialYear := ReferenceYear + 1;\r\n" +
                "                        end { if }\r\n" +
                "                        else\r\n" +
                "                            PartialYear := ReferenceYear;\r\n" +
                "                    end { else }\r\n" +
                "                    else { ReferenceDay < PartialDay }\r\n" +
                "                    begin\r\n" +
                "                        PartialMonth := ReferenceMonth - 1;\r\n" +
                "                        if (PartialMonth < 1) then\r\n" +
                "                        begin\r\n" +
                "                            PartialMonth := 12;\r\n" +
                "                            PartialYear := ReferenceYear - 1;\r\n" +
                "                        end { if }\r\n" +
                "                        else\r\n" +
                "                            PartialYear := ReferenceYear;\r\n" +
                "                    end; { else }\r\n" +
                "                end { if }\r\n" +
                "                else\r\n" +
                "                    PartialYear := ReferenceYear;\r\n" +
                "                IsValid := BuildUniversalDate\r\n" +
                "                    (PartialDate , PartialYear , PartialMonth , PartialDay);\r\n" +
                "            end { if }\r\n" +
                "            else\r\n" +
                "                IsValid := false;\r\n" +
                "        end { if }\r\n" +
                "        else\r\n" +
                "            IsValid := false;\r\n" +
                "\r\n" +
                "    CompleteDate := IsValid;\r\n" +
                "\r\n" +
                "end; { function CompleteDate }\r\n" +
                "\r\n" +
                "{** 07-30-98 * SecondsSinceMidnight **************************************}\r\n" +
                "\r\n" +
                "function SecondsSinceMidnight :longint;\r\n" +
                "\r\n" +
                "var\r\n" +
                "    Hours                                    :word;\r\n" +
                "    Minutes                                  :word;\r\n" +
                "    Seconds                                  :word;\r\n" +
                "    Sec1000                                  :word;\r\n" +
                "\r\n" +
                "    lHours                                   :longint;\r\n" +
                "    lMinutes                                 :longint;\r\n" +
                "    lSeconds                                 :longint;\r\n" +
                "\r\n" +
                "begin { function SecondSinceMidnight }\r\n" +
                "\r\n" +
                "    decodetime(GetUTCTime , Hours , Minutes , Seconds , Sec1000);\r\n" +
                "\r\n" +
                "    lHours := Hours;\r\n" +
                "    lMinutes := Minutes;\r\n" +
                "    lSeconds := Seconds;\r\n" +
                "\r\n" +
                "    SecondsSinceMidnight := (lHours * SEC_PER_HR) +\r\n" +
                "        (lMinutes * SEC_PER_MIN) +\r\n" +
                "        lSeconds;\r\n" +
                "\r\n" +
                "end; { function SecondSinceMidnight }\r\n" +
                "\r\n" +
                "{** GetUniversalTime * 07-30-98 ******************************************}\r\n" +
                "\r\n" +
                "function GetUniversalTime :UniversalTimeType;\r\n" +
                "var\r\n" +
                "    Hours , Minutes , Seconds                :string[3];\r\n" +
                "    wHours , wMinutes , wSeconds , wSec1000  :word;\r\n" +
                "begin\r\n" +
                "    decodetime(GetUTCTime , wHours , wMinutes , wSeconds , wSec1000);\r\n" +
                "    str((wHours + 100) :3 , Hours);\r\n" +
                "    str((wMinutes + 100) :3 , Minutes);\r\n" +
                "    str((wSeconds + 100) :3 , Seconds);\r\n" +
                "    GetUniversalTime := copy(Hours , 2 , 2) + copy(Minutes , 2 , 2) + copy(Seconds , 2 , 2);\r\n" +
                "end; {GetUniversalTime }\r\n" +
                "\r\n" +
                "{** GetUniversalTimeMS * 01-04-02 ******************************************}\r\n" +
                "\r\n" +
                "function GetUniversalTimeMS :UniversalTimeTypeMS;\r\n" +
                "var\r\n" +
                "    Hours , Minutes , Seconds                :string[3];\r\n" +
                "    MSSeconds                                :string[4];\r\n" +
                "    wHours , wMinutes , wSeconds , wSec1000  :word;\r\n" +
                "begin\r\n" +
                "    decodetime(GetUTCTime , wHours , wMinutes , wSeconds , wSec1000);\r\n" +
                "    str((wHours + 100) :3 , Hours);\r\n" +
                "    str((wMinutes + 100) :3 , Minutes);\r\n" +
                "    str((wSeconds + 100) :3 , Seconds);\r\n" +
                "    str((wSec1000 + 1000) :4 , MSSeconds);\r\n" +
                "    GetUniversalTimeMS := copy(Hours , 2 , 2) + copy(Minutes , 2 , 2)\r\n" +
                "        + copy(Seconds , 2 , 2) + copy(MSSeconds , 2 , 3);\r\n" +
                "end; {GetUniversalTimeMS }\r\n" +
                "\r\n" +
                "{** 08-05-98 * GetUniversalDate *******************************************}\r\n" +
                "\r\n" +
                "function GetUniversalDate :UniversalDateType;\r\n" +
                "begin\r\n" +
                "  Result := ConvertDateTimeToUniversalDate(GetUTCDateTime());\r\n" +
                "  if FTestDate <> '' then\r\n" +
                "      Result := FTestDate;\r\n" +
                "end; {GetUniversalDate }\r\n" +
                "\r\n" +
                "function GetCrewDate(RefDateTime: TDateTime = 0) :CrewDateType;\r\n" +
                "var\r\n" +
                "  S: AnsiString;\r\n" +
                "begin\r\n" +
                "  if RefDateTime = 0 then\r\n" +
                "    RefDateTime := GetUTCDateTime;\r\n" +
                "  S := FormatDateTime('YYYYMMDD', RefDateTime);\r\n" +
                "  Move(S[1], Result, Length(S));\r\n" +
                "end; {GetCrewDate }\r\n" +
                "\r\n" +
                "function GetUniversalYear :string;\r\n" +
                "var\r\n" +
                "    wYear , wMonth , wDay                    :word;\r\n" +
                "    Year                                     :string[4];\r\n" +
                "begin\r\n" +
                "    decodedate(GetUTCDateTime , wYear , wMonth , wDay);\r\n" +
                "    str(wYear , Year);\r\n" +
                "    GetUniversalYear := Year;\r\n" +
                "end; {GetUniversalYear}\r\n" +
                "\r\n" +
                "function AddCenturyToYear(YearOfCentury :string) :string;\r\n" +
                "{Adds the appropriate 2 digit century prefix to a 2 digit year}\r\n" +
                "var\r\n" +
                "    Century                                  :string[2];\r\n" +
                "    CurrentNYear                             :string[2];\r\n" +
                "    Year                                     :string[4];\r\n" +
                "    NCentury                                 :integer;\r\n" +
                "    rc                                       :integer;\r\n" +
                "\r\n" +
                "begin\r\n" +
                "    Year := GetUniversalYear;\r\n" +
                "    if YearOfCentury = copy(Year , 3 , 2) then\r\n" +
                "        AddCenturyToYear := Year\r\n" +
                "    else\r\n" +
                "    begin\r\n" +
                "        CurrentNYear := copy(Year , 3 , 2);\r\n" +
                "        val(copy(Year , 1 , 2) , NCentury , rc);\r\n" +
                "        if YearOfCentury > '75' then\r\n" +
                "        begin\r\n" +
                "            if CurrentNYear < '25' then\r\n" +
                "                dec(NCentury);\r\n" +
                "        end\r\n" +
                "        else\r\n" +
                "        begin\r\n" +
                "            if (YearOfCentury < '25')\r\n" +
                "                and (CurrentNYear > '75') then\r\n" +
                "                inc(NCentury);\r\n" +
                "        end;\r\n" +
                "        Century := Int2FullStr(NCentury , 2);\r\n" +
                "        AddCenturyToYear := Century + YearOfCentury\r\n" +
                "    end; //else not current year\r\n" +
                "end; {AddCenturytoYear}\r\n" +
                "\r\n" +
                "{** 04-23-97 * NumericDate ***********************************************}\r\n" +
                "\r\n" +
                "function IsValidInteger(S: String): Boolean;\r\n" +
                "var\r\n" +
                "  Value: Integer;\r\n" +
                "begin\r\n" +
                "  Result := TryStrToInt(S, Value);\r\n" +
                "end;\r\n" +
                "\r\n" +
                "function NumericDate\r\n" +
                "    (\r\n" +
                "    const iDate :UniversalDateType\r\n" +
                "    )\r\n" +
                "    :NumericDateType;\r\n" +
                "\r\n" +
                "    { if s = '00XXX00' the function will set the date to the maximum\r\n" +
                "      date allowed with this logic - MAY 15, 9999 }\r\n" +
                "\r\n" +
                "var\r\n" +
                "    i                                        :integer;\r\n" +
                "    NCentury , NYear , NMonth , NDay         :ShortInt;\r\n" +
                "    Year                                     :string[4];\r\n" +
                "    s                                        :UniversalDateType;\r\n" +
                "begin { function NumericDate }\r\n" +
                "    if (iDate = '00XXX00') then\r\n" +
                "    begin\r\n" +
                "        NCentury := 99;\r\n" +
                "        NYear := 99;\r\n" +
                "        NMonth := 05;\r\n" +
                "        NDay := 15;\r\n" +
                "    end { if }\r\n" +
                "    else\r\n" +
                "    begin\r\n" +
                "        s := iDate;\r\n" +
                "        if not ValidateDate(s) then\r\n" +
                "        begin\r\n" +
                "            NCentury := 0;\r\n" +
                "            NYear := 0;\r\n" +
                "            NMonth := 0;\r\n" +
                "            NDay := 0;\r\n" +
                "        end\r\n" +
                "        else\r\n" +
                "        begin\r\n" +
                "            NMonth := 0;\r\n" +
                "            for i := 1 to 12 do\r\n" +
                "                if copy(s , 3 , 3) = AlphaMonths[i] then\r\n" +
                "                    NMonth := i;\r\n" +
                "            val(copy(s , 1 , 2) , NDay , i);\r\n" +
                "            if i <> 0 then\r\n" +
                "                NDay := 0;\r\n" +
                "            if not IsValidInteger(copy(s , 6 , 2)) then\r\n" +
                "            begin\r\n" +
                "                NYear := 0;\r\n" +
                "                NCentury := 0;\r\n" +
                "            end\r\n" +
                "            else\r\n" +
                "            begin\r\n" +
                "                Year := AddCenturyToYear(copy(s , 6 , 2));\r\n" +
                "                val(copy(Year , 1 , 2) , NCentury , i);\r\n" +
                "                val(copy(Year , 3 , 2) , NYear , i);\r\n" +
                "            end;\r\n" +
                "            if (NMonth = 0)\r\n" +
                "                or (NDay = 0) then\r\n" +
                "                NCentury := 0;\r\n" +
                "        end; // valid date\r\n" +
                "    end; // not 00XXX00\r\n" +
                "\r\n" +
                "    NumericDate := chr(NCentury) + chr(NYear) + chr(NMonth) + chr(NDay);\r\n" +
                "end; { function NumericDate }\r\n" +
                "\r\n" +
                "function ValidateDate(var s :string) :boolean;\r\n" +
                "var\r\n" +
                "    mm , dd , yy , i                         :integer;\r\n" +
                "    wDate                                    :UniversalDateType;\r\n" +
                "begin\r\n" +
                "    Result := True;\r\n" +
                "    if (s = '00XXX00') then\r\n" +
                "        EXIT;\r\n" +
                "    wDate := GetUniversalDate;\r\n" +
                "    mm := 0;\r\n" +
                "    i := 1;\r\n" +
                "    s := JustData(s);\r\n" +
                "    while (i <= 12) and\r\n" +
                "        (pos(AlphaMonths[i] , s) = 0) do\r\n" +
                "        inc(i);\r\n" +
                "    if i <= 12 then\r\n" +
                "    begin\r\n" +
                "        mm := i;\r\n" +
                "        i := pos(AlphaMonths[i] , s);\r\n" +
                "        if i < 3 then\r\n" +
                "            s := '0' + s;\r\n" +
                "        if length(s) < 7 then\r\n" +
                "            s := s + copy(wDate , 6 , 2);\r\n" +
                "    end //valid month found\r\n" +
                "    else\r\n" +
                "        Result := false;\r\n" +
                "    val(copy(s , 1 , 2) , dd , i);\r\n" +
                "    if i <> 0 then\r\n" +
                "        Result := false;\r\n" +
                "    if dd < 1 then\r\n" +
                "        Result := false;\r\n" +
                "    val(copy(s , 6 , 2) , yy , i);\r\n" +
                "    if yy < 0 then\r\n" +
                "        Result := false;\r\n" +
                "    if i <> 0 then\r\n" +
                "        Result := false;\r\n" +
                "    case mm of\r\n" +
                "        1 , 3 , 5 , 7 , 8 , 10 , 12\r\n" +
                "            :\r\n" +
                "            begin\r\n" +
                "                if dd > 31 then\r\n" +
                "                    Result := false;\r\n" +
                "            end;\r\n" +
                "        4 , 6 , 9 , 11\r\n" +
                "            :\r\n" +
                "            begin\r\n" +
                "                if dd > 30 then\r\n" +
                "                    Result := false;\r\n" +
                "            end;\r\n" +
                "        2\r\n" +
                "            :\r\n" +
                "            begin\r\n" +
                "                if LeapYear(yy) then\r\n" +
                "                begin\r\n" +
                "                    if dd > 29 then\r\n" +
                "                        Result := false;\r\n" +
                "                end\r\n" +
                "                else\r\n" +
                "                begin\r\n" +
                "                    if dd > 28 then\r\n" +
                "                        Result := false;\r\n" +
                "                end;\r\n" +
                "            end;\r\n" +
                "    end;\r\n" +
                "end;\r\n" +
                "\r\n" +
                "function ValidateTime(s :string): Boolean;\r\n" +
                "begin\r\n" +
                "    Result := True;\r\n" +
                "    if (s[1] < '0')\r\n" +
                "        or (s[1] > '2') then\r\n" +
                "        Result := false;\r\n" +
                "    if (s[2] < '0')\r\n" +
                "        or (s[2] > '9') then\r\n" +
                "        Result := false;\r\n" +
                "    if (s[1] = '2')\r\n" +
                "        and (s[2] > '3') then\r\n" +
                "        Result := false;\r\n" +
                "    if (s[3] < '0')\r\n" +
                "        or (s[3] > '5') { was 'and' should be 'or' ktj 5/1/96 } then\r\n" +
                "        Result := false;\r\n" +
                "    if (s[4] < '0')\r\n" +
                "        or (s[4] > '9') { was 'and' should be 'or' ktj 5/1/96 } then\r\n" +
                "        Result := false;\r\n" +
                "end;\r\n" +
                "\r\n" +
                "function ValidateGMTVariance(s :string): Boolean;\r\n" +
                "begin\r\n" +
                "    Result := True;\r\n" +
                "    if (s[1] <> ' ')\r\n" +
                "        and (s[1] <> '+')\r\n" +
                "        and (s[1] <> '-') then\r\n" +
                "        Result := false;\r\n" +
                "    if (s[2] < '0')\r\n" +
                "        or (s[2] > '1') then\r\n" +
                "        Result := false;\r\n" +
                "    if (s[3] < '0')\r\n" +
                "        or (s[3] > '9') then\r\n" +
                "        Result := false;\r\n" +
                "    if (s[2] = '1')\r\n" +
                "        and (s[3] > '4') {3 instead of 2 to allow for DST at Dateline} {now 4 for Tonga} then\r\n" +
                "        Result := false;\r\n" +
                "    if (s[4] < '0')\r\n" +
                "        and (s[4] > '5') then\r\n" +
                "        Result := false;\r\n" +
                "    if (s[5] < '0')\r\n" +
                "        and (s[5] > '9') then\r\n" +
                "        Result := false;\r\n" +
                "end; {ValidateGMTVariance}\r\n" +
                "                                                  {unit DateTime}\r\n" +
                "\r\n" +
                "function ValidateDateChange(s :string): Boolean;\r\n" +
                "begin\r\n" +
                "    Result := True;\r\n" +
                "    if (s[1] <> ' ')\r\n" +
                "        and (s[1] <> '+')\r\n" +
                "        and (s[1] <> '-') then\r\n" +
                "        Result := false;\r\n" +
                "    if s[2] <> ' ' then\r\n" +
                "    begin\r\n" +
                "        if (s[2] < '0')\r\n" +
                "            or (s[2] > '2') then\r\n" +
                "            Result := false;\r\n" +
                "    end;\r\n" +
                "end; {ValidateDateChange}\r\n" +
                "\r\n" +
                "function ValidateDailyFrequency(s :string): Boolean;\r\n" +
                "var\r\n" +
                "    i ,\r\n" +
                "        FirstBlank                           :integer;\r\n" +
                "    PreviousChar                             :char;\r\n" +
                "begin\r\n" +
                "    Result := True;\r\n" +
                "    FirstBlank := pos(' ' , s);\r\n" +
                "    if FirstBlank = 0 then\r\n" +
                "        FirstBlank := 8;\r\n" +
                "    if s[1] = 'X' then\r\n" +
                "        i := 2\r\n" +
                "    else\r\n" +
                "        i := 1;\r\n" +
                "    PreviousChar := '0';\r\n" +
                "    while i < FirstBlank do\r\n" +
                "    begin\r\n" +
                "        if (s[i] <= PreviousChar)\r\n" +
                "            or (s[i] > '7') then\r\n" +
                "            Result := false;\r\n" +
                "        PreviousChar := s[i];\r\n" +
                "        inc(i);\r\n" +
                "    end;\r\n" +
                "    while i <= 7 do\r\n" +
                "    begin\r\n" +
                "        if s[i] <> ' ' then\r\n" +
                "            Result := false;\r\n" +
                "        inc(i);\r\n" +
                "    end;\r\n" +
                "end; {ValidateDailyFrequency}\r\n" +
                "                                                  {unit DateTime}\r\n" +
                "\r\n" +
                "function LeapYear(YearAD :integer) :boolean;\r\n" +
                "begin\r\n" +
                "    if YearAD < 100 then\r\n" +
                "        YearAD := Str2Int(AddCenturyToYear(Int2FullStr(YearAD , 2)));\r\n" +
                "    if copy(Int2IntStr(YearAD , 4) , 3 , 2) = '00' then\r\n" +
                "    begin\r\n" +
                "        if (YearAD mod 400) = 0 then\r\n" +
                "            LeapYear := true\r\n" +
                "        else\r\n" +
                "            LeapYear := false;\r\n" +
                "    end\r\n" +
                "    else\r\n" +
                "    begin\r\n" +
                "        if (YearAD mod 4) = 0 then\r\n" +
                "            LeapYear := true\r\n" +
                "        else\r\n" +
                "            LeapYear := false;\r\n" +
                "    end;\r\n" +
                "end; {LeapYear}\r\n" +
                "\r\n" +
                "function JulianDay(DateNumeric :string) :longint;\r\n" +
                "var\r\n" +
                "    Century , Year , Month , DayOfMonth      :integer;\r\n" +
                "    YearsAD , DayOfYear                      :longint;\r\n" +
                "begin\r\n" +
                "    JulianDay := 0;\r\n" +
                "    Century := ord(DateNumeric[1]);\r\n" +
                "    Year := ord(DateNumeric[2]);\r\n" +
                "    Month := ord(DateNumeric[3]);\r\n" +
                "    DayOfMonth := ord(DateNumeric[4]);\r\n" +
                "    if (Month < 1) or (Month > 12) then\r\n" +
                "        EXIT;\r\n" +
                "    if (DayOfMonth < 1) or (DayOfMonth > 31) then\r\n" +
                "        EXIT;\r\n" +
                "    YearsAD := (Century * 100) + (Year - 1);\r\n" +
                "    if (Month > 2)\r\n" +
                "        and (LeapYear(YearsAD + 1)) then\r\n" +
                "        DayOfYear := AnnualDaysToMonth[Month] + DayOfMonth + 1\r\n" +
                "    else\r\n" +
                "        DayOfYear := AnnualDaysToMonth[Month] + DayOfMonth;\r\n" +
                "    JulianDay := DayOfYear;\r\n" +
                "end; {JulianDay}\r\n" +
                "\r\n" +
                "function DayAD(DateNumeric :string) :longint;\r\n" +
                "var\r\n" +
                "    Century , Year , Month , DayOfMonth      :integer;\r\n" +
                "    YearsAD , DayOfYear                      :longint;\r\n" +
                "begin\r\n" +
                "    DayAD := 1;\r\n" +
                "    Century := ord(DateNumeric[1]);\r\n" +
                "    Year := ord(DateNumeric[2]);\r\n" +
                "    Month := ord(DateNumeric[3]);\r\n" +
                "    DayOfMonth := ord(DateNumeric[4]);\r\n" +
                "    if (Month < 1) or (Month > 12) then\r\n" +
                "        EXIT;\r\n" +
                "    if (DayOfMonth < 1) or (DayOfMonth > 31) then\r\n" +
                "        EXIT;\r\n" +
                "    YearsAD := (Century * 100) + (Year - 1);\r\n" +
                "    if (Month > 2)\r\n" +
                "        and (LeapYear(YearsAD + 1)) then\r\n" +
                "        DayOfYear := AnnualDaysToMonth[Month] + DayOfMonth + 1\r\n" +
                "    else\r\n" +
                "        DayOfYear := AnnualDaysToMonth[Month] + DayOfMonth;\r\n" +
                "    DayAD := (YearsAD * 365)\r\n" +
                "        + (YearsAD div 4) {number of leap years}\r\n" +
                "        + DayOfYear;\r\n" +
                "end; {DayAD}\r\n" +
                "\r\n" +
                "function DayOfWeekAD(DayAD :longint) :shortint;\r\n" +
                "begin\r\n" +
                "    DayOfWeekAD := ((DayAD - 2) mod 7) + 1;\r\n" +
                "end; {DayOfWeekAD}\r\n" +
                "\r\n" +
                "function DayOfWeekAlpha(DayOfWeek :shortint) :string;\r\n" +
                "begin\r\n" +
                "    DayOfWeekAlpha := AlphaDaysOfWeek[DayOfWeek];\r\n" +
                "end; {DayOfWeekAlpha}\r\n" +
                "\r\n" +
                "function NumericDayOfWeek(Date :TDateTime) :integer;\r\n" +
                "var\r\n" +
                "    DayOfWeek                                :string;\r\n" +
                "    I                                        :integer;\r\n" +
                "\r\n" +
                "begin\r\n" +
                "    try //if FormatDateTime throws an exception\r\n" +
                "        DayOfWeek := FormatDateTime('ddd' , Date);\r\n" +
                "        result := 0; //no match\r\n" +
                "        for I := 1 to 7 do\r\n" +
                "        begin\r\n" +
                "            if (UpperCase(DayOfWeek) = AlphaDaysOfWeek[I]) then\r\n" +
                "            begin\r\n" +
                "                result := I;\r\n" +
                "                break;\r\n" +
                "            end;\r\n" +
                "        end;\r\n" +
                "    except\r\n" +
                "        result := -1; //bad day of week\r\n" +
                "    end;\r\n" +
                "end;\r\n" +
                "\r\n" +
                "function DayADToDate(DayAD :longint) :string;\r\n" +
                "var\r\n" +
                "    DaysOverQuad , YearsOverQuad , YearAD , i ,\r\n" +
                "        Month , DayOfMonth                   :integer;\r\n" +
                "    DayOfYear , Quad                         :longint;\r\n" +
                "    DayString , YearString                   :string[5];\r\n" +
                "begin\r\n" +
                "    DayADToDate := '       ';\r\n" +
                "    Month := 0;\r\n" +
                "    if (DayAD < 1461) or (DayAD > 2000000) then\r\n" +
                "        EXIT;\r\n" +
                "    Quad := (DayAD - 1) div 1461;\r\n" +
                "    DaysOverQuad := DayAD mod 1461;\r\n" +
                "    if DaysOverQuad = 0 then\r\n" +
                "        YearsOverQuad := 3\r\n" +
                "    else\r\n" +
                "        YearsOverQuad := (DaysOverQuad - 1) div 365;\r\n" +
                "    YearAD := Quad * 4\r\n" +
                "        + YearsOverQuad\r\n" +
                "        + 1;\r\n" +
                "    DayOfYear := DayAD\r\n" +
                "        - Quad * 1461\r\n" +
                "        - YearsOverQuad * 365;\r\n" +
                "    if DayOfYear = 60 then\r\n" +
                "    begin\r\n" +
                "        if LeapYear(YearAD) then\r\n" +
                "        begin\r\n" +
                "            Month := 2;\r\n" +
                "            DayOfMonth := 29;\r\n" +
                "        end\r\n" +
                "        else\r\n" +
                "        begin\r\n" +
                "            Month := 3;\r\n" +
                "            DayOfMonth := 1;\r\n" +
                "        end;\r\n" +
                "    end\r\n" +
                "    else\r\n" +
                "    begin\r\n" +
                "        if LeapYear(YearAD)\r\n" +
                "            and (DayOfYear > 60) then\r\n" +
                "            DayOfYear := DayOfYear - 1;\r\n" +
                "        for i := 1 to 12 do\r\n" +
                "        begin\r\n" +
                "            if DayOfYear > AnnualDaysToMonth[i] then\r\n" +
                "                Month := i;\r\n" +
                "        end;\r\n" +
                "        DayOfMonth := DayOfYear - AnnualDaysToMonth[Month];\r\n" +
                "    end;\r\n" +
                "    str(YearAD :5 , YearString);\r\n" +
                "    str((DayOfMonth + 1000) :5 , DayString);\r\n" +
                "    DayADToDate := copy(DayString , 4 , 2)\r\n" +
                "        + AlphaMonths[Month]\r\n" +
                "        + copy(YearString , 4 , 2);\r\n" +
                "end; {DayADToDate}\r\n" +
                "\r\n" +
                "function NumericMonth(Month :string) :shortint;\r\n" +
                "var\r\n" +
                "    i                                        :integer;\r\n" +
                "begin\r\n" +
                "    NumericMonth := 0;\r\n" +
                "    for i := 1 to 12 do\r\n" +
                "        if Month = AlphaMonths[i] then\r\n" +
                "            NumericMonth := i;\r\n" +
                "end;\r\n" +
                "\r\n" +
                "function AlphaMonth(MonthNumeric :shortint) :string;\r\n" +
                "begin\r\n" +
                "    if (MonthNumeric < 13) and\r\n" +
                "        (MonthNumeric > 0) then\r\n" +
                "        AlphaMonth := AlphaMonths[MonthNumeric]\r\n" +
                "    else\r\n" +
                "        AlphaMonth := 'UNK';\r\n" +
                "end;\r\n" +
                "\r\n" +
                "function TimeToMinutes(Time :string) :integer;\r\n" +
                "var\r\n" +
                "    NumHours , NumMinutes , rc               :integer;\r\n" +
                "begin\r\n" +
                "    if Time[1] = ' ' then\r\n" +
                "        Time[1] := '0';\r\n" +
                "    if Time[2] = ' ' then\r\n" +
                "        Time[2] := '0';\r\n" +
                "    if Time[3] = ' ' then\r\n" +
                "        Time[3] := '0';\r\n" +
                "    if Time[4] = ' ' then\r\n" +
                "        Time[4] := '0';\r\n" +
                "    val(copy(Time , 1 , 2) , NumHours , rc);\r\n" +
                "    val(copy(Time , 3 , 2) , NumMinutes , rc);\r\n" +
                "    TimeToMinutes := NumHours * MIN_PER_HR + NumMinutes;\r\n" +
                "end; {TimeToMinutes}\r\n" +
                "\r\n" +
                "function MinutesToTime(Minutes :integer) :string;\r\n" +
                "var\r\n" +
                "    NumHours , NumMinutes                    :integer;\r\n" +
                "    CorrectedMinutes                         :integer;\r\n" +
                "    wTime                                    :string[6];\r\n" +
                "begin\r\n" +
                "    if (Minutes > 1440) then\r\n" +
                "        CorrectedMinutes := Minutes - 1440\r\n" +
                "    else\r\n" +
                "        CorrectedMinutes := Minutes;\r\n" +
                "    NumHours := CorrectedMinutes div MIN_PER_HR;\r\n" +
                "    NumMinutes := CorrectedMinutes mod MIN_PER_HR;\r\n" +
                "    if (NumHours = 24) and (NumMinutes = 0) then\r\n" +
                "        NumHours := 0;\r\n" +
                "   { Make sure it is left justified with zeros... }\r\n" +
                "    str((NumHours * 100 + NumMinutes + 10000) :6 , wTime);\r\n" +
                "    MinutesToTime := copy(wTime , 3 , 4);\r\n" +
                "end; {MinutesToTime}\r\n" +
                "\r\n" +
                "function MinutesToTime5H(Minutes :LongInt) :string;\r\n" +
                "var\r\n" +
                "    NumHours , NumMinutes                    :LongInt;\r\n" +
                "    wTime                                    :string[9];\r\n" +
                "begin\r\n" +
                "    NumHours := Minutes div MIN_PER_HR;\r\n" +
                "    NumMinutes := Minutes mod MIN_PER_HR;\r\n" +
                "    if (NumHours = 24) and (NumMinutes = 0) then\r\n" +
                "        NumHours := 0;\r\n" +
                "   { Make sure it is left justified with zeros... }\r\n" +
                "    str((NumHours * 100 + NumMinutes + 100000000) :9 , wTime);\r\n" +
                "    MinutesToTime5H := copy(wTime , 3 , 7);\r\n" +
                "end; {MinutesToTime5H}\r\n" +
                "\r\n" +
                "function SecondsToTime(Seconds :integer) :string;\r\n" +
                "var\r\n" +
                "    NumHours , NumMinutes , NumSeconds       :integer;\r\n" +
                "    CorrectedSeconds                         :integer;\r\n" +
                "    SecondsRemaining                         :integer;\r\n" +
                "    wTime                                    :string[8];\r\n" +
                "begin\r\n" +
                "    if (Seconds > SEC_PER_DAY) then\r\n" +
                "        CorrectedSeconds := Seconds - SEC_PER_DAY\r\n" +
                "    else\r\n" +
                "        CorrectedSeconds := Seconds;\r\n" +
                "    NumHours := CorrectedSeconds div SEC_PER_HR;\r\n" +
                "    SecondsRemaining := CorrectedSeconds mod SEC_PER_HR;\r\n" +
                "    NumMinutes := SecondsRemaining div SEC_PER_MIN;\r\n" +
                "    NumSeconds := SecondsRemaining mod SEC_PER_MIN;\r\n" +
                "    if (NumHours = 24) and (NumMinutes = 0) and (NumSeconds = 0) then\r\n" +
                "        NumHours := 0;\r\n" +
                "   { Make sure it is left justified with zeros... }\r\n" +
                "    str(((NumHours * 10000) + (NumMinutes * 100) + NumSeconds  + 10000000) :8 , wTime);\r\n" +
                "    SecondsToTime := copy(wTime , 3 , 6);\r\n" +
                "end; {SecondsToTime}\r\n" +
                "\r\n" +
                "function GMTMinuteAD(NumericLocalDate\r\n" +
                "    , LocalTime\r\n" +
                "    , GMTVariance\r\n" +
                "    , DateChange :string) :longint;\r\n" +
                "var\r\n" +
                "    Accumulator                              :longint;\r\n" +
                "    ReturnCode                               :integer;\r\n" +
                "begin\r\n" +
                "    val(DateChange , Accumulator , ReturnCode);\r\n" +
                "    if ReturnCode = 0 then\r\n" +
                "        Accumulator := DayAD(NumericLocalDate) + Accumulator\r\n" +
                "    else\r\n" +
                "        Accumulator := DayAD(NumericLocalDate);\r\n" +
                "    Accumulator := Accumulator * MIN_PER_DAY\r\n" +
                "        + TimeToMinutes(LocalTime);\r\n" +
                "    if GMTVariance[1] = '-' then\r\n" +
                "        GMTMinuteAD := Accumulator + TimeToMinutes(copy(GMTVariance , 2 , 4))\r\n" +
                "    else\r\n" +
                "        GMTMinuteAD := Accumulator - TimeToMinutes(copy(GMTVariance , 2 , 4));\r\n" +
                "end; {GMTMinuteAD}\r\n" +
                "\r\n" +
                "function MinAdToTime(i :longint) :string;\r\n" +
                "begin\r\n" +
                "    Result := MinutesToTime(i mod MIN_PER_DAY);\r\n" +
                "    if (ValidEAGLETime(Result) = false) then\r\n" +
                "        Result := '    ';\r\n" +
                "end; // MinAdToTime\r\n" +
                "\r\n" +
                "function MinAdToDate(i: longint; var ivIsOK: Boolean): string;\r\n" +
                "begin\r\n" +
                "    Result := DayAdToDate(i div MIN_PER_DAY);\r\n" +
                "    ivIsOK := ValidateDate(Result);\r\n" +
                "    if (not ivIsOK) then\r\n" +
                "        Result := '       ';\r\n" +
                "end; // MinAdToTime\r\n" +
                "\r\n" +
                "function MinAdToDate(i: longint): string;\r\n" +
                "var\r\n" +
                "  vIsOK: Boolean;\r\n" +
                "begin\r\n" +
                "  Result := MinAdToDate(i, vIsOK);\r\n" +
                "end;\r\n" +
                "\r\n" +
                "procedure CalculateNextMonth(Date :string;\r\n" +
                "    var NextMonth , Year :string);\r\n" +
                "var\r\n" +
                "    NMonth , NYear , i                       :integer;\r\n" +
                "begin\r\n" +
                "    NMonth := 0;\r\n" +
                "    for i := 1 to 12 do\r\n" +
                "        if copy(Date , 3 , 3) = AlphaMonths[i] then\r\n" +
                "            NMonth := i;\r\n" +
                "    if NMonth = 0 then\r\n" +
                "        NextMonth := '   '\r\n" +
                "    else\r\n" +
                "    begin\r\n" +
                "        inc(NMonth);\r\n" +
                "        val(copy(Date , 6 , 2) , NYear , i);\r\n" +
                "        if i <> 0 then\r\n" +
                "            NYear := 99;\r\n" +
                "        if NYear < 50 then\r\n" +
                "            NYear := NYear + 2000\r\n" +
                "        else\r\n" +
                "            NYear := NYear + 1900;\r\n" +
                "        if NMonth > 12 then\r\n" +
                "        begin\r\n" +
                "            NMonth := 1;\r\n" +
                "            inc(NYear);\r\n" +
                "        end;\r\n" +
                "        NextMonth := AlphaMonths[NMonth];\r\n" +
                "        str(NYear :4 , Year);\r\n" +
                "    end;\r\n" +
                "end; {NextMonth}\r\n" +
                "\r\n" +
                "function LocalToGMT(LocalTime , GMTVariance :string) :string;\r\n" +
                "var\r\n" +
                "    GMT                                      :integer;\r\n" +
                "begin\r\n" +
                "    if GMTVariance[1] = '-' then\r\n" +
                "        GMT := TimeToMinutes(LocalTime)\r\n" +
                "            + TimeToMinutes(copy(GMTVariance , 2 , 4))\r\n" +
                "    else\r\n" +
                "        GMT := TimeToMinutes(LocalTime)\r\n" +
                "            - TimeToMinutes(copy(GMTVariance , 2 , 4));\r\n" +
                "    if GMT < 0 then\r\n" +
                "    begin\r\n" +
                "        LocalToGMT := MinutesToTime(GMT + MIN_PER_DAY);\r\n" +
                "    end\r\n" +
                "    else\r\n" +
                "    begin\r\n" +
                "        if GMT >= MIN_PER_DAY then\r\n" +
                "        begin\r\n" +
                "            LocalToGMT := MinutesToTime(GMT - MIN_PER_DAY);\r\n" +
                "        end\r\n" +
                "        else\r\n" +
                "        begin\r\n" +
                "            LocalToGMT := MinutesToTime(GMT);\r\n" +
                "        end;\r\n" +
                "    end;\r\n" +
                "end; {LocalToGMT}\r\n" +
                "\r\n" +
                "function GMTtoLocal(GMT , GMTVariance :string) :string;\r\n" +
                "var\r\n" +
                "    Local                                    :integer;\r\n" +
                "begin\r\n" +
                "    if GMTVariance[1] = '-' then\r\n" +
                "        Local := TimeToMinutes(GMT)\r\n" +
                "            - TimeToMinutes(copy(GMTVariance , 2 , 4))\r\n" +
                "    else\r\n" +
                "        Local := TimeToMinutes(GMT)\r\n" +
                "            + TimeToMinutes(copy(GMTVariance , 2 , 4));\r\n" +
                "    if Local < 0 then\r\n" +
                "    begin\r\n" +
                "        GMTtoLocal := MinutesToTime(Local + MIN_PER_DAY);\r\n" +
                "    end\r\n" +
                "    else\r\n" +
                "    begin\r\n" +
                "        if Local >= MIN_PER_DAY then\r\n" +
                "        begin\r\n" +
                "            GMTtoLocal := MinutesToTime(Local - MIN_PER_DAY);\r\n" +
                "        end\r\n" +
                "        else\r\n" +
                "        begin\r\n" +
                "            GMTtoLocal := MinutesToTime(local);\r\n" +
                "        end;\r\n" +
                "    end;\r\n" +
                "end; {GMTtoLocal}\r\n" +
                "\r\n" +
                "function TimeToAMPM(Time :string) :string;\r\n" +
                "var\r\n" +
                "    iTime , rc                               :integer;\r\n" +
                "begin\r\n" +
                "    val(copy(Time , 1 , 2) , iTime , rc);\r\n" +
                "    if (rc = 0)\r\n" +
                "        and (iTime >= 1200) then\r\n" +
                "    begin\r\n" +
                "        iTime := iTime - 1200;\r\n" +
                "        str(iTime :4 , Time);\r\n" +
                "        TimeToAMPM := Time + 'P';\r\n" +
                "    end\r\n" +
                "    else\r\n" +
                "        TimeToAMPM := Time + 'A';\r\n" +
                "end; {TimeToAMPM}\r\n" +
                "\r\n" +
                "function TimeToAMPM1(Time :string) :string;\r\n" +
                "var\r\n" +
                "    TempHour                                 :string[2];\r\n" +
                "{   AMPM      :Char;}\r\n" +
                "    AMPM                                     :string[1];\r\n" +
                "    iTime                                    :integer;\r\n" +
                "    oTime                                    :string[5];\r\n" +
                "begin\r\n" +
                "    AMPM := ' ';\r\n" +
                "    TempHour := copy(Time , 1 , 2);\r\n" +
                "    iTime := Str2Int(TempHour);\r\n" +
                "    if iTime > 12 then\r\n" +
                "    begin\r\n" +
                "        iTime := iTime - 12;\r\n" +
                "        if iTime = 12 then\r\n" +
                "        begin\r\n" +
                "            iTime := 0;\r\n" +
                "            AMPM := 'A';\r\n" +
                "        end\r\n" +
                "        else\r\n" +
                "            AMPM := 'P';\r\n" +
                "    end\r\n" +
                "    else\r\n" +
                "    begin\r\n" +
                "        if iTime = 12 then\r\n" +
                "            AMPM := 'P'\r\n" +
                "        else\r\n" +
                "            AMPM := 'A';\r\n" +
                "    end;\r\n" +
                "    TempHour := '  ';\r\n" +
                "    TempHour := Int2IntStr(iTime , 2);\r\n" +
                "    if TempHour[1] = ' ' then\r\n" +
                "        TempHour[1] := '0';\r\n" +
                "    if TempHour[2] = ' ' then\r\n" +
                "        TempHour[2] := '0';\r\n" +
                "    oTime := TempHour[1]\r\n" +
                "        + TempHour[2]\r\n" +
                "        + Time[3]\r\n" +
                "        + Time[4]\r\n" +
                "        + AMPM[1];\r\n" +
                "    TimeToAMPM1 := oTime;\r\n" +
                "end; {TimeToAMPM1}\r\n" +
                "\r\n" +
                "function TimeToAMPM2(Time :string) :string;\r\n" +
                "var\r\n" +
                "    TempHour                                 :string[2];\r\n" +
                "{   AMPM      :Char;}\r\n" +
                "    AMPM                                     :string[1];\r\n" +
                "    iTime                                    :integer;\r\n" +
                "    oTime                                    :string[5];\r\n" +
                "begin\r\n" +
                "    AMPM := ' ';\r\n" +
                "    TempHour := copy(Time , 1 , 2);\r\n" +
                "    iTime := Str2Int(TempHour);\r\n" +
                "    if iTime > 12 then\r\n" +
                "    begin\r\n" +
                "        iTime := iTime - 12;\r\n" +
                "        if iTime = 12 then\r\n" +
                "        begin\r\n" +
                "            iTime := 0;\r\n" +
                "            AMPM := 'A';\r\n" +
                "        end\r\n" +
                "        else\r\n" +
                "            AMPM := 'P';\r\n" +
                "    end\r\n" +
                "    else\r\n" +
                "    begin\r\n" +
                "        if iTime = 12 then\r\n" +
                "            AMPM := 'P'\r\n" +
                "        else\r\n" +
                "            AMPM := 'A';\r\n" +
                "    end;\r\n" +
                "    TempHour := '  ';\r\n" +
                "    TempHour := Int2IntStr(iTime , 2);\r\n" +
                "    if TempHour[2] = ' ' then\r\n" +
                "    begin\r\n" +
                "        TempHour[1] := '1';\r\n" +
                "        TempHour[2] := '2';\r\n" +
                "    end;\r\n" +
                "    oTime := TempHour[1]\r\n" +
                "        + TempHour[2]\r\n" +
                "        + Time[3]\r\n" +
                "        + Time[4]\r\n" +
                "        + AMPM[1];\r\n" +
                "    if oTime = '1200P' then\r\n" +
                "        oTime := '1200N';\r\n" +
                "    if oTime = '1200A' then\r\n" +
                "        oTime := '1200M';\r\n" +
                "\r\n" +
                "    TimeToAMPM2 := oTime;\r\n" +
                "end; {TimeToAMPM2}\r\n" +
                "\r\n" +
                "function TimeAMToTime(Time :string) :string;\r\n" +
                "var\r\n" +
                "    iTime                                    :integer;\r\n" +
                "    oTime                                    :string[4];\r\n" +
                "begin\r\n" +
                "    if Time[5] = 'P' then\r\n" +
                "    begin\r\n" +
                "        iTime := TimeToMinutes(copy(Time , 1 , 4)) + 720;\r\n" +
                "        oTime := MinutesToTime(iTime);\r\n" +
                "        if (oTime[1] = '2') and (oTime[2] = '4') then\r\n" +
                "        begin\r\n" +
                "            oTime[1] := '1';\r\n" +
                "            oTime[2] := '2';\r\n" +
                "        end;\r\n" +
                "    end\r\n" +
                "    else\r\n" +
                "    begin\r\n" +
                "        oTime := copy(Time , 1 , 4);\r\n" +
                "    end;\r\n" +
                "    TimeAMToTime := oTime;\r\n" +
                "end; {TimeAMToTime}\r\n" +
                "\r\n" +
                "function TimeToTimeMaintenance(iTime :integer) :string;\r\n" +
                "var\r\n" +
                "    oTime                                    :real;\r\n" +
                "begin\r\n" +
                "    oTime := iTime / MIN_PER_HR;\r\n" +
                "    TimeToTimeMaintenance := Real2Str(oTime , 4);\r\n" +
                "end; { TimeToTimeMaintenance }\r\n" +
                "\r\n" +
                "function EagleToCCYYMMDDDate(EagleDate :UniversalDateType) : CCYYMMDDDateType;\r\n" +
                "var\r\n" +
                "    S: AnsiString;\r\n" +
                "begin\r\n" +
                "    S := Format('%s%.2d%s', [AddCenturyToYear(copy(EagleDate , 6 , 2)),\r\n" +
                "      NumericMonth(copy(EagleDate , 3 , 3)),\r\n" +
                "      Copy(EagleDate, 1, 2)]);\r\n" +
                "    Move(S[1], Result, Length(S));\r\n" +
                "end; {EagleToCCYYMMDDDate}\r\n" +
                "\r\n" +
                "procedure EagleToCrewTracDate\r\n" +
                "    (EagleDate :UniversalDateType;\r\n" +
                "    var StartCrewTracDate :CrewDateType);\r\n" +
                "begin\r\n" +
                "   StartCrewTracDate := EagleToCCYYMMDDDate(EagleDate);\r\n" +
                "end; {EagleToCrewTracDate}\r\n" +
                "\r\n" +
                "{** 04-10-95 * FlightEventUTCToLt ****************************************}\r\n" +
                "\r\n" +
                "procedure FlightEventUTCToLt\r\n" +
                "    (\r\n" +
                "    sNumFltDateUTC :NumericDateType;\r\n" +
                "    sSTDUTC :EagleTimeType;\r\n" +
                "    sFltEventUTC :EagleTimeType;\r\n" +
                "    sUTCVariance :UTCVarianceType;\r\n" +
                "    var stFltEventLt :FlightTimeType\r\n" +
                "    );\r\n" +
                "\r\n" +
                "{\r\n" +
                "  FILENAME:     datetime.pas\r\n" +
                "\r\n" +
                "  DESCRIPTION: FlightEventUTCToLt\r\n" +
                "\r\n" +
                "  PARAMETERS:  sNumFltDateUTC      UTC flight date (numeric)\r\n" +
                "               sSTDUTC             UTC schedule time of departure\r\n" +
                "               sFltEventUTC        UTC flight time\r\n" +
                "               sUTCVariance        UTC time variance for input time\r\n" +
                "               stFltEventLt        Lt flight Time\r\n" +
                "\r\n" +
                "  RETURNS:     None                NA\r\n" +
                "\r\n" +
                "  LOCAL:       iMinDifference      Time difference in minutes\r\n" +
                "               iMinFltTimeUTC      UTC flight time (0-1439); minutes\r\n" +
                "                                   since UTC \"midnight\"\r\n" +
                "               iMinSTDUTC          UTC scheduled departure time (0-1439);\r\n" +
                "                                   minutes since UTC \"midnight\"\r\n" +
                "               iDayADFltDateLt     Local date; days since base date\r\n" +
                "               iDayADFltDateUTC    UTC date; days since base date\r\n" +
                "               sFltDateLt          Local flight date string (ex. 02MAR95)\r\n" +
                "\r\n" +
                "-------------------------------------------------------------------------}\r\n" +
                "\r\n" +
                "var\r\n" +
                "    iMinDifference                           :integer;\r\n" +
                "    iMinFltTimeUTC                           :integer;\r\n" +
                "    iMinSTDUTC                               :integer;\r\n" +
                "    lDayADFltDateLt                          :longint;\r\n" +
                "    lDayADFltDateUTC                         :longint;\r\n" +
                "    sFltDateLt                               :UniversalDateType;\r\n" +
                "    sFltEventDate                            :CrewDateType;\r\n" +
                "\r\n" +
                "begin { procedure FlightEventUTCToLt }\r\n" +
                "\r\n" +
                "    lDayADFltDateUTC := DayAD(sNumFltDateUTC);\r\n" +
                "    iMinSTDUTC := TimeToMinutes(sSTDUTC);\r\n" +
                "    iMinFltTimeUTC := TimeToMinutes(sFltEventUTC);\r\n" +
                "\r\n" +
                "    iMinDifference := iMinFltTimeUTC - iMinSTDUTC;\r\n" +
                "    if (iMinDifference < -MAX_EARLY_DEP_MINS) then\r\n" +
                "        Inc(lDayADFltDateUTC)\r\n" +
                "    else if (iMinDifference > MAX_FLT_LENGTH_MINS) then\r\n" +
                "        Dec(lDayADFltDateUTC);\r\n" +
                "\r\n" +
                "  { Convert to local date and time }\r\n" +
                "    stFltEventLt.UTCVariance := TimeToMinutes(Copy(sUTCVariance , 2 , 4));\r\n" +
                "    if (sUTCVariance[1] = '-') then\r\n" +
                "        stFltEventLt.UTCVariance := -stFltEventLt.UTCVariance;\r\n" +
                "\r\n" +
                "    stFltEventLt.Time := iMinFltTimeUTC + stFltEventLt.UTCVariance;\r\n" +
                "    if (stFltEventLt.Time >= MIN_PER_DAY) then\r\n" +
                "    begin\r\n" +
                "        Dec(stFltEventLt.Time , MIN_PER_DAY);\r\n" +
                "        lDayADFltDateLt := lDayADFltDateUTC + 1;\r\n" +
                "    end { then }\r\n" +
                "    else if (stFltEventLt.Time < 0) then\r\n" +
                "    begin\r\n" +
                "        Inc(stFltEventLt.Time , MIN_PER_DAY);\r\n" +
                "        lDayADFltDateLt := lDayADFltDateUTC - 1;\r\n" +
                "    end { then }\r\n" +
                "    else\r\n" +
                "        lDayADFltDateLt := lDayADFltDateUTC;\r\n" +
                "\r\n" +
                "    sFltDateLt := DayADtoDate(lDayADFltDateLt);\r\n" +
                "    EagleToCrewTracDate(sFltDateLt , sFltEventDate);\r\n" +
                "    move(sFltEventDate , stFltEventLt.Date , 9);\r\n" +
                "\r\n" +
                "end; { procedure FlightEventUTCToLt }\r\n" +
                "\r\n" +
                "{** 04-07-95 * GMTToOUTOFFONINMVTCrewTracTime ****************************}\r\n" +
                "\r\n" +
                "procedure GMTtoOUTOFFONINMVTCrewTracTime\r\n" +
                "    (EagleNumGMTDate ,\r\n" +
                "    EagleSTDudt ,\r\n" +
                "    EagleETDudt ,\r\n" +
                "    EagleGMTTime ,\r\n" +
                "    EagleGMTVariance :string;\r\n" +
                "    var CrewTracTime :FlightTimeType);\r\n" +
                "var\r\n" +
                "    LocalDayAD                               :longint;\r\n" +
                "    GMTDayAD                                 :longint;\r\n" +
                "    GMTTime                                  :integer;\r\n" +
                "    LocalDate                                :string[7];\r\n" +
                "    CrewTracDate                             :CrewDateType;\r\n" +
                "begin\r\n" +
                "    with CrewTracTime do\r\n" +
                "    begin\r\n" +
                "        GMTDayAD := DayAD(EagleNumGMTDate);\r\n" +
                "        GMTTime := TimeToMinutes(EagleGMTTime);\r\n" +
                "     {Convert to local}\r\n" +
                "        UTCVariance := TimeToMinutes(copy(EagleGMTVariance , 2 , 4));\r\n" +
                "        if EagleGMTVariance[1] = '-' then\r\n" +
                "            UTCVariance := -UTCVariance;\r\n" +
                "        Time := GMTTime + UTCVariance;\r\n" +
                "        LocalDayAD := GMTDayAD;\r\n" +
                "        if Time >= MIN_PER_DAY then\r\n" +
                "        begin\r\n" +
                "            inc(LocalDayAD);\r\n" +
                "            Time := Time - MIN_PER_DAY;\r\n" +
                "        end;\r\n" +
                "        if Time < 0 then\r\n" +
                "        begin\r\n" +
                "            dec(LocalDayAD);\r\n" +
                "            Time := Time + MIN_PER_DAY;\r\n" +
                "        end;\r\n" +
                "        LocalDate := DayADtoDate(LocalDayAD);\r\n" +
                "        EagleToCrewTracDate(LocalDate , CrewTracDate);\r\n" +
                "        move(CrewTracDate[1] , Date[1] , 8);\r\n" +
                "    end;\r\n" +
                "end; {OUTOFFONINMVTCrewTracTime}\r\n" +
                "\r\n" +
                "function YMD2DMYEagle(inDate :string) :string;\r\n" +
                "begin\r\n" +
                "    YMD2DMYEagle := copy(inDate , 7 , 2)\r\n" +
                "        + AlphaMonth(Str2Int(copy(inDate , 5 , 2)))\r\n" +
                "        + copy(inDate , 3 , 2);\r\n" +
                "end;\r\n" +
                "\r\n" +
                "function ZIPDate(iNumDate :string) :string;\r\n" +
                "var\r\n" +
                "    i                                        :integer;\r\n" +
                "    Day , Month , Year                       :string[2];\r\n" +
                "    Date                                     :string[6];\r\n" +
                "begin\r\n" +
                "    str(ord(iNumDate[4]) :2 , Day);\r\n" +
                "    str(ord(iNumDate[3]) :2 , Month);\r\n" +
                "    str(ord(iNumDate[2]) :2 , Year);\r\n" +
                "    Date := Month + Day + Year;\r\n" +
                "    for i := 1 to 6 do\r\n" +
                "    begin\r\n" +
                "        if Date[i] = ' ' then\r\n" +
                "            Date[i] := '0';\r\n" +
                "    end;\r\n" +
                "    ZIPDate := Date;\r\n" +
                "end;\r\n" +
                "\r\n" +
                "function ComputeElapsedMinutes(Time1udt , Time2udt :string) :integer;\r\n" +
                "var\r\n" +
                "    dMin                                     :integer;\r\n" +
                "begin\r\n" +
                "    dMin := TimeToMinutes(Time2udt) - TimeToMinutes(Time1udt);\r\n" +
                "    if dMin < 0 then\r\n" +
                "        ComputeElapsedMinutes := dMin + MIN_PER_DAY\r\n" +
                "    else\r\n" +
                "        ComputeElapsedMinutes := dMin;\r\n" +
                "end; {ComputeElapsedMinutes}\r\n" +
                "\r\n" +
                "procedure Delay(DelayAmount :word);\r\n" +
                "\r\n" +
                "var\r\n" +
                "    EndTime                                  :TDateTime;\r\n" +
                "    FirstTime                                :TDateTime;\r\n" +
                "    DelayTime                                :double;\r\n" +
                "\r\n" +
                "begin\r\n" +
                "  { This has a resolution of 1/4 second because of the Sleep(250) but it\r\n" +
                "    is much easier on resources }\r\n" +
                "    FirstTime := Time;\r\n" +
                "    DelayTime := DelayAmount / 86400000;\r\n" +
                "    repeat\r\n" +
                "        Sleep(250);\r\n" +
                "        EndTime := Time;\r\n" +
                "        Application.ProcessMessages;\r\n" +
                "    until ((EndTime - FirstTime) > DelayTime) or (EndTime < FirstTime);\r\n" +
                "end; {Delay}\r\n" +
                "\r\n" +
                "{** 02-24-99 * NowUTC *****************************************************}\r\n" +
                "\r\n" +
                "Function NowUTC : TDateTime;\r\n" +
                "Var\r\n" +
                "	sysTime		: TSystemTime;\r\n" +
                "Begin\r\n" +
                "	GetSystemTime(sysTime);	//on Windows, always returns current UTC Time - Windows internally works only in UTC.\r\n" +
                "	Result := SystemTimeToDateTime(sysTime);\r\n" +
                "End;\r\n" +
                "\r\n" +
                "function nowUTCMinute: TDateTime;\r\n" +
                "\r\n" +
                "var\r\n" +
                "    SystemTime                          : TSystemTime;\r\n" +
                "    CurrentTime                         : TDateTime;\r\n" +
                "    CurrentDate                         : TDateTime;\r\n" +
                "\r\n" +
                "begin // nowUTCMinute\r\n" +
                "    GetSystemTime(SystemTime); // GetSystemTime gets time in UTC\r\n" +
                "    CurrentTime := EncodeTime(\r\n" +
                "            SystemTime.wHour,\r\n" +
                "            SystemTime.wMinute,\r\n" +
                "            0,  //Second\r\n" +
                "            0); //MilliSeconds\r\n" +
                "\r\n" +
                "    CurrentDate := EncodeDate(\r\n" +
                "            SystemTime.wYear,\r\n" +
                "            SystemTime.wMonth,\r\n" +
                "            SystemTime.wDay);\r\n" +
                "\r\n" +
                "        Result := CurrentDate + CurrentTime;\r\n" +
                "end; // nowUTCMinute\r\n" +
                "\r\n" +
                "{** 08-05-98 * GetUTCDate *****************************************************}\r\n" +
                "\r\n" +
                "function GetUTCDate :TDateTime;\r\n" +
                "begin // GetUTCDate\r\n" +
                "    Result := Trunc(GetUTCDateTime);\r\n" +
                "end; // GetUTCDate\r\n" +
                "\r\n" +
                "{** 07-30-98 * GetUTCTime *****************************************************}\r\n" +
                "\r\n" +
                "function GetUTCTime :TDateTime;\r\n" +
                "begin // GetUTCTime\r\n" +
                "    Result := GetUTCDateTime;\r\n" +
                "    Result := Result - Trunc(Result);\r\n" +
                "end; // GetUTCTime\r\n" +
                "\r\n" +
                "{** 07-30-98 * GetUTCDateTime *************************************************}\r\n" +
                "\r\n" +
                "function GetUTCDateTime :TDateTime;\r\n" +
                "\r\n" +
                "var\r\n" +
                "    SystemTime                               :TSystemTime;\r\n" +
                "    CurrentTime                              :TDateTime;\r\n" +
                "    CurrentDate                              :TDateTime;\r\n" +
                "\r\n" +
                "begin // GetUTCDateTime\r\n" +
                "    if (not AllTimeIsUTCBased) then\r\n" +
                "        Result := Now // get time from system clock without regard for utc time zone\r\n" +
                "    else\r\n" +
                "    begin\r\n" +
                "        GetSystemTime(SystemTime); // GetSystemTime gets time in UTC\r\n" +
                "\r\n" +
                "        CurrentTime := EncodeTime(\r\n" +
                "            SystemTime.wHour ,\r\n" +
                "            SystemTime.wMinute ,\r\n" +
                "            SystemTime.wSecond ,\r\n" +
                "            SystemTime.wMilliSeconds);\r\n" +
                "\r\n" +
                "        CurrentDate := EncodeDate(\r\n" +
                "            SystemTime.wYear ,\r\n" +
                "            SystemTime.wMonth ,\r\n" +
                "            SystemTime.wDay);\r\n" +
                "\r\n" +
                "        Result := CurrentDate + CurrentTime;\r\n" +
                "    end; // else\r\n" +
                "end; // GetUTCDateTime\r\n" +
                "\r\n" +
                "procedure TimeIsUTCBased(Value :boolean);\r\n" +
                "begin\r\n" +
                "    AllTimeIsUTCBased := Value;\r\n" +
                "end; // TimeIsUTCBased\r\n" +
                "\r\n" +
                "function NumToDate(DateNumeric :string) :string;\r\n" +
                "begin\r\n" +
                "    Result := DayADToDate(DayAD(DateNumeric));\r\n" +
                "end; // NumToDate\r\n" +
                "\r\n" +
                "// =============================================================================\r\n" +
                "// DMB - this is a new function\r\n" +
                "\r\n" +
                "function EagleDateToDateTime(const StringDateTime: ShortString; var ivIsOK: Boolean): TDateTime;\r\n" +
                "var\r\n" +
                "    Hour                                     :integer;\r\n" +
                "    Min                                      :integer;\r\n" +
                "    TempDate                                 :UniversalDateType;\r\n" +
                "    Day , Month , Year                       :Word;\r\n" +
                "begin\r\n" +
                "    TempDate := StringDateTime;\r\n" +
                "    ivIsOK := ValidateDate(TempDate);\r\n" +
                "    if ivIsOK then\r\n" +
                "    begin\r\n" +
                "       Hour := StrToIntDef(copy(StringDateTime , 9 , 2) , 0);\r\n" +
                "       Min := StrToIntDef(copy(StringDateTime , 11 , 2) , 0);\r\n" +
                "       Day := StrToIntDef(Copy(TempDate , 1 , 2), 0);\r\n" +
                "       Month := NumericMonth(Copy(TempDate , 3 , 3));\r\n" +
                "       Year := StrToIntDef(AddCenturyToYear(Copy(TempDate , 6 , 2)), 0);\r\n" +
                "       Result := Trunc(EncodeDate(Year, Month, Day)) + EncodeTime(Hour , Min , 0 , 0); // DMB\r\n" +
                "    end\r\n" +
                "    else\r\n" +
                "       Result := 0.0;\r\n" +
                "end;\r\n" +
                "\r\n" +
                "function EagleDateToDateTime(const StringDateTime: ShortString): TDateTime;\r\n" +
                "var\r\n" +
                "  vIsOK: Boolean;\r\n" +
                "begin\r\n" +
                "  Result := EagleDateToDateTime(StringDateTime, vIsOK);\r\n" +
                "end;\r\n" +
                "\r\n" +
                "function DateTimetoEagleDate(const DateTimeIn :TDateTime) :shortstring;\r\n" +
                "var\r\n" +
                "  Day, Month, Year : Word;\r\n" +
                "  DayStr, YearStr : shortstring;\r\n" +
                "begin\r\n" +
                "  if DateTimeIn = 0.0 then\r\n" +
                "     Result := '       '  //blank date\r\n" +
                "  else begin\r\n" +
                "     DecodeDate( DateTimeIn, Year, Month, Day );\r\n" +
                "\r\n" +
                "     DayStr  := PadL( IntToStr( Day ), 2, '0' );\r\n" +
                "     YearStr := PadL( IntToStr( Year ), 2, '0' );\r\n" +
                "\r\n" +
                "     Result := DayStr + AlphaMonth(Month) + copy( YearStr, 3, 2 );\r\n" +
                "  end;\r\n" +
                "end;\r\n" +
                "\r\n" +
                "function EagleDateTimeToDateTime(const iDate, iTime: ShortString; var ivIsOK: Boolean): TDateTime;\r\n" +
                "begin\r\n" +
                "  Result := EagleDateToDateTime(iDate, ivIsOK) + EagleTimeToDateTime(iTime);\r\n" +
                "end;\r\n" +
                "\r\n" +
                "function EagleDateTimeToDateTime(const iDate, iTime: ShortString): TDateTime;\r\n" +
                "var\r\n" +
                "  vIsOK: Boolean;\r\n" +
                "begin\r\n" +
                "  Result := EagleDateTimeToDateTime(iDate, iTime, vIsOK);\r\n" +
                "end;\r\n" +
                "\r\n" +
                "function EagleTimeToDateTime(const iTime: ShortString; const iFixTime : Boolean = False): TDateTime;\r\n" +
                "var\r\n" +
                "  vHH, vMM: Word;\r\n" +
                "begin\r\n" +
                "  vHH := StrToIntDef(Copy(iTime, 1, 2), 0);\r\n" +
                "  vMM := StrToIntDef(Copy(iTime, 3, 2), 0);\r\n" +
                "  if (iFixTime) and (vMM = 60) then //used when time is equal to e.g. 2160 - should be 2200\r\n" +
                "  begin\r\n" +
                "    vMM := 0;\r\n" +
                "    Result := EncodeTime(vHH, vMM, 0, 0);\r\n" +
                "    Result := IncHour(Result, 1)\r\n" +
                "  end\r\n" +
                "  else\r\n" +
                "    Result := EncodeTime(vHH, vMM, 0, 0);\r\n" +
                "end;\r\n" +
                "\r\n" +
                "function DateTimeToEagleTime(const iDateTime: TDateTime) : ShortString;\r\n" +
                "var\r\n" +
                "  vHours, vMins, vSecs, vMsecs : Word;\r\n" +
                "  vHoursStr, vMinsStr : ShortString;\r\n" +
                "begin\r\n" +
                "  Result := '    ';  //blank time\r\n" +
                "  if iDateTime = 0.0 then\r\n" +
                "    exit;\r\n" +
                "\r\n" +
                "  DecodeTime( iDateTime, vHours, vMins, vSecs, vMsecs );\r\n" +
                "  vHoursStr  := PadL( IntToStr( vHours ), 2, '0' );\r\n" +
                "  vMinsStr := PadL( IntToStr( vMins ), 2, '0' );\r\n" +
                "  Result := vHoursStr + vMinsStr;\r\n" +
                "end;\r\n" +
                "\r\n" +
                "function NumericYesterday :string;\r\n" +
                "begin\r\n" +
                "    Result := NumericDate(DayADtoDate(DayAD(NumericDate(GetUniversalDate)) - 1));\r\n" +
                "end; // NumericYesterday\r\n" +
                "\r\n" +
                "function NumericDateToUniversalDate(iNumDate :string) :UniversalDateType;\r\n" +
                "begin\r\n" +
                "    NumericDateToUniversalDate := copy(NumericToYYYYMMDD(iNumDate) , 7 , 2) +\r\n" +
                "        AlphaMonth(StrToIntDef(copy(NumericToYYYYMMDD(iNumDate) , 5 , 2), 0)) +\r\n" +
                "        copy(NumericToYYYYMMDD(iNumDate) , 3 , 2);\r\n" +
                "end;\r\n" +
                "\r\n" +
                "Function UniversalDateTimeToTDateTime\r\n" +
                "    (    UniversalDate: UniversalDateType;\r\n" +
                "         HHMM         : EagleTimeType;\r\n" +
                "     var iTDateTime   : TDateTime          ) : boolean;\r\n" +
                "var\r\n" +
                "   MMDDYY           :string[6];\r\n" +
                "   WorkDate         :TDateTime;\r\n" +
                "   WorkTime         :TDateTime;\r\n" +
                "Begin\r\n" +
                "    Result := false;\r\n" +
                "    iTDateTime := 0;\r\n" +
                "    if ValidEagleTime(HHMM) then\r\n" +
                "       if UniversalToUSDate(UniversalDate,MMDDYY) then\r\n" +
                "       begin\r\n" +
                "           WorkDate := EncodeDate(StrToInt(AddCenturyToYear(copy(MMDDYY,5,2))),\r\n" +
                "                                  StrToInt(copy(MMDDYY,1,2)),\r\n" +
                "                                  StrToInt(copy(MMDDYY,3,2)));\r\n" +
                "           WorkTime := EncodeTime(StrToInt(copy(HHMM,1,2)),\r\n" +
                "                                  StrToInt(copy(HHMM,3,2)),\r\n" +
                "                                  0,\r\n" +
                "                                  0);\r\n" +
                "\r\n" +
                "           iTDateTime := WorkDate + WorkTime;\r\n" +
                "           Result := true;\r\n" +
                "       end;\r\n" +
                "end;\r\n" +
                "\r\n" +
                "Function TDateTimeToUniversalDateTime\r\n" +
                "    (    iTDateTime   : TDateTime;\r\n" +
                "     var UniversalDate: UniversalDateType;\r\n" +
                "     var HHMM         : EagleTimeType ) : boolean;\r\n" +
                "var\r\n" +
                "   Hour, Min, Sec, MSec :word;\r\n" +
                "begin\r\n" +
                "   UniversalDate := ConvertDateTimeToUniversalDate(iTDateTime);\r\n" +
                "   decodetime(iTDateTime,Hour, Min, Sec, MSec);\r\n" +
                "   HHMM := int2fullstr(Hour,2) + int2fullstr(Min,2);\r\n" +
                "   result := true;\r\n" +
                "end;\r\n" +
                "\r\n" +
                "function TDateTimeToDDHHMM (iTDateTime   : TDateTime) :string;\r\n" +
                "var\r\n" +
                "   Hour, Min, Sec, MSec :word;\r\n" +
                "   Year, Month, Day: Word;\r\n" +
                "begin\r\n" +
                "   decodetime(iTDateTime,Hour, Min, Sec, MSec);\r\n" +
                "   decodedate(iTDateTime, Year, Month, Day);\r\n" +
                "   result := int2fullstr(Day,2) + int2fullstr(Hour,2) + int2fullstr(Min,2);\r\n" +
                "end;\r\n" +
                "\r\n" +
                "function DDHHMMToTDateTime (iDDHHMM   : string; iRefTimestamp: TDateTime = -1) :TDateTime;\r\n" +
                "var\r\n" +
                "   DD: word;\r\n" +
                "   Hour, Min, Sec, MSec :word;\r\n" +
                "   Year, Month, Day: Word;\r\n" +
                "   DateDateTime: TDateTime;\r\n" +
                "   TimeDateTime: TDateTime;\r\n" +
                "begin\r\n" +
                "   if iRefTimestamp = -1 then\r\n" +
                "     iRefTimestamp := Now;\r\n" +
                "   DD := Str2Int(copy(iDDHHMM,1,2));\r\n" +
                "   decodedate(iRefTimestamp, Year, Month, Day);\r\n" +
                "   if Day <> DD\r\n" +
                "   then begin\r\n" +
                "       if    (Day > 20)\r\n" +
                "         and (DD < 10)\r\n" +
                "       then begin\r\n" +
                "          if Month = 12\r\n" +
                "          then begin\r\n" +
                "             Month := 1;\r\n" +
                "             inc(Year);\r\n" +
                "          end\r\n" +
                "          else inc(Month);\r\n" +
                "       end\r\n" +
                "       else begin\r\n" +
                "           if    (Day < 20)\r\n" +
                "             and (DD > 10)\r\n" +
                "           then begin\r\n" +
                "               if Month = 1\r\n" +
                "               then begin\r\n" +
                "                   Month := 12;\r\n" +
                "                   dec(Year);\r\n" +
                "               end\r\n" +
                "               else dec(Month);\r\n" +
                "           end;\r\n" +
                "       end;\r\n" +
                "       Day := DD;\r\n" +
                "   end;\r\n" +
                "   DateDateTime := encodedate(Year, Month, Day);\r\n" +
                "   Hour := Str2Int(copy(iDDHHMM,3,2));\r\n" +
                "   Min  := Str2Int(copy(iDDHHMM,5,2));\r\n" +
                "   Sec  := 0;\r\n" +
                "   MSec := 0;\r\n" +
                "   TimeDateTime := encodetime(Hour, Min, Sec, MSec);\r\n" +
                "   result := DateDateTime + TimeDateTime;\r\n" +
                "end;\r\n" +
                "\r\n" +
                "{ TODO : Copied from Drb\\DateTime.pas }\r\n" +
                "\r\n" +
                "{** 10-27-99 * NumericDateToDDMMMYY **************************************}\r\n" +
                "Function NumericDateToDDMMMYY (NumericDateIn : String) : String;\r\n" +
                "Var\r\n" +
                "  i         : Integer;\r\n" +
                "  Day, Year : String[2];\r\n" +
                "  Month     : String[3];\r\n" +
                "  Date      : String[7];\r\n" +
                "\r\n" +
                "Begin // NumericDateToDDMMMYY\r\n" +
                "\r\n" +
                "  Str (Ord (NumericDateIn[4]):2, Day);\r\n" +
                "  Month := AlphaMonth (Ord (NumericDateIn[3]));\r\n" +
                "  Str (Ord (NumericDateIn[2]):2, Year);\r\n" +
                "\r\n" +
                "  Date := Day + Month + Year;\r\n" +
                "\r\n" +
                "  For i := 1 to 7 Do\r\n" +
                "    If Date[i] = ' ' then\r\n" +
                "      Date[i] := '0';\r\n" +
                "\r\n" +
                "  Result := Date;\r\n" +
                "\r\n" +
                "end; // NumericDateToDDMMMYY\r\n" +
                "\r\n" +
                "{ TODO : Copied from Drb\\DateTime.pas }\r\n" +
                "{** 11-04-99 * ValidNumericDate ***********************************************}\r\n" +
                "Function ValidNumericDate (Const NumericDateIn : String) : Boolean;\r\n" +
                "Var\r\n" +
                "  ValidCentury,\r\n" +
                "  ValidYear,\r\n" +
                "  ValidMonth,\r\n" +
                "  ValidDay     : Boolean;\r\n" +
                "\r\n" +
                "Begin // ValidNumericDate\r\n" +
                "\r\n" +
                "  ValidCentury := (NumericDateIn[1] >= #19) and (NumericDateIn[1] <= #99);\r\n" +
                "  ValidYear    := (NumericDateIn[2] >= #0)  and (NumericDateIn[2] <= #99);\r\n" +
                "  ValidMonth   := (NumericDateIn[3] >= #1)  and (NumericDateIn[3] <= #12);\r\n" +
                "  ValidDay     := (NumericDateIn[4] >= #1)  and (NumericDateIn[4] <= #31);\r\n" +
                "\r\n" +
                "  Result := ValidCentury and ValidYear and ValidMonth and ValidDay;\r\n" +
                "\r\n" +
                "end; // ValidNumericDate\r\n" +
                "\r\n" +
                "{ TODO : Copied from Drb\\DateTime.pas }{** 07-30-98 * DateTimeToDDMMMYY ************************************************}\r\n" +
                "function DateTimeToDDMMMYY( NewDate : TDateTime ) : string;\r\n" +
                "\r\n" +
                "var\r\n" +
                "  Year    : word;\r\n" +
                "  Month   : word;\r\n" +
                "  Day     : word;\r\n" +
                "  YearStr : string;\r\n" +
                "  DayStr  : string;\r\n" +
                "\r\n" +
                "begin // DateTimeToDDMMMYY\r\n" +
                "  try\r\n" +
                "    try\r\n" +
                "      DecodeDate(NewDate, Year, Month, Day);\r\n" +
                "    except\r\n" +
                "      DecodeDate(GetUTCDateTime, Year, Month, Day);\r\n" +
                "    end; // try\r\n" +
                "\r\n" +
                "    DayStr  := IntToStr(Day);\r\n" +
                "    if Length(DayStr) < 2 then\r\n" +
                "      DayStr := '0'+DayStr;\r\n" +
                "\r\n" +
                "    YearStr  := Copy(IntToStr(Year),3,2);\r\n" +
                "\r\n" +
                "    Result := DayStr+ DateTime.AlphaMonth(Month) + YearStr;\r\n" +
                "  except\r\n" +
                "    Result := coBlankDate;\r\n" +
                "  end;\r\n" +
                "end; // DateTimeToDDMMMYY\r\n" +
                "\r\n" +
                "{ TODO : Copied from Drb\\DateTime.pas }\r\n" +
                "function NumericDateToDateTime(const iNumericDate : string): TDatetime;\r\n" +
                "begin\r\n" +
                "  Result := DDMMMYYDateToDateTime(NumericDateToDDMMMYY(iNumericDate));\r\n" +
                "end;\r\n" +
                "\r\n" +
                "{ TODO : Copied from Drb\\DateTime.pas }\r\n" +
                "{** 04-20-04 * DDMMMYYDateToDateTime ******************************************}\r\n" +
                "function DDMMMYYDateToDateTime( DDMMMYYDate  : string ) : TDateTime;\r\n" +
                "\r\n" +
                "var\r\n" +
                "  YearStr : string[4];\r\n" +
                "  Year    : word;\r\n" +
                "  Month   : word;\r\n" +
                "  Day     : word;\r\n" +
                "\r\n" +
                "begin // DDMMMYYDateToDateTime\r\n" +
                "  try\r\n" +
                "    Day     := StrToInt(Copy(DDMMMYYDate,1,2));\r\n" +
                "  except\r\n" +
                "    on E : EConvertError do\r\n" +
                "      raise Exception.Create( Copy(DDMMMYYDate,1,2) + ' is not a valid day' );\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "  try\r\n" +
                "    YearStr := DateTime.AddCenturyToYear( Copy(DDMMMYYDate,6,2) );\r\n" +
                "    Year    := StrToInt( YearStr );\r\n" +
                "  except\r\n" +
                "    on E : EConvertError do\r\n" +
                "      raise Exception.Create( YearStr + ' is not a valid year' );\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "  Month := DateTime.NumericMonth( Copy(DDMMMYYDate,3,3) );\r\n" +
                "\r\n" +
                "  try\r\n" +
                "    Result := EncodeDate( Year, Month, Day);\r\n" +
                "  except\r\n" +
                "    Result := GetUTCDateTime;\r\n" +
                "  end; // try\r\n" +
                "end; // DDMMMYYDateToDateTime\r\n" +
                "\r\n" +
                "function IsFakeDateEnabled: Boolean;\r\n" +
                "begin\r\n" +
                "  Result := FileExists('FakeDate.txt');\r\n" +
                "end;\r\n" +
                "\r\n" +
                "procedure FakeDate;\r\n" +
                "begin\r\n" +
                "  FTestDate := '';\r\n" +
                "  with TStringList.Create do\r\n" +
                "    try\r\n" +
                "      LoadFromFile('FakeDate.txt');\r\n" +
                "      if Count <> 1 then\r\n" +
                "        Exit;\r\n" +
                "\r\n" +
                "      FTestDate := Strings[0];\r\n" +
                "      if not ValidateDate(FTestDate) then\r\n" +
                "      begin\r\n" +
                "        ShowMessageFmt('Error in format (DDMMMYY) of fake date of - \"%s\"'#13#10' from \"%s\\FakeDate.txt\"', [\r\n" +
                "          FTestDate,\r\n" +
                "          GetCurrentDir\r\n" +
                "        ]);\r\n" +
                "        FTestDate := '';\r\n" +
                "      end\r\n" +
                "      else\r\n" +
                "        ShowMessageFmt('Running with fake date of - \"%s\" from \"%s\\FakeDate.txt\"', [\r\n" +
                "          FTestDate,\r\n" +
                "          GetCurrentDir\r\n" +
                "        ]);\r\n" +
                "    finally\r\n" +
                "      Free;\r\n" +
                "    end;\r\n" +
                "end;\r\n" +
                "\r\n" +
                "function DateTimeToNumericDate(const DateTimeIn :TDateTime) :NumericDateType;\r\n" +
                "var\r\n" +
                "  vEagleDate : UniversalDateType;\r\n" +
                "begin\r\n" +
                "  vEagleDate := DateTimetoEagleDate(DateTimeIn);\r\n" +
                "  Result := NumericDate(vEagleDate);\r\n" +
                "end;\r\n" +
                "\r\n" +
                "function NumericDateWithTimeToDateTime(iNumericDateTime: ShortString): TDateTime;\r\n" +
                "var\r\n" +
                "  MessageDate: UniversalDateType;\r\n" +
                "  MessageTime: EagleTimeType;\r\n" +
                "  MessageDateTime: TDateTime;\r\n" +
                "begin\r\n" +
                "  MessageDate := DayADtoDate(DayAD(copy(iNumericDateTime, 1, 4)));\r\n" +
                "  MessageTime := MinutesToTime(ord(iNumericDateTime[5])*60+ord(iNumericDateTime[6]));\r\n" +
                "  if not UniversalDateTimeToTDateTime(MessageDate, MessageTime, MessageDateTime) then\r\n" +
                "    MessageDateTime := Now;\r\n" +
                "  Result := MessageDateTime;\r\n" +
                "end;\r\n" +
                "\r\n" +
                "function DateTimeToNumericDateWithTime(iValue: TDateTime): ShortString;\r\n" +
                "var\r\n" +
                "  vHours, vMinutes, vSeconds, vMillis: Word;\r\n" +
                "begin\r\n" +
                "  DecodeTime(iValue, vHours, vMinutes, vSeconds, vMillis);\r\n" +
                "  Result := DateTimeToNumericDate(iValue) + Chr(vHours) + Chr(vMinutes);\r\n" +
                "end;\r\n" +
                "\r\n" +
                "function YYYYMMDDToTDateTime(iYYYYMMDD: AnsiString): TDateTime;\r\n" +
                "begin\r\n" +
                "  Result := NumericDateToDateTime(YYYYMMDDtoNumericDate(iYYYYMMDD));\r\n" +
                "end;\r\n" +
                "\r\n" +
                "function HHMMToTTime(iHHMM: AnsiString): TDateTime;\r\n" +
                "begin\r\n" +
                "  Result := TimeOf(DDHHMMToTDateTime('01' + iHHMM));\r\n" +
                "end;\r\n" +
                "\r\n" +
                "initialization\r\n" +
                "  // Default to always use UTC time\r\n" +
                "    TimeIsUTCBased(true);\r\n" +
                "{ TODO -oMPL : Copied from DRB DateTime }\r\n" +
                "  coMaxDate := EncodeDate(9999, 12, 31);\r\n" +
                "  if IsFakeDateEnabled then\r\n" +
                "    FakeDate;\r\n" +
                "end. // Unit DateTime\r\n" +
                "\r\n" +
                "\r\n";
        	
        	#endregion

        	TestGetUnits(cDateTime, 
                         new string[] {"SysUtils", "Classes", "Math", "Windows"}, 
                         new string[] {"Mystd", "Forms", "Dialogs", "DateUtils"});
        }
        
        [Test]
        public void GetUnits_Login()
        {
        	#region (Test value)

        	const string cLogin =
                "interface\r\n" +
                "\r\n" +
                "uses\r\n" +
                "  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,\r\n" +
                "  Dialogs,\r\n" +
                "  MYSTD, DATETIME, OPERio, TimeSync, AdminOptionsInfo, ChangePassword,\r\n" +
                "  DRBCryptography, codeio, GDeclare, Errorsio, FILEVIEW, {TraditionalLoginFrm,}\r\n" +
                "  GuggenheimLoginFrm;\r\n" +
                "\r\n" +
                "type\r\n" +
                "//  TDefaultLoginForm = TTraditionalLoginForm;\r\n" +
                "  TDefaultLoginForm = TGuggenheimLoginForm;\r\n" +
                "\r\n" +
                "  TLogin = class(TInterfacedObject)\r\n" +
                "  private\r\n" +
                "    FAdminOptionsInfoRec :TPasswordOptionsInfoRec;\r\n" +
                "    FNoNeedToValidate :Boolean;\r\n" +
                "    FUserID: string[4];\r\n" +
                "    FCheckDPMAIL :boolean;\r\n" +
                "    FirstTime :boolean;\r\n" +
                "    procedure ShowDPMAIL(OperatorRecord: OperatorRecordType);\r\n" +
                "    procedure SetDataPath(const Value: AnsiString);\r\n" +
                "  private\r\n" +
                "    FSigninForm: TForm;\r\n" +
                "    FSigninFormClass: TFormClass;\r\n" +
                "    procedure SetSigninFormClass(const Value: TFormClass);\r\n" +
                "  protected\r\n" +
                "    function SignInFormNeeded: TForm; virtual;\r\n" +
                "    function GetPassword: AnsiString;\r\n" +
                "    function GetCompanyCode: AnsiString;\r\n" +
                "    function GetLocalDisk: AnsiString;\r\n" +
                "    function GetMasterDisk: AnsiString;\r\n" +
                "    function GetMasterExeDisk: AnsiString;\r\n" +
                "    function GetOperatorId: AnsiString;\r\n" +
                "    function GetSystemName: AnsiString;\r\n" +
                "    function GetWorkstation: AnsiString;\r\n" +
                "    function GetDataDisk: AnsiString;\r\n" +
                "    procedure SetPassword(const Value: AnsiString);\r\n" +
                "    procedure SetCompanyCode(const Value: AnsiString);\r\n" +
                "    procedure SetLocalDisk(const Value: AnsiString);\r\n" +
                "    procedure SetMasterDisk(const Value: AnsiString);\r\n" +
                "    procedure SetMasterExeDisk(const Value: AnsiString);\r\n" +
                "    procedure SetOperatorId(const Value: AnsiString);\r\n" +
                "    procedure SetSystemName(const Value: AnsiString);\r\n" +
                "    procedure SetWorkstation(const Value: AnsiString);\r\n" +
                "    procedure SetDataDisk(const Value: AnsiString);\r\n" +
                "    function MissingData: boolean;\r\n" +
                "    function GetDataPath: AnsiString;\r\n" +
                "    function CalcDataPath :ShortString;\r\n" +
                "    procedure FocusField(Field: AnsiString); virtual;\r\n" +
                "    function OtherChecks :boolean; virtual;\r\n" +
                "    function CheckPasswordRules(var OperInfoRec :OPERATORRecordType) :boolean;\r\n" +
                "    procedure ProcessFieldError(Field, ErrorMessage: AnsiString); virtual;\r\n" +
                "    procedure SetCheckDPMAIL(Value: Boolean);\r\n" +
                "    property Password: AnsiString read GetPassword write SetPassword;\r\n" +
                "    property CompanyCode: AnsiString read GetCompanyCode write SetCompanyCode;\r\n" +
                "    property Workstation: AnsiString read GetWorkstation write SetWorkstation;\r\n" +
                "    property MasterExeDisk: AnsiString read GetMasterExeDisk write SetMasterExeDisk;\r\n" +
                "    property MasterDisk: AnsiString read GetMasterDisk write SetMasterDisk;\r\n" +
                "    property LocalDisk: AnsiString read GetLocalDisk write SetLocalDisk;\r\n" +
                "    property SystemName: AnsiString read GetSystemName write SetSystemName;\r\n" +
                "    property OperatorId: AnsiString read GetOperatorId write SetOperatorId;\r\n" +
                "    property DataDisk: AnsiString read GetDataDisk write SetDataDisk;\r\n" +
                "    property NoNeedToValidate: Boolean read FNoNeedToValidate write FNoNeedToValidate;\r\n" +
                "    property DataPath: AnsiString read GetDataPath write SetDataPath;\r\n" +
                "    procedure Initialize;\r\n" +
                "    procedure VerifyPassword;\r\n" +
                "  public\r\n" +
                "    AOK :boolean;\r\n" +
                "    constructor Create(const SigninFormClass: TFormClass = nil);\r\n" +
                "    destructor Destroy; override;\r\n" +
                "    procedure Execute;\r\n" +
                "    function CheckPassword( Password :shortstring;\r\n" +
                "        OperInfoRec :OperatorRecordType;\r\n" +
                "        var PasswordValid :boolean) :ShortString;\r\n" +
                "    property CheckDPMAIL: Boolean read FCheckDPMAIL write SetCheckDPMAIL;\r\n" +
                "    property SigninFormClass: TFormClass read FSigninFormClass write SetSigninFormClass;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "function ShowLoginDialog(DoDPMailCheck: Boolean = False): Boolean;\r\n" +
                "\r\n" +
                "implementation\r\n" +
                "\r\n" +
                "uses\r\n" +
                "  CommonTypes, CommonConsts,\r\n" +
                "  ReceiveMAIL ,\r\n" +
                "  DMGlobals, LoginFrm;\r\n" +
                "\r\n" +
                "{$I ERRMSGS.INC}\r\n" +
                "\r\n" +
                "function ShowLoginDialog(DoDPMailCheck: Boolean = False): Boolean;\r\n" +
                "begin\r\n" +
                "    with TLogin.Create do\r\n" +
                "    try\r\n" +
                "      CheckDPMAIL := DoDPMailCheck;\r\n" +
                "      Execute;\r\n" +
                "      Result := AOK;\r\n" +
                "    finally\r\n" +
                "      Free;\r\n" +
                "    end;\r\n" +
                "end;\r\n" +
                "\r\n" +
                "{ TLogin }\r\n";

        	#endregion

        	TestGetUnits(cLogin,
        	             new string[] {"Windows", "Messages", "SysUtils", "Variants", "Classes", "Graphics", "Controls", 
        	             	"Forms", "Dialogs", "MYSTD", "DATETIME", "OPERio", "TimeSync", "AdminOptionsInfo", "ChangePassword", 
        	             	"DRBCryptography", "codeio", "GDeclare", "Errorsio", "FILEVIEW", "GuggenheimLoginFrm"},
        	             new string[] {"CommonTypes", "CommonConsts", "ReceiveMAIL", "DMGlobals", "LoginFrm"});
        }
        
        [Test]
        public void GetUnits_Mqin_zzz()
        {
        	#region (Test value)
        	
        	const string cMqin_zzz =
                "{*******************************************************************\r\n" +
                "$History: Mqin_zzz.pas $\r\n" +
                "//\r\n" +
                "// *****************  Version 17  *****************\r\n" +
                "// User: 893894       Date: 1/09/06    Time: 2:01p\r\n" +
                "// Updated in $/_Patch_Staging/5.4.0/U\r\n" +
                "// Never call FindClose without corresponding FindFirst. It causes serious\r\n" +
                "// problems because FindClose tries to free some resources that have not\r\n" +
                "// been allocated.\r\n" +
                "//\r\n" +
                "// *****************  Version 15  *****************\r\n" +
                "// User: 899482       Date: 6/01/05    Time: 12:37a\r\n" +
                "// Updated in $/DISPMGR/5.3/U\r\n" +
                "// Remove global declaration of CodeRecord from GDCL_ALL (HAL 57104)\r\n" +
                "//\r\n" +
                "// *****************  Version 13  *****************\r\n" +
                "// User: Dmscm        Date: 8/09/04    Time: 4:23p\r\n" +
                "// Updated in $/DISPMGR/_AllSource/U\r\n" +
                "// ASD Enhancement (ANZ 87743)\r\n" +
                "//\r\n" +
                "// *****************  Version 12  *****************\r\n" +
                "// User: 899469       Date: 4/01/04    Time: 11:59p\r\n" +
                "// Updated in $/DISPMGR/_AllSource/U\r\n" +
                "// Modified GetTransactions to process AMDAR documents (JZA 66709)\r\n" +
                "//\r\n" +
                "// *****************  Version 11  *****************\r\n" +
                "// User: 618382       Date: 3/05/04    Time: 2:51p\r\n" +
                "// Updated in $/DISPMGR/_AllSource/U\r\n" +
                "// Fixed CommDrvr Crash when an .err file exists in EIA SITA format\r\n" +
                "// message(EIA 68049)\r\n" +
                "//\r\n" +
                "// *****************  Version 10  *****************\r\n" +
                "// User: 618382       Date: 2/12/04    Time: 2:23p\r\n" +
                "// Updated in $/DISPMGR/_AllSource/U\r\n" +
                "// Added procedure MoveSitaFileToIATAFolder(68049,77312 EIA)\r\n" +
                "//\r\n" +
                "// *****************  Version 9  *****************\r\n" +
                "// User: 618382       Date: 1/20/04    Time: 3:23p\r\n" +
                "// Updated in $/DISPMGR/_AllSource/U\r\n" +
                "// Changed StrToInt to StrToIntDef(SBG 71732)\r\n" +
                "//\r\n" +
                "// *****************  Version 8  *****************\r\n" +
                "// User: 618382       Date: 10/10/03   Time: 12:59p\r\n" +
                "// Updated in $/DISPMGR/_AllSource/U\r\n" +
                "// Functionality added to do Automatic Schedule load\r\n" +
                "//\r\n" +
                "// *****************  Version 7  *****************\r\n" +
                "// User: 899480       Date: 9/19/03    Time: 6:25p\r\n" +
                "// Updated in $/DISPMGR/_AllSource/U\r\n" +
                "// Remove reference to EAGLE and AIRPATH (DDTS 69217)\r\n" +
                "//\r\n" +
                "// *****************  Version 6  *****************\r\n" +
                "// User: 618382       Date: 7/28/03    Time: 4:52p\r\n" +
                "// Updated in $/AP360T/_AllSource/U\r\n" +
                "// Added EIA SITA message type processing\r\n" +
                "//\r\n" +
                "// *****************  Version 5  *****************\r\n" +
                "// User: 982760       Date: 5/22/03    Time: 12:14a\r\n" +
                "// Updated in $/AP360T/_AllSource/U\r\n" +
                "// Correction to CPA's Email to MSSend.  (2244 CPA)\r\n" +
                "//\r\n" +
                "// *****************  Version 4  *****************\r\n" +
                "// User: 982760       Date: 11/19/02   Time: 7:06p\r\n" +
                "// Updated in $/FP1/_AllSource/U\r\n" +
                "// Merged with V3.6.3 - KB\r\n" +
                "//\r\n" +
                "// *****************  Version 3  *****************\r\n" +
                "// User: 899480       Date: 11/06/02   Time: 5:27p\r\n" +
                "// Updated in $/FP1/_AllSource/U\r\n" +
                "// Add logic to receive Email messages from EmailInPath and\r\n" +
                "// output to EmailOutPath (CPAMerge-SL).\r\n" +
                " *\r\n" +
                " * *****************  Version 2  *****************\r\n" +
                " * User: 982760       Date: 4/24/02    Time: 6:38a\r\n" +
                " * Updated in $/FP1/FDCOMM/U\r\n" +
                " * Merged - KB\r\n" +
                "//\r\n" +
                "// *****************  Version 19  *****************\r\n" +
                "// User: Wek          Date: 10/15/01   Time: 1:37p\r\n" +
                "// Updated in $/GUI/eaglegui/u\r\n" +
                "// Hermes ACARS interface(AWE1635).\r\n" +
                "//\r\n" +
                "// *****************  Version 18  *****************\r\n" +
                "// User: Wek          Date: 4/20/00    Time: 11:46a\r\n" +
                "// Updated in $/GUI/eaglegui/u\r\n" +
                "// Added processing of Canadian NOTAMS. (WJA 694)\r\n" +
                "//\r\n" +
                "// *****************  Version 17  *****************\r\n" +
                "// User: Jmk          Date: 11/30/99   Time: 10:32a\r\n" +
                "// Updated in $/GUI/eaglegui/u\r\n" +
                "// Kerry's 30 Nov 99 update\r\n" +
                "//\r\n" +
                "// *****************  Version 16  *****************\r\n" +
                "// User: Jmk          Date: 7/29/99    Time: 2:02p\r\n" +
                "// Updated in $/GUI/eaglegui/u\r\n" +
                "// Kerry's 29Jul99 update\r\n" +
                "//\r\n" +
                "// *****************  Version 15  *****************\r\n" +
                "// User: Jmk          Date: 7/19/99    Time: 9:26a\r\n" +
                "// Updated in $/GUI/eaglegui/u\r\n" +
                "// Kerry Update 19Jul99\r\n" +
                "//\r\n" +
                "// *****************  Version 14  *****************\r\n" +
                "// User: Jmk          Date: 6/25/99    Time: 11:32a\r\n" +
                "// Updated in $/GUI/eaglegui/u\r\n" +
                "// OAS 25Jun99 Updates from Kerry\r\n" +
                "//\r\n" +
                "// *****************  Version 13  *****************\r\n" +
                "// User: Jmk          Date: 6/02/99    Time: 11:43a\r\n" +
                "// Updated in $/GUI/eaglegui/u\r\n" +
                "// Updates from Kerry 1 jun 99\r\n" +
                "//\r\n" +
                "// *****************  Version 12  *****************\r\n" +
                "// User: Jmk          Date: 5/04/99    Time: 1:50p\r\n" +
                "// Updated in $/GUI/eaglegui/u\r\n" +
                "// OAS changes may 5 1999\r\n" +
                "//\r\n" +
                "// *****************  Version 11  *****************\r\n" +
                "// User: Jmk          Date: 4/20/99    Time: 3:06p\r\n" +
                "// Updated in $/GUI/eaglegui/u\r\n" +
                "// Kerry update apr20 99\r\n" +
                "//\r\n" +
                "// *****************  Version 10  *****************\r\n" +
                "// User: Wek          Date: 2/22/99    Time: 1:07p\r\n" +
                "// Updated in $/GUI/eaglegui/u\r\n" +
                "// OAS Washington Resync\r\n" +
                "//\r\n" +
                "// *****************  Version 9  *****************\r\n" +
                "// User: Wek          Date: 1/07/99    Time: 3:58p\r\n" +
                "// Updated in $/GUI/eaglegui/u\r\n" +
                "// dRb 837 Remove all hardcoding of \\EAGLE\\ and use SystemPath field from\r\n" +
                "// GDECLARE\r\n" +
                "//\r\n" +
                "// *****************  Version 8  *****************\r\n" +
                "// User: Wek          Date: 9/23/98    Time: 2:11p\r\n" +
                "// Updated in $/GUI/eaglegui/u\r\n" +
                "// Ron's Labor Day Update\r\n" +
                "//\r\n" +
                "// *****************  Version 7  *****************\r\n" +
                "// User: Wek          Date: 7/04/98    Time: 6:09p\r\n" +
                "// Updated in $/GUI/eaglegui/u\r\n" +
                "// REG Independence Day Synchronization\r\n" +
                "//\r\n" +
                "// *****************  Version 6  *****************\r\n" +
                "// User: Wek          Date: 5/24/98    Time: 6:32p\r\n" +
                "// Updated in $/GUI/eaglegui/u\r\n" +
                "// Ron's Memorial Day Offering\r\n" +
                "//\r\n" +
                "// *****************  Version 5  *****************\r\n" +
                "// User: Imp          Date: 4/02/98    Time: 4:18p\r\n" +
                "// Updated in $/GUI/eaglegui/u\r\n" +
                "// oas files 4-2-98\r\n" +
                "//\r\n" +
                "// *****************  Version 4  *****************\r\n" +
                "// User: Imp          Date: 2/05/98    Time: 9:53a\r\n" +
                "// Updated in $/GUI/eaglegui/u\r\n" +
                "// RG changes sent 02-05-98\r\n" +
                "//\r\n" +
                "// *****************  Version 3  *****************\r\n" +
                "// User: Shl          Date: 1/08/98    Time: 4:52p\r\n" +
                "// Updated in $/GUI/eaglegui/u\r\n" +
                "// RG-OASNEW Changes sent on 02-01-98\r\n" +
                "//\r\n" +
                "// *****************  Version 2  *****************\r\n" +
                "// User: Imp          Date: 12/16/97   Time: 4:48p\r\n" +
                "// Updated in $/GUI/eaglegui/u\r\n" +
                "// dRb72 Added History stamp\r\n" +
                "*******************************************************************}\r\n" +
                "Unit MQIN_ZZZ;\r\n" +
                "\r\n" +
                "Interface\r\n" +
                "\r\n" +
                "uses\r\n" +
                "    Windows,\r\n" +
                "    Classes,\r\n" +
                "    SysUtils,\r\n" +
                "    Dialogs;\r\n" +
                "\r\n" +
                "\r\n" +
                "{BEGIN COMMENTS\r\n" +
                "\r\n" +
                "TITLE:  System Message Transmitter and Receiver for ANZ teletype messages\r\n" +
                "\r\n" +
                "PURPOSE:\r\n" +
                "\r\n" +
                "History\r\n" +
                "    DATE    WHO VER DESCRIPTION\r\n" +
                "    ======  === === =========================================================\r\n" +
                "    22Sep00 KB       Handle Graphical files in TRAN\r\n" +
                "    21Jul99 KB  2.63 Ignore AFTN Check messages\r\n" +
                "    05Jul99 KB  2.62 Process X25 Sita messages through SITA processing rather than IATA\r\n" +
                "    10Jun99 KB  2.61 If files a Tran Directory needs to go through the Scansmi processing\r\n" +
                "                     in RECV then write them to a SCAN subdirectory of  TranoutPath.\r\n" +
                "    22Apr99 KB  2.60 Ignore zero length TRAN messages.\r\n" +
                "    21Apr99 KB  2.59 Change the way we ensure each part is for the same schedule\r\n" +
                "    12Apr99 KB  2.58 Ansett to use Air NZ format Schedules\r\n" +
                "    31Mar99 KB  2.57 Default TranMsgtype to blank if not 3 chars after the '.'\r\n" +
                "     2Feb99 TD  2.56 Provide for Code Record Specification of the OPMET\r\n" +
                "                     Source Directory\r\n" +
                "    27Jan99 TD  2.55 Extract OPMET Messages and Write as Transactions Out\r\n" +
                "    26Jan99 TD  2.54 Retain last char of Transaction file name for\r\n" +
                "                     Ansett message type identification\r\n" +
                "                     Disable DuplicateMessageCheck for Transactions\r\n" +
                "    01Jan99 KB  2.53 Added CheckCircuitSequenceNo for all circuits. Uses MQIN_ZZZ\r\n" +
                "                     ..../ORIG. alternateCode.\r\n" +
                "    30Dec98 TD  2.53 Use Global Variable 'SystemPath' Instead of ':\\EAGLE\\'\r\n" +
                "    22Jun98 KB  2.52 Removed CheckCarinaCircuitSequenceNo\r\n" +
                "     4Jun98 KB  2.51 Changes for processing Schedule files for the ICOC project\r\n" +
                "     1May98 KB  2.50 If a message has an envelope, strip it off and then process the\r\n" +
                "           message in the format of the enveloped message.\r\n" +
                "    28Apr98 KB  2.49 Don't check for duplicate or add to savemsgs if Sita message\r\n" +
                "                     has echo status of 'S'.\r\n" +
                "    20Apr98 KB  2.48 Allow for multiple input directories.  These are defined in\r\n" +
                "             MQIN_ZZZ code records. eg IATA/ORIG1, IATA/ORIG2, ...\r\n" +
                "     8Apr98 KB  2.47 Use FindOldestFile to process in the correct order\r\n" +
                "    17Mar98 KB  2.46 Changes for filtering out duplicate messages\r\n" +
                "     3Feb98 KB  2.45 Change Code Record CodeReference from RECV_ZZZ to COMMDRVR\r\n" +
                "    11Aug97 TD  2.44 Corection to Error Locating POSWX KeyWord.\r\n" +
                "END COMMENTS}\r\n" +
                "\r\n" +
                "Procedure MQIN_ZZZMainProcedure;\r\n" +
                "\r\n" +
                "Implementation\r\n" +
                "\r\n" +
                "Uses\r\n" +
                "    AMDARMsgIO, CommonConsts, CommonTypes, DMGlobals, GDeclare,\r\n" +
                "    GDCL_ALL, DateTime, MySTD, Codeio, CARINAio, MMATEZZZ, {AFTN Message Mate Handler....}\r\n" +
                "    QIO_AAA, OPMETAAA, POSTLOG, QMSGDESK, QMSGRESP, OASFiles, oCOMMCTL,\r\n" +
                "    FILEUNIT, GATEWYio, CompList, SCHFiles, FileCtrl, FunctionalityProvider,\r\n" +
                "  FunctionalityInterface, FlightWxImagesFunctionalityInterface, MessageTypeRecognizer;\r\n" +
                "\r\n" +
                "Const\r\n" +
                "    ProgramName = 'MQIN_ZZZ';\r\n" +
                "    ProgramVersion = '2.61';\r\n" +
                "    MaxSITAElapsedTime = 5;\r\n" +
                "\r\n";

        	#endregion
        	
        	TestGetUnits(cMqin_zzz,
        	             new string[] {"Windows", "Classes", "SysUtils", "Dialogs"},
        	             new string[] {"AMDARMsgIO", "CommonConsts", "CommonTypes", "DMGlobals", "GDeclare",
        	             	"GDCL_ALL", "DateTime", "MySTD", "Codeio", "CARINAio", "MMATEZZZ", "QIO_AAA", "OPMETAAA",
        	             	"POSTLOG", "QMSGDESK", "QMSGRESP", "OASFiles", "oCOMMCTL", "FILEUNIT", "GATEWYio",
        	             	"CompList", "SCHFiles", "FileCtrl", "FunctionalityProvider", "FunctionalityInterface",
        	             	"FlightWxImagesFunctionalityInterface", "MessageTypeRecognizer"});
        }
        
        [Test]
        public void GetUnits_Log4D()
        {
        	#region (Test value)
        	
        	const string cLog4D =
                "unit Log4D;\r\n" +
                "\r\n" +
                "{\r\n" +
                "  Logging for Delphi.\r\n" +
                "  Based on log4j Java package from Apache\r\n" +
                "  (http://jakarta.apache.org/log4j/docs/index.html).\r\n" +
                "  Currently based on log4j 1.2.8.\r\n" +
                "\r\n" +
                "  Written by Keith Wood (kbwood@iprimus.com.au).\r\n" +
                "  Version 1.0 - 29 April 2001.\r\n" +
                "  Version 1.2 - 9 September 2003.\r\n" +
                "  Version 1.3 - 24 July 2004.\r\n" +
                "  Version 1.4 - 19 January 2007.\r\n" +
                "}\r\n" +
                "\r\n" +
                "interface\r\n" +
                "\r\n" +
                "{$H+}\r\n" +
                "\r\n" +
                "{$IFDEF VER130} { Delphi 5 }\r\n" +
                "{$DEFINE DELPHI5}\r\n" +
                "{$DEFINE DELPHI4_UP}\r\n" +
                "{$DEFINE DELPHI5_UP}\r\n" +
                "{$ENDIF}\r\n" +
                "\r\n" +
                "{$IFDEF VER140} { Delphi 6 }\r\n" +
                "{$DEFINE DELPHI6}\r\n" +
                "{$DEFINE DELPHI4_UP}\r\n" +
                "{$DEFINE DELPHI5_UP}\r\n" +
                "{$DEFINE DELPHI6_UP}\r\n" +
                "{$ENDIF}\r\n" +
                "\r\n" +
                "{$IFDEF VER150} { Delphi 7 }\r\n" +
                "{$DEFINE DELPHI7}\r\n" +
                "{$DEFINE DELPHI4_UP}\r\n" +
                "{$DEFINE DELPHI5_UP}\r\n" +
                "{$DEFINE DELPHI6_UP}\r\n" +
                "{$DEFINE DELPHI7_UP}\r\n" +
                "{$ENDIF}\r\n" +
                "\r\n" +
                "{$IFDEF VER180} { Delphi 2006 }\r\n" +
                "{$DEFINE DELPHI7}\r\n" +
                "{$DEFINE DELPHI4_UP}\r\n" +
                "{$DEFINE DELPHI5_UP}\r\n" +
                "{$DEFINE DELPHI6_UP}\r\n" +
                "{$DEFINE DELPHI7_UP}\r\n" +
                "{$ENDIF}\r\n" +
                "\r\n" +
                "uses\r\n" +
                "  Classes,\r\n" +
                "{$IFDEF LINUX}\r\n" +
                "  SyncObjs,\r\n" +
                "{$ELSE}\r\n" +
                "  Windows, Winsock,\r\n" +
                "{$ENDIF}\r\n" +
                "{$IFDEF DELPHI5_UP}\r\n" +
                "  Contnrs,\r\n" +
                "{$ENDIF}\r\n" +
                "  SysUtils;\r\n" +
                "\r\n" +
                "const\r\n" +
                "  Log4DVersion = '1.4';\r\n" +
                "\r\n" +
                "  { Default pattern string for log output.\r\n" +
                "    Shows the application supplied message. }\r\n" +
                "  DefaultPattern = '%m%n';\r\n" +
                "  { A conversion pattern equivalent to the TTCC layout.\r\n" +
                "    Shows runtime, thread, level, logger, NDC, and message. }\r\n" +
                "  TTCCPattern = '%r [%t] %p %c %x - %m%n';\r\n" +
                "\r\n" +
                "  { Common prefix for option names in an initialisation file.\r\n" +
                "    Note that the search for all option names is case sensitive. }\r\n" +
                "  KeyPrefix = 'log4d';\r\n" +
                "  { Specify the additivity of a logger's appenders. }\r\n" +
                "  AdditiveKey = KeyPrefix + '.additive.';\r\n" +
                "  { Define a named appender. }\r\n" +
                "  AppenderKey = KeyPrefix + '.appender.';\r\n" +
                "  { Nominate a factory to use to generate loggers.\r\n" +
                "    This factory must have been registered with RegisterLoggerFactory.\r\n" +
                "    If none is specified, then the default factory is used. }\r\n" +
                "  LoggerFactoryKey = KeyPrefix + '.loggerFactory';\r\n" +
                "  { Define a new logger, and set its logging level and appenders. }\r\n" +
                "  LoggerKey = KeyPrefix + '.logger.';\r\n" +
                "  { Defining this value as true makes log4d print internal debug\r\n" +
                "    statements to debug output. }\r\n" +
                "  DebugKey = KeyPrefix + '.configDebug';\r\n" +
                "  { Specify the error handler to be used with an appender. }\r\n" +
                "  ErrorHandlerKey = '.errorHandler';\r\n" +
                "  { Specify the filters to be used with an appender. }\r\n" +
                "  FilterKey = '.filter';\r\n" +
                "  { Specify the layout to be used with an appender. }\r\n" +
                "  LayoutKey = '.layout';\r\n" +
                "  { Associate an object renderer with the class to be rendered. }\r\n" +
                "  RendererKey = KeyPrefix + '.renderer.';\r\n" +
                "  { Set the logging level and appenders for the root. }\r\n" +
                "  RootLoggerKey = KeyPrefix + '.rootLogger';\r\n" +
                "  { Set the overall logging level. }\r\n" +
                "  ThresholdKey = KeyPrefix + '.threshold';\r\n" +
                "\r\n" +
                "  { Special level value signifying inherited behaviour. }\r\n" +
                "  InheritedLevel = 'inherited';\r\n" +
                "\r\n" +
                "  { Accept option for TLog*Filter. }\r\n" +
                "  AcceptMatchOpt = 'acceptOnMatch';\r\n" +
                "  { Appending option for TLogFileAppender. }\r\n" +
                "  AppendOpt = 'append';\r\n" +
                "  { Common date format option for layouts. }\r\n" +
                "  DateFormatOpt = 'dateFormat';\r\n" +
                "  { File name option for TLogFileAppender. }\r\n" +
                "  FileNameOpt = 'fileName';\r\n" +
                "  { Case-sensitivity option for TLogStringFilter. }\r\n" +
                "  IgnoreCaseOpt = 'ignoreCase';\r\n" +
                "  { Match string option for TLogLevelMatchFilter and TLogStringFilter. }\r\n" +
                "  MatchOpt = 'match';\r\n" +
                "  { Maximum string option for TLogLevelRangeFilter. }\r\n" +
                "  MaxOpt = 'maximum';\r\n" +
                "  { Minimum string option for TLogLevelRangeFilter. }\r\n" +
                "  MinOpt = 'minimum';\r\n" +
                "  { Pattern option for TLogPatternLayout. }\r\n" +
                "  PatternOpt = 'pattern';\r\n" +
                "  { Title option for TLogHTMLLayout. }\r\n" +
                "  TitleOpt = 'title';\r\n" +
                "\r\n" +
                "type\r\n" +
                "{$IFDEF DELPHI4}\r\n" +
                "  TClassList = TList;\r\n" +
                "  TObjectList = TList;\r\n" +
                "{$ENDIF}\r\n" +
                "\r\n" +
                "{$IFDEF LINUX}\r\n" +
                "  TRTLCriticalSection = TCriticalSection;\r\n" +
                "{$ENDIF}\r\n" +
                "\r\n" +
                "  { Log-specific exceptions. }\r\n" +
                "  ELogException = class(Exception);\r\n" +
                "\r\n" +
                "  { Allow for initialisation of a dynamically created object. }\r\n" +
                "  ILogDynamicCreate = interface(IUnknown)\r\n" +
                "    ['{287DAA34-3A9F-45C6-9417-1B0D4DFAC86C}']\r\n" +
                "    procedure Init;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "  { Get/set arbitrary options on an object. }\r\n" +
                "  ILogOptionHandler = interface(ILogDynamicCreate)\r\n" +
                "    ['{AC1C0E30-2DBF-4C55-9C2E-9A0F1A3E4F58}']\r\n" +
                "    function GetOption(const Name: string): string;\r\n" +
                "    procedure SetOption(const Name, Value: string);\r\n" +
                "    property Options[const Name: string]: string Read GetOption Write SetOption;\r\n" +
                "    procedure OnSetOptionsComplete;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "  { Base class for handling options. }\r\n" +
                "  TLogOptionHandler = class(TInterfacedObject, ILogOptionHandler)\r\n" +
                "  private\r\n" +
                "    FOptions: TStringList;\r\n" +
                "  protected\r\n" +
                "    function GetOption(const Name: string): string; virtual;\r\n" +
                "    procedure SetOption(const Name, Value: string); virtual;\r\n" +
                "    procedure OnSetOptionsComplete; virtual;\r\n" +
                "  public\r\n" +
                "    constructor Create; virtual;\r\n" +
                "    destructor Destroy; override;\r\n" +
                "    property Options[const Name: string]: string Read GetOption Write SetOption;\r\n" +
                "    procedure Init; virtual;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "{ Levels ----------------------------------------------------------------------}\r\n" +
                "\r\n" +
                "    { Levels of messages for logging.\r\n" +
                "      The Level property identifies increasing severity of messages.\r\n" +
                "      All those above or equal to a particular setting are logged. }\r\n" +
                "  TLogLevel = class(TObject)\r\n" +
                "  private\r\n" +
                "    FLevel: integer;\r\n" +
                "    FName:  string;\r\n" +
                "  protected\r\n" +
                "    constructor Create(Name: string; Level: integer);\r\n" +
                "  public\r\n" +
                "    property Level: integer Read FLevel;\r\n" +
                "    property Name: string Read FName;\r\n" +
                "    function IsGreaterOrEqual(LogLevel: TLogLevel): boolean;\r\n" +
                "    { Retrieve a level object given its level. }\r\n" +
                "    class function GetLevel(LogLevel: integer): TLogLevel; overload;\r\n" +
                "    { Retrieve a level object given its level, or default if not valid. }\r\n" +
                "    class function GetLevel(LogLevel: integer; DefaultLevel: TLogLevel): TLogLevel;\r\n" +
                "      overload;\r\n" +
                "    { Retrieve a level object given its name. }\r\n" +
                "    class function GetLevel(Name: string): TLogLevel; overload;\r\n" +
                "    { Retrieve a level object given its name, or default if not valid. }\r\n" +
                "    class function GetLevel(Name: string; DefaultLevel: TLogLevel): TLogLevel;\r\n" +
                "      overload;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "const\r\n" +
                "  { Levels of logging as integer values. }\r\n" +
                "  OffValue   = High(integer);\r\n" +
                "  FatalValue = 50000;\r\n" +
                "  ErrorValue = 40000;\r\n" +
                "  WarnValue  = 30000;\r\n" +
                "  InfoValue  = 20000;\r\n" +
                "  DebugValue = 10000;\r\n" +
                "  AllValue   = Low(integer);\r\n" +
                "\r\n" +
                "var\r\n" +
                "  { Standard levels are automatically created (in decreasing severity):\r\n" +
                "    Off, Fatal, Error, Warn, Info, Debug, All. }\r\n" +
                "  Off:   TLogLevel;\r\n" +
                "  Fatal: TLogLevel;\r\n" +
                "  Error: TLogLevel;\r\n" +
                "  Warn:  TLogLevel;\r\n" +
                "  Info:  TLogLevel;\r\n" +
                "  Debug: TLogLevel;\r\n" +
                "  All:   TLogLevel;\r\n" +
                "\r\n" +
                "{ NDC -------------------------------------------------------------------------}\r\n" +
                "\r\n" +
                "type\r\n" +
                "  { Keep track of the nested diagnostic context (NDC). }\r\n" +
                "  TLogNDC = class(TObject)\r\n" +
                "  private\r\n" +
                "    class function GetNDCIndex: integer;\r\n" +
                "    class function GetThreadId: string;\r\n" +
                "  public\r\n" +
                "    class procedure Clear;\r\n" +
                "    class procedure CloneStack(const Context: TStringList);\r\n" +
                "    class function GetDepth: integer;\r\n" +
                "    class procedure Inherit(const Context: TStringList);\r\n" +
                "    class function Peek: string;\r\n" +
                "    class function Pop: string;\r\n" +
                "    class procedure Push(const Context: string);\r\n" +
                "    class procedure Remove;\r\n" +
                "    class procedure SetMaxDepth(const MaxDepth: integer);\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "{ Events ----------------------------------------------------------------------}\r\n" +
                "\r\n" +
                "  TLogLogger = class;\r\n" +
                "\r\n" +
                "  { An event to be logged. }\r\n" +
                "  TLogEvent = class(TPersistent)\r\n" +
                "  private\r\n" +
                "    FError:     Exception;\r\n" +
                "    FLevel:     TLogLevel;\r\n" +
                "    FLogger:    TLogLogger;\r\n" +
                "    FMessage:   string;\r\n" +
                "    FTimeStamp: TDateTime;\r\n" +
                "    FFileName:  string;\r\n" +
                "    FMethod:    string;\r\n" +
                "    function GetElapsedTime: longint;\r\n" +
                "    function GetErrorClass: string;\r\n" +
                "    function GetErrorMessage: string;\r\n" +
                "    function GetLoggerName: string;\r\n" +
                "    function GetNDC: string;\r\n" +
                "    function GetThreadId: longint;\r\n" +
                "    function GetProcessId: longint;\r\n" +
                "  public\r\n" +
                "    constructor Create(const Logger: TLogLogger; const Level: TLogLevel; const FileName, Method, Message: string; const Err: Exception; const TimeStamp: TDateTime = 0); overload;\r\n" +
                "    constructor Create(const Logger: TLogLogger; const Level: TLogLevel; const FileName, Method: string; const Message: TObject; const Err: Exception; const TimeStamp: TDateTime = 0); overload;\r\n" +
                "    property ElapsedTime: longint Read GetElapsedTime;\r\n" +
                "    property Error: Exception Read FError;\r\n" +
                "    property ErrorClass: string Read GetErrorClass;\r\n" +
                "    property ErrorMessage: string Read GetErrorMessage;\r\n" +
                "    property Level: TLogLevel Read FLevel;\r\n" +
                "    property LoggerName: string Read GetLoggerName;\r\n" +
                "    property Message: string Read FMessage;\r\n" +
                "    property NDC: string Read GetNDC;\r\n" +
                "    property ThreadId: longint Read GetThreadId;\r\n" +
                "    property ProcessId: longint Read GetProcessId;\r\n" +
                "    property TimeStamp: TDateTime Read FTimeStamp;\r\n" +
                "    property FileName: string Read FFileName;\r\n" +
                "    property Method: string Read FMethod;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "{ Logger factory --------------------------------------------------------------}\r\n" +
                "\r\n" +
                "  { Factory for creating loggers. }\r\n" +
                "  ILogLoggerFactory = interface(IUnknown)\r\n" +
                "    ['{AEE5E86C-B708-45B2-BEAD-B370D71CAA2F}']\r\n" +
                "    function MakeNewLoggerInstance(const Name: string): TLogLogger;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "  { Default implementation of a logger factory. }\r\n" +
                "  TLogDefaultLoggerFactory = class(TInterfacedObject, ILogLoggerFactory)\r\n" +
                "  public\r\n" +
                "    function MakeNewLoggerInstance(const Name: string): TLogLogger;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "{ Loggers ---------------------------------------------------------------------}\r\n" +
                "\r\n" +
                "  ILogAppender  = interface;\r\n" +
                "  ILogRenderer  = interface;\r\n" +
                "  TLogHierarchy = class;\r\n" +
                "\r\n" +
                "  { This is the central class in the Log4D package. One of the distinctive\r\n" +
                "    features of Log4D is hierarchical loggers and their evaluation. }\r\n" +
                "  TLogLogger = class(TLogOptionHandler, ILogOptionHandler)\r\n" +
                "  private\r\n" +
                "    FAdditive:  boolean;\r\n" +
                "    FAppenders: TInterfaceList;\r\n" +
                "    FHierarchy: TLogHierarchy;\r\n" +
                "    FLevel:     TLogLevel;\r\n" +
                "    FName:      string;\r\n" +
                "    FParent:    TLogLogger;\r\n" +
                "  protected\r\n" +
                "    FCriticalLogger: TRTLCriticalSection;\r\n" +
                "    procedure CallAppenders(const Event: TLogEvent);\r\n" +
                "    procedure CloseAllAppenders;\r\n" +
                "    function CountAppenders: integer;\r\n" +
                "    procedure DoLog(const LogLevel: TLogLevel; const FileName, Method, Message: string; const Err: Exception = nil); overload; virtual;\r\n" +
                "    procedure DoLog(const LogLevel: TLogLevel; const FileName, Method: string; const Message: TObject; const Err: Exception = nil); overload; virtual;\r\n" +
                "    function GetLevel: TLogLevel; virtual;\r\n" +
                "  public\r\n" +
                "    constructor Create(const Name: string); reintroduce;\r\n" +
                "    destructor Destroy; override;\r\n" +
                "    property Additive: boolean Read FAdditive Write FAdditive;\r\n" +
                "    property Appenders: TInterfaceList Read FAppenders;\r\n" +
                "    property Hierarchy: TLogHierarchy Read FHierarchy Write FHierarchy;\r\n" +
                "    property Level: TLogLevel Read GetLevel Write FLevel;\r\n" +
                "    property Name: string Read FName;\r\n" +
                "    property Parent: TLogLogger Read FParent;\r\n" +
                "    procedure AddAppender(const Appender: ILogAppender);\r\n" +
                "    procedure AssertLog(const Assertion: boolean; const FileName, Method, Message: string); overload;\r\n" +
                "    procedure AssertLog(const Assertion: boolean; const FileName, Method, Message: string; Params: array of const); overload;\r\n" +
                "    procedure AssertLog(const Assertion: boolean; const FileName, Method: string; const Message: TObject); overload;\r\n" +
                "    procedure Debug(const FileName, Method, Message: string; const Err: Exception = nil);\r\n" +
                "      overload; virtual;\r\n" +
                "    procedure Debug(const FileName, Method, Message: string; Params: array of const; const Err: Exception = nil); overload; virtual;\r\n" +
                "    procedure Debug(const FileName, Method: string; const Message: TObject; const Err: Exception = nil); overload; virtual;\r\n" +
                "    procedure Error(const FileName, Method, Message: string; const Err: Exception = nil);\r\n" +
                "      overload; virtual;\r\n" +
                "    procedure Error(const FileName, Method, Message: string; Params: array of const; const Err: Exception = nil); overload; virtual;\r\n" +
                "    procedure Error(const FileName, Method: string; const Message: TObject; const Err: Exception = nil); overload; virtual;\r\n" +
                "    procedure Fatal(const FileName, Method, Message: string; const Err: Exception = nil);\r\n" +
                "      overload; virtual;\r\n" +
                "    procedure Fatal(const FileName, Method, Message: string; Params: array of const; const Err: Exception = nil); overload; virtual;\r\n" +
                "    procedure Fatal(const FileName, Method: string; const Message: TObject; const Err: Exception = nil); overload; virtual;\r\n" +
                "    function GetAppender(const Name: string): ILogAppender;\r\n" +
                "    class function GetLogger(const Clazz: TClass; const Factory: ILogLoggerFactory = nil): TLogLogger; overload;\r\n" +
                "    class function GetLogger(const Name: string; const Factory: ILogLoggerFactory = nil): TLogLogger; overload;\r\n" +
                "    class function GetRootLogger: TLogLogger;\r\n" +
                "    procedure Info(const FileName, Method, Message: string; const Err: Exception = nil);\r\n" +
                "      overload; virtual;\r\n" +
                "    procedure Info(const FileName, Method, Message: string; Params: array of const; const Err: Exception = nil); overload; virtual;\r\n" +
                "    procedure Info(const FileName, Method: string; const Message: TObject; const Err: Exception = nil); overload; virtual;\r\n" +
                "    function IsAppender(const Appender: ILogAppender): boolean;\r\n" +
                "    function IsDebugEnabled: boolean;\r\n" +
                "    function IsEnabledFor(const LogLevel: TLogLevel): boolean;\r\n" +
                "    function IsErrorEnabled: boolean;\r\n" +
                "    function IsFatalEnabled: boolean;\r\n" +
                "    function IsInfoEnabled: boolean;\r\n" +
                "    function IsWarnEnabled: boolean;\r\n" +
                "    procedure LockLogger;\r\n" +
                "    procedure Log(const LogLevel: TLogLevel; const FileName, Method, Message: string; const Err: Exception = nil); overload;\r\n" +
                "    procedure Log(const LogLevel: TLogLevel; const FileName, Method, Message: string; Params: array of const; const Err: Exception = nil); overload;\r\n" +
                "    procedure Log(const LogLevel: TLogLevel; const FileName, Method: string; const Message: TObject; const Err: Exception = nil); overload;\r\n" +
                "    procedure RemoveAllAppenders;\r\n" +
                "    procedure RemoveAppender(const Appender: ILogAppender); overload;\r\n" +
                "    procedure RemoveAppender(const Name: string); overload;\r\n" +
                "    procedure UnlockLogger;\r\n" +
                "    procedure Warn(const FileName, Method, Message: string; const Err: Exception = nil);\r\n" +
                "      overload; virtual;\r\n" +
                "    procedure Warn(const FileName, Method, Message: string; Params: array of const; const Err: Exception = nil); overload; virtual;\r\n" +
                "    procedure Warn(const FileName, Method: string; const Message: TObject; const Err: Exception = nil); overload; virtual;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "  { The specialised root logger - cannot have a nil level. }\r\n" +
                "  TLogRoot = class(TLogLogger)\r\n" +
                "  private\r\n" +
                "    procedure SetLevel(const Level: TLogLevel);\r\n" +
                "  public\r\n" +
                "    constructor Create(const Level: TLogLevel);\r\n" +
                "    property Level: TLogLevel Read GetLevel Write SetLevel;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "  { Specialised logger for internal logging. }\r\n" +
                "  TLogLog = class(TLogLogger)\r\n" +
                "  private\r\n" +
                "    FInternalDebugging: boolean;\r\n" +
                "  protected\r\n" +
                "    procedure DoLog(const LogLevel: TLogLevel; const FileName, Method, Message: string; const Err: Exception); override;\r\n" +
                "    procedure DoLog(const LogLevel: TLogLevel; const FileName, Method: string; const Message: TObject; const Err: Exception); override;\r\n" +
                "  public\r\n" +
                "    constructor Create;\r\n" +
                "    property InternalDebugging: boolean Read FInternalDebugging Write FInternalDebugging;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "{ Hierarchy -------------------------------------------------------------------}\r\n" +
                "\r\n" +
                "  { Listen to events occuring within a hierarchy. }\r\n" +
                "  ILogHierarchyEventListener = interface\r\n" +
                "    ['{A216D50F-B9A5-4871-8EE3-CB55C41E138B}']\r\n" +
                "    procedure AddAppenderEvent(const Logger: TLogLogger; const Appender: ILogAppender);\r\n" +
                "    procedure RemoveAppenderEvent(const Logger: TLogLogger; const Appender: ILogAppender);\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "  { This class is specialised in retreiving loggers by name and\r\n" +
                "    also maintaining the logger hierarchy.\r\n" +
                "\r\n" +
                "    The casual user should not have to deal with this class directly.\r\n" +
                "\r\n" +
                "    The structure of the logger hierachy is maintained by the GetInstance\r\n" +
                "    method. The hierarchy is such that children link to their parent but\r\n" +
                "    parents do not have any pointers to their children. Moreover, loggers\r\n" +
                "    can be instantiated in any order, in particular descendant before ancestor.\r\n" +
                "\r\n" +
                "    In case a descendant is created before a particular ancestor, then it creates\r\n" +
                "    an empty node for the ancestor and adds itself to it. Other descendants\r\n" +
                "    of the same ancestor add themselves to the previously created node. }\r\n" +
                "  TLogHierarchy = class(TObject)\r\n" +
                "  private\r\n" +
                "    FEmittedNoAppenderWarning: boolean;\r\n" +
                "    FListeners: TInterfaceList;\r\n" +
                "    FLoggers:   TStringList;\r\n" +
                "    FRenderedClasses: TClassList;\r\n" +
                "    FRenderers: TInterfaceList;\r\n" +
                "    FRoot:      TLogLogger;\r\n" +
                "    FThreshold: TLogLevel;\r\n" +
                "    procedure SetThreshold(const Level: TLogLevel); overload;\r\n" +
                "    procedure UpdateParent(const Logger: TLogLogger);\r\n" +
                "  protected\r\n" +
                "    FCriticalHierarchy: TRTLCriticalSection;\r\n" +
                "  public\r\n" +
                "    constructor Create(Root: TLogLogger);\r\n" +
                "    destructor Destroy; override;\r\n" +
                "    property Root: TLogLogger Read FRoot;\r\n" +
                "    property Threshold: TLogLevel Read FThreshold Write SetThreshold;\r\n" +
                "    procedure AddHierarchyEventListener(const Listener: ILogHierarchyEventListener);\r\n" +
                "    procedure AddRenderer(RenderedClass: TClass; Renderer: ILogRenderer);\r\n" +
                "    procedure Clear;\r\n" +
                "    procedure EmitNoAppenderWarning(const Logger: TLogLogger);\r\n" +
                "    function Exists(const Name: string): TLogLogger;\r\n" +
                "    procedure FireAppenderEvent(const Adding: boolean; const Logger: TLogLogger; const Appender: ILogAppender);\r\n" +
                "    procedure GetCurrentLoggers(const List: TStringList);\r\n" +
                "    function GetLogger(const Name: string; const Factory: ILogLoggerFactory = nil): TLogLogger;\r\n" +
                "    function GetRenderer(const RenderedClass: TClass): ILogRenderer;\r\n" +
                "    function IsDisabled(const LogLevel: integer): boolean;\r\n" +
                "    procedure RemoveHierarchyEventListener(const Listener: ILogHierarchyEventListener);\r\n" +
                "    procedure ResetConfiguration;\r\n" +
                "    procedure Shutdown;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "{ Layouts ---------------------------------------------------------------------}\r\n" +
                "\r\n" +
                "  { Functional requirements for a layout. }\r\n" +
                "  ILogLayout = interface(ILogOptionHandler)\r\n" +
                "    ['{87FDD680-96D7-45A0-A135-CB88ABAD5519}']\r\n" +
                "    function Format(const Event: TLogEvent): string;\r\n" +
                "    function GetContentType: string;\r\n" +
                "    function GetFooter: string;\r\n" +
                "    function GetHeader: string;\r\n" +
                "    function IgnoresException: boolean;\r\n" +
                "    property ContentType: string Read GetContentType;\r\n" +
                "    property Footer: string Read GetFooter;\r\n" +
                "    property Header: string Read GetHeader;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "  { Abstract base for layouts.\r\n" +
                "    Subclasses must at least override Format.\r\n" +
                "\r\n" +
                "    Accepts the following options:\r\n" +
                "\r\n" +
                "    # Format for date and time stamps, string, optional, defaults to ShortDateFormat\r\n" +
                "    # See FormatDateTime function for more details\r\n" +
                "    log4d.appender.<name>.layout.dateFormat=yyyy/mm/dd hh:nn:ss.zzz\r\n" +
                "  }\r\n" +
                "  TLogCustomLayout = class(TLogOptionHandler, ILogDynamicCreate,\r\n" +
                "    ILogOptionHandler, ILogLayout)\r\n" +
                "  private\r\n" +
                "    FDateFormat: string;\r\n" +
                "  protected\r\n" +
                "    property DateFormat: string Read FDateFormat Write FDateFormat;\r\n" +
                "    function GetContentType: string; virtual;\r\n" +
                "    function GetHeader: string; virtual;\r\n" +
                "    function GetFooter: string; virtual;\r\n" +
                "    procedure SetOption(const Name, Value: string); override;\r\n" +
                "  public\r\n" +
                "    property ContentType: string Read GetContentType;\r\n" +
                "    property Footer: string Read GetFooter;\r\n" +
                "    property Header: string Read GetHeader;\r\n" +
                "    function Format(const Event: TLogEvent): string; virtual; abstract;\r\n" +
                "    function IgnoresException: boolean; virtual;\r\n" +
                "    procedure Init; override;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "  { Basic implementation of a layout. }\r\n" +
                "  TLogSimpleLayout = class(TLogCustomLayout)\r\n" +
                "  public\r\n" +
                "    function Format(const Event: TLogEvent): string; override;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "  { This layout outputs events in a HTML table.\r\n" +
                "\r\n" +
                "    Accepts the following options:\r\n" +
                "\r\n" +
                "    # Title for HTML page, string, optional\r\n" +
                "    log4d.appender.<name>.layout.title=Logging Messages\r\n" +
                "  }\r\n" +
                "  TLogHTMLLayout = class(TLogCustomLayout)\r\n" +
                "  private\r\n" +
                "    FTitle: string;\r\n" +
                "  protected\r\n" +
                "    function GetContentType: string; override;\r\n" +
                "    function GetFooter: string; override;\r\n" +
                "    function GetHeader: string; override;\r\n" +
                "    procedure SetOption(const Name, Value: string); override;\r\n" +
                "  public\r\n" +
                "    property Title: string Read FTitle Write FTitle;\r\n" +
                "    function Format(const Event: TLogEvent): string; override;\r\n" +
                "    function IgnoresException: boolean; override;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "  { Layout based on specified pattern.\r\n" +
                "\r\n" +
                "    Accepts the following options:\r\n" +
                "\r\n" +
                "    # Format for rendering the log event, string, optional, defaults to %m%n\r\n" +
                "    # See comments of Format method for more details\r\n" +
                "    log4d.appender.<name>.layout.pattern=%r [%t] %p %c %x - %m%n\r\n" +
                "  }\r\n" +
                "  TLogPatternLayout = class(TLogCustomLayout)\r\n" +
                "  private\r\n" +
                "    FPattern:      string;\r\n" +
                "    FPatternParts: TStringList;\r\n" +
                "    procedure SetPattern(const Pattern: string);\r\n" +
                "  protected\r\n" +
                "    procedure SetOption(const Name, Value: string); override;\r\n" +
                "  public\r\n" +
                "    constructor Create(const Pattern: string = DefaultPattern); reintroduce;\r\n" +
                "    destructor Destroy; override;\r\n" +
                "    property Pattern: string Read FPattern Write SetPattern;\r\n" +
                "    function Format(const Event: TLogEvent): string; override;\r\n" +
                "    procedure Init; override;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "{ Renderers -------------------------------------------------------------------}\r\n" +
                "\r\n" +
                "  { Renderers transform an object into a string message for display. }\r\n" +
                "  ILogRenderer = interface(ILogOptionHandler)\r\n" +
                "    ['{169B03C6-E2C7-4F62-AD19-17408AB30681}']\r\n" +
                "    function Render(const Message: TObject): string;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "  { Abstract base class for renderers - handles basic option setting.\r\n" +
                "    Subclasses must at least override Render. }\r\n" +
                "  TLogCustomRenderer = class(TLogOptionHandler, ILogDynamicCreate,\r\n" +
                "    ILogOptionHandler, ILogRenderer)\r\n" +
                "  public\r\n" +
                "    function Render(const Message: TObject): string; virtual; abstract;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "{ ErrorHandler ----------------------------------------------------------------}\r\n" +
                "\r\n" +
                "    { Appenders may delegate their error handling to ErrorHandlers.\r\n" +
                "\r\n" +
                "      Error handling is a particularly tedious to get right because by\r\n" +
                "      definition errors are hard to predict and to reproduce. }\r\n" +
                "  ILogErrorHandler = interface(ILogOptionHandler)\r\n" +
                "    ['{B902C52A-5E4E-47A8-B291-BE8E7660F754}']\r\n" +
                "    procedure SetAppender(const Appender: ILogAppender);\r\n" +
                "    procedure SetBackupAppender(const BackupAppender: ILogAppender);\r\n" +
                "    procedure SetLogger(const Logger: TLogLogger);\r\n" +
                "    { The appender for which errors are handled. }\r\n" +
                "    property Appender: ILogAppender Write SetAppender;\r\n" +
                "    { The appender to use in case of failure. }\r\n" +
                "    property BackupAppender: ILogAppender Write SetBackupAppender;\r\n" +
                "    { The logger that the failing appender might be attached to. }\r\n" +
                "    property Logger: TLogLogger Write SetLogger;\r\n" +
                "    { This method prints the error message passed as a parameter. }\r\n" +
                "    procedure Error(const Message: string); overload;\r\n" +
                "    { This method should handle the error. Information about the error\r\n" +
                "      condition is passed a parameter. }\r\n" +
                "    procedure Error(const Message: string; const Err: Exception; const ErrorCode: integer; const Event: TLogEvent = nil); overload;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "  { Abstract base class for error handlers - handles basic option setting.\r\n" +
                "    Subclasses must at least override Error. }\r\n" +
                "  TLogCustomErrorHandler = class(TLogOptionHandler, ILogDynamicCreate,\r\n" +
                "    ILogOptionHandler, ILogErrorHandler)\r\n" +
                "  private\r\n" +
                "    FAppender: ILogAppender;\r\n" +
                "    FBackupAppender: ILogAppender;\r\n" +
                "    FLogger:   TLogLogger;\r\n" +
                "  protected\r\n" +
                "    procedure SetAppender(const Appender: ILogAppender); virtual;\r\n" +
                "    procedure SetBackupAppender(const BackupAppender: ILogAppender); virtual;\r\n" +
                "    procedure SetLogger(const Logger: TLogLogger); virtual;\r\n" +
                "  public\r\n" +
                "    property Appender: ILogAppender Write SetAppender;\r\n" +
                "    property BackupAppender: ILogAppender Write SetBackupAppender;\r\n" +
                "    property Logger: TLogLogger Write SetLogger;\r\n" +
                "    procedure Error(const Message: string); overload; virtual; abstract;\r\n" +
                "    procedure Error(const Message: string; const Err: Exception; const ErrorCode: integer; const Event: TLogEvent = nil); overload;\r\n" +
                "      virtual; abstract;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "  { Fallback on an alternative appender if an error arises. }\r\n" +
                "  TLogFallbackErrorHandler = class(TLogCustomErrorHandler)\r\n" +
                "  private\r\n" +
                "    FLoggers: TObjectList;\r\n" +
                "  protected\r\n" +
                "    procedure SetLogger(const Logger: TLogLogger); override;\r\n" +
                "  public\r\n" +
                "    constructor Create; override;\r\n" +
                "    destructor Destroy; override;\r\n" +
                "    procedure Error(const Message: string); overload; override;\r\n" +
                "    procedure Error(const Message: string; const Err: Exception; const ErrorCode: integer; const Event: TLogEvent = nil); overload;\r\n" +
                "      override;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "  { Displays only the first error sent to it to debugging output. }\r\n" +
                "  TLogOnlyOnceErrorHandler = class(TLogCustomErrorHandler)\r\n" +
                "  private\r\n" +
                "    FSeenError: boolean;\r\n" +
                "  public\r\n" +
                "    procedure Error(const Message: string); overload; override;\r\n" +
                "    procedure Error(const Message: string; const Err: Exception; const ErrorCode: integer; const Event: TLogEvent = nil); overload;\r\n" +
                "      override;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "{ Filters ---------------------------------------------------------------------}\r\n" +
                "\r\n" +
                "  TLogFilterDecision = (fdDeny, fdNeutral, fdAccept);\r\n" +
                "\r\n" +
                "  { Filters can control to a finer degree of detail which messages get logged. }\r\n" +
                "  ILogFilter = interface(ILogOptionHandler)\r\n" +
                "    ['{B28213D7-ACE2-4C44-B820-D9437D44F8DA}']\r\n" +
                "    function Decide(const Event: TLogEvent): TLogFilterDecision;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "  { Abstract base class for filters - handles basic option setting.\r\n" +
                "    Subclasses must at least override Decide.\r\n" +
                "\r\n" +
                "    Accepts the following options:\r\n" +
                "\r\n" +
                "    # Class identification\r\n" +
                "    log4d.appender.<name>.filter1=TLogCustomFilter\r\n" +
                "    # Accept or reject the log event when deciding, Boolean, optional, default true\r\n" +
                "    log4d.appender.<name>.filter1.acceptOnMatch=true\r\n" +
                "  }\r\n" +
                "  TLogCustomFilter = class(TLogOptionHandler, ILogDynamicCreate,\r\n" +
                "    ILogOptionHandler, ILogFilter)\r\n" +
                "  private\r\n" +
                "    FAcceptOnMatch: boolean;\r\n" +
                "  protected\r\n" +
                "    property AcceptOnMatch: boolean Read FAcceptOnMatch Write FAcceptOnMatch;\r\n" +
                "    procedure SetOption(const Name, Value: string); override;\r\n" +
                "  public\r\n" +
                "    constructor Create(const AcceptOnMatch: boolean = True); reintroduce;\r\n" +
                "    function Decide(const Event: TLogEvent): TLogFilterDecision; virtual;\r\n" +
                "      abstract;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "  { Deny all messages. }\r\n" +
                "  TLogDenyAllFilter = class(TLogCustomFilter)\r\n" +
                "  public\r\n" +
                "    property AcceptOnMatch;\r\n" +
                "    function Decide(const Event: TLogEvent): TLogFilterDecision; override;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "  { Filter by the message's level.\r\n" +
                "\r\n" +
                "    Accepts the following options (as well as the standard acceptOnMatch one):\r\n" +
                "\r\n" +
                "    # Class identification\r\n" +
                "    log4d.appender.<name>.filter1=TLogLevelMatchFilter\r\n" +
                "    # Logging level to match on, Level, mandatory\r\n" +
                "    log4d.appender.<name>.filter1.match=warn\r\n" +
                "  }\r\n" +
                "  TLogLevelMatchFilter = class(TLogCustomFilter)\r\n" +
                "  private\r\n" +
                "    FMatchLevel: TLogLevel;\r\n" +
                "  protected\r\n" +
                "    procedure SetOption(const Name, Value: string); override;\r\n" +
                "  public\r\n" +
                "    constructor Create(const MatchLevel: TLogLevel; const AcceptOnMatch: boolean = True); reintroduce;\r\n" +
                "    property AcceptOnMatch;\r\n" +
                "    property MatchLevel: TLogLevel Read FMatchLevel Write FMatchLevel;\r\n" +
                "    function Decide(const Event: TLogEvent): TLogFilterDecision; override;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "  { Filter by the message's level being within a range.\r\n" +
                "\r\n" +
                "    Accepts the following options (as well as the standard acceptOnMatch one):\r\n" +
                "\r\n" +
                "    # Class identification\r\n" +
                "    log4d.appender.<name>.filter1=TLogLevelRangeFilter\r\n" +
                "    # Minimum logging level to match on, Level, mandatory\r\n" +
                "    log4d.appender.<name>.filter1.minimum=warn\r\n" +
                "    # Maximum logging level to match on, Level, mandatory\r\n" +
                "    log4d.appender.<name>.filter1.maximum=error\r\n" +
                "  }\r\n" +
                "  TLogLevelRangeFilter = class(TLogCustomFilter)\r\n" +
                "  private\r\n" +
                "    FMaxLevel: TLogLevel;\r\n" +
                "    FMinLevel: TLogLevel;\r\n" +
                "  protected\r\n" +
                "    procedure SetOption(const Name, Value: string); override;\r\n" +
                "  public\r\n" +
                "    constructor Create(const MaxLevel, MinLevel: TLogLevel; const AcceptOnMatch: boolean = True); reintroduce;\r\n" +
                "    property AcceptOnMatch;\r\n" +
                "    property MaxLevel: TLogLevel Read FMaxLevel Write FMaxLevel;\r\n" +
                "    property MinLevel: TLogLevel Read FMinLevel Write FMinLevel;\r\n" +
                "    function Decide(const Event: TLogEvent): TLogFilterDecision; override;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "  { Filter by text within the message.\r\n" +
                "\r\n" +
                "    Accepts the following options (as well as the standard acceptOnMatch one):\r\n" +
                "\r\n" +
                "    # Class identification\r\n" +
                "    log4d.appender.<name>.filter1=TLogStringFilter\r\n" +
                "    # Text to match on anywhere in message, string, mandatory\r\n" +
                "    log4d.appender.<name>.filter1.match=xyz\r\n" +
                "    # Whether match is case-sensitive, Boolean, optional, defaults to false\r\n" +
                "    log4d.appender.<name>.filter1.ignoreCase=true\r\n" +
                "  }\r\n" +
                "  TLogStringFilter = class(TLogCustomFilter)\r\n" +
                "  private\r\n" +
                "    FIgnoreCase: boolean;\r\n" +
                "    FMatch:      string;\r\n" +
                "  protected\r\n" +
                "    procedure SetOption(const Name, Value: string); override;\r\n" +
                "  public\r\n" +
                "    constructor Create(const Match: string; const IgnoreCase: boolean = False; const AcceptOnMatch: boolean = True); reintroduce;\r\n" +
                "    property AcceptOnMatch;\r\n" +
                "    property IgnoreCase: boolean Read FIgnoreCase Write FIgnoreCase;\r\n" +
                "    property Match: string Read FMatch Write FMatch;\r\n" +
                "    function Decide(const Event: TLogEvent): TLogFilterDecision; override;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "{ Appenders -------------------------------------------------------------------}\r\n" +
                "\r\n" +
                "    { Implement this interface for your own strategies\r\n" +
                "      for printing log statements. }\r\n" +
                "  ILogAppender = interface(ILogOptionHandler)\r\n" +
                "    ['{E1A06EA7-34CA-4DA4-9A8A-C76CF34257AC}']\r\n" +
                "    procedure AddFilter(const Filter: ILogFilter);\r\n" +
                "    procedure Append(const Event: TLogEvent);\r\n" +
                "    procedure Close;\r\n" +
                "    function GetErrorHandler: ILogErrorHandler;\r\n" +
                "    function GetFilters: TInterfaceList;\r\n" +
                "    function GetLayout: ILogLayout;\r\n" +
                "    function GetName: string;\r\n" +
                "    procedure RemoveAllFilters;\r\n" +
                "    procedure RemoveFilter(const Filter: ILogFilter);\r\n" +
                "    function RequiresLayout: boolean;\r\n" +
                "    procedure SetErrorHandler(const ErrorHandler: ILogErrorHandler);\r\n" +
                "    procedure SetLayout(const Layout: ILogLayout);\r\n" +
                "    procedure SetName(const Name: string);\r\n" +
                "    property ErrorHandler: ILogErrorHandler Read GetErrorHandler Write SetErrorHandler;\r\n" +
                "    property Filters: TInterfaceList Read GetFilters;\r\n" +
                "    property Layout: ILogLayout Read GetLayout Write SetLayout;\r\n" +
                "    property Name: string Read GetName Write SetName;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "  { Basic implementation of an appender for printing log statements.\r\n" +
                "    Subclasses should at least override DoAppend(string). }\r\n" +
                "  TLogCustomAppender = class(TLogOptionHandler, ILogDynamicCreate,\r\n" +
                "    ILogOptionHandler, ILogAppender)\r\n" +
                "  private\r\n" +
                "    FClosed:  boolean;\r\n" +
                "    FErrorHandler: ILogErrorHandler;\r\n" +
                "    FFilters: TInterfaceList;\r\n" +
                "    FLayout:  ILogLayout;\r\n" +
                "    FName:    string;\r\n" +
                "    function GetErrorHandler: ILogErrorHandler;\r\n" +
                "    function GetFilters: TInterfaceList;\r\n" +
                "    function GetLayout: ILogLayout;\r\n" +
                "    function GetName: string;\r\n" +
                "    procedure SetErrorHandler(const ErrorHandler: ILogErrorHandler);\r\n" +
                "    procedure SetLayout(const Layout: ILogLayout);\r\n" +
                "    procedure SetName(const Name: string);\r\n" +
                "  protected\r\n" +
                "    FCriticalAppender: TRTLCriticalSection;\r\n" +
                "    function CheckEntryConditions: boolean; virtual;\r\n" +
                "    function CheckFilters(const Event: TLogEvent): boolean; virtual;\r\n" +
                "    procedure DoAppend(const Event: TLogEvent); overload; virtual;\r\n" +
                "    procedure DoAppend(const Message: string); overload; virtual; abstract;\r\n" +
                "    procedure WriteFooter; virtual;\r\n" +
                "    procedure WriteHeader; virtual;\r\n" +
                "  public\r\n" +
                "    constructor Create(const Name: string; const Layout: ILogLayout = nil);\r\n" +
                "      reintroduce; virtual;\r\n" +
                "    destructor Destroy; override;\r\n" +
                "    property ErrorHandler: ILogErrorHandler Read GetErrorHandler Write SetErrorHandler;\r\n" +
                "    property Filters: TInterfaceList Read GetFilters;\r\n" +
                "    property Layout: ILogLayout Read GetLayout Write SetLayout;\r\n" +
                "    property Name: string Read GetName Write SetName;\r\n" +
                "    procedure AddFilter(const Filter: ILogFilter); virtual;\r\n" +
                "    procedure Append(const Event: TLogEvent); virtual;\r\n" +
                "    procedure Close; virtual;\r\n" +
                "    procedure Init; override;\r\n" +
                "    procedure RemoveAllFilters; virtual;\r\n" +
                "    procedure RemoveFilter(const Filter: ILogFilter); virtual;\r\n" +
                "    function RequiresLayout: boolean; virtual;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "  { Discard log messages. }\r\n" +
                "  TLogNullAppender = class(TLogCustomAppender)\r\n" +
                "  protected\r\n" +
                "    procedure DoAppend(const Message: string); override;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "  { Send log messages to debugging output. }\r\n" +
                "  TLogODSAppender = class(TLogCustomAppender)\r\n" +
                "  protected\r\n" +
                "    procedure DoAppend(const Message: string); override;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "  { Send log messages to a stream. }\r\n" +
                "  TLogStreamAppender = class(TLogCustomAppender)\r\n" +
                "  private\r\n" +
                "    FStream: TStream;\r\n" +
                "  protected\r\n" +
                "    procedure DoAppend(const Message: string); override;\r\n" +
                "    procedure DoWrite(const StrStream: TStringStream); virtual;\r\n" +
                "  public\r\n" +
                "    constructor Create(const Name: string; const Stream: TStream; const Layout: ILogLayout = nil); reintroduce; virtual;\r\n" +
                "    destructor Destroy; override;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "  { Send log messages to a file.\r\n" +
                "\r\n" +
                "    Accepts the following options:\r\n" +
                "\r\n" +
                "    # Class identification\r\n" +
                "    log4d.appender.<name>=TLogFileAppender\r\n" +
                "    # Name of the file to write to, string, mandatory\r\n" +
                "    log4d.appender.<name>.fileName=C:\\Logs\\App.log\r\n" +
                "    # Whether to append to file, Boolean, optional, defaults to true\r\n" +
                "    log4d.appender.<name>.append=false\r\n" +
                "  }\r\n" +
                "  TLogFileAppender = class(TLogStreamAppender)\r\n" +
                "  private\r\n" +
                "    FAppend:   boolean;\r\n" +
                "    FFileName: TFileName;\r\n" +
                "    function GetMachineName: String;\r\n" +
                "    function GetExecutableName: String;\r\n" +
                "    function ExpandOption(Data, Option, Value: String): String;\r\n" +
                "    function ExpandOptions(Value: String): String;\r\n" +
                "  protected\r\n" +
                "    procedure SetOption(const Name, Value: string); override;\r\n" +
                "    procedure CreateLogFile;\r\n" +
                "    procedure OnSetOptionsComplete(); override;\r\n" +
                "    procedure OpenLogFile();\r\n" +
                "  public\r\n" +
                "    constructor Create(const Name, FileName: string; const Layout: ILogLayout = nil; const Append: boolean = True);\r\n" +
                "      reintroduce; virtual;\r\n" +
                "    property FileName: TFileName Read FFileName;\r\n" +
                "    property OpenAppend: boolean Read FAppend;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "  TRollingLogFileAppender = class(TLogFileAppender)\r\n" +
                "  private\r\n" +
                "    FMaxSize: int64;\r\n" +
                "    FCount:   integer;\r\n" +
                "    function CanRollFiles: boolean;\r\n" +
                "  protected\r\n" +
                "    procedure SetOption(const Name, Value: string); override;\r\n" +
                "    procedure DoWrite(const StrStream: TStringStream); override;\r\n" +
                "    function ExtractName(const iFileName: string): string;\r\n" +
                "    function ExtractExtension(const iFileName: string): string;\r\n" +
                "    function FindIndex(const iText: string; const iSearchItem: char): longint;\r\n" +
                "    function MakeFileName(const iFileName: string; const iAppendIndex: longint): string;\r\n" +
                "    procedure RollLogFiles();\r\n" +
                "    procedure ChangeFileName(const iFrom, iTo: string);\r\n" +
                "    procedure RemoveFile(const iFileName: string);\r\n" +
                "  public\r\n" +
                "    constructor Create(const Name, FileName: string; const Layout: ILogLayout = nil; const Append: boolean = True); override;\r\n" +
                "    procedure Init; override;\r\n" +
                "    property MaxSize: int64 Read FMaxSize Write FMaxSize;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "{ Configurators ---------------------------------------------------------------}\r\n" +
                "\r\n" +
                "  { Use this class to quickly configure the package. }\r\n" +
                "  TLogBasicConfigurator = class(TObject)\r\n" +
                "  private\r\n" +
                "    FRegistry:      TStringList;\r\n" +
                "    FLoggerFactory: ILogLoggerFactory;\r\n" +
                "  protected\r\n" +
                "    { Used by subclasses to add a renderer\r\n" +
                "      to the hierarchy passed as parameter. }\r\n" +
                "    procedure AddRenderer(const Hierarchy: TLogHierarchy; const RenderedName, RendererName: string);\r\n" +
                "    function AppenderGet(const Name: string): ILogAppender;\r\n" +
                "    procedure AppenderPut(const Appender: ILogAppender);\r\n" +
                "    procedure SetGlobalProps(const Hierarchy: TLogHierarchy; const FactoryClassName, Debug, Threshold: string);\r\n" +
                "  public\r\n" +
                "    constructor Create;\r\n" +
                "    destructor Destroy; override;\r\n" +
                "    { Add appender to the root logger. If no appender is provided,\r\n" +
                "      add a TLogODSAppender that uses TLogPatternLayout with the\r\n" +
                "      TTCCPattern and prints to debugging output for the root logger. }\r\n" +
                "    class procedure Configure(const Appender: ILogAppender = nil);\r\n" +
                "    { Reset the default hierarchy to its default. It is equivalent to calling\r\n" +
                "      Logger.GetDefaultHierarchy.ResetConfiguration.\r\n" +
                "      See TLogHierarchy.ResetConfiguration for more details. }\r\n" +
                "    class procedure ResetConfiguration;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "  { Extends BasicConfigurator to provide configuration from an external file.\r\n" +
                "    See DoConfigure for the expected format.\r\n" +
                "\r\n" +
                "    It is sometimes useful to see how Log4D is reading configuration files.\r\n" +
                "    You can enable Log4D internal logging by defining the log4d.debug variable. }\r\n" +
                "  TLogPropertyConfigurator = class(TLogBasicConfigurator)\r\n" +
                "  protected\r\n" +
                "    procedure ConfigureRootLogger(const Props: TStringList; const Hierarchy: TLogHierarchy);\r\n" +
                "    procedure ParseAdditivityForLogger(const Props: TStringList; const Logger: TLogLogger);\r\n" +
                "    function ParseAppender(const Props: TStringList; const AppenderName: string): ILogAppender;\r\n" +
                "    procedure ParseLoggersAndRenderers(const Props: TStringList; const Hierarchy: TLogHierarchy);\r\n" +
                "    procedure ParseLogger(const Props: TStringList; const Logger: TLogLogger; const Value: string);\r\n" +
                "  public\r\n" +
                "    class procedure Configure(const Filename: string); overload;\r\n" +
                "    class procedure Configure(const Props: TStringList); overload;\r\n" +
                "    procedure DoConfigure(const FileName: string; const Hierarchy: TLogHierarchy);\r\n" +
                "      overload;\r\n" +
                "    procedure DoConfigure(const Props: TStringList; const Hierarchy: TLogHierarchy); overload;\r\n" +
                "  end;\r\n" +
                "\r\n" +
                "{ Register a new appender class. }\r\n" +
                "procedure RegisterAppender(const Appender: TClass);\r\n" +
                "{ Find an appender based on its class name and create a new instance of it. }\r\n" +
                "function FindAppender(const ClassName: string): ILogAppender;\r\n" +
                "\r\n" +
                "{ Register a new logger factory class. }\r\n" +
                "procedure RegisterLoggerFactory(const LoggerFactory: TClass);\r\n" +
                "{ Find a logger factory based on its class name\r\n" +
                "  and create a new instance of it. }\r\n" +
                "function FindLoggerFactory(const ClassName: string): ILogLoggerFactory;\r\n" +
                "\r\n" +
                "{ Register a new error handler class. }\r\n" +
                "procedure RegisterErrorHandler(const ErrorHandler: TClass);\r\n" +
                "{ Find an error handler based on its class name\r\n" +
                "  and create a new instance of it. }\r\n" +
                "function FindErrorHandler(const ClassName: string): ILogErrorHandler;\r\n" +
                "\r\n" +
                "{ Register a new filter class. }\r\n" +
                "procedure RegisterFilter(const Filter: TClass);\r\n" +
                "{ Find a filter based on its class name and create a new instance of it. }\r\n" +
                "function FindFilter(const ClassName: string): ILogFilter;\r\n" +
                "\r\n" +
                "{ Register a new layout class. }\r\n" +
                "procedure RegisterLayout(const Layout: TClass);\r\n" +
                "{ Find a layout based on its class name and create a new instance of it. }\r\n" +
                "function FindLayout(const ClassName: string): ILogLayout;\r\n" +
                "\r\n" +
                "{ Register a new class that can be rendered. }\r\n" +
                "procedure RegisterRendered(const Rendered: TClass);\r\n" +
                "{ Find a rendered class based on its class name and return its class. }\r\n" +
                "function FindRendered(const ClassName: string): TClass;\r\n" +
                "\r\n" +
                "{ Register a new object renderer class. }\r\n" +
                "procedure RegisterRenderer(const Renderer: TClass);\r\n" +
                "{ Find an object renderer based on its class name\r\n" +
                "  and create a new instance of it. }\r\n" +
                "function FindRenderer(const ClassName: string): ILogRenderer;\r\n" +
                "\r\n" +
                "{ Convert string value to a Boolean, with default. }\r\n" +
                "function StrToBool(Value: string; const Default: boolean): boolean;\r\n" +
                "\r\n" +
                "{$IFDEF LINUX}\r\n" +
                "procedure EnterCriticalSection(var CS: TCriticalSection);\r\n" +
                "procedure LeaveCriticalSection(var CS: TCriticalSection);\r\n" +
                "procedure InitializeCriticalSection(var CS: TCriticalSection);\r\n" +
                "procedure DeleteCriticalSection(var CS: TCriticalSection);\r\n" +
                "function GetCurrentThreadID: Integer;\r\n" +
                "procedure OutputDebugString(const S: PChar);\r\n" +
                "{$ENDIF}\r\n" +
                "\r\n" +
                "var\r\n" +
                "  { Default implementation of ILogLoggerFactory }\r\n" +
                "  DefaultLoggerFactory: TLogDefaultLoggerFactory;\r\n" +
                "  { The logging hierarchy }\r\n" +
                "  DefaultHierarchy: TLogHierarchy;\r\n" +
                "  { Internal package logging. }\r\n" +
                "  LogLog: TLogLog;\r\n" +
                "\r\n" +
                "implementation\r\n" +
                "\r\n" +
                "const\r\n" +
                "  CRLF = #13#10;\r\n" +
                "  TAB  = #9;\r\n" +
                "  MilliSecsPerDay = 24 * 60 * 60 * 1000;\r\n" +
                "  DefaultRollingFileSize = 1024 * 1024;\r\n" +
                "\r\n" +
                "\r\n";
        		
        	#endregion
        	
        	TestGetUnits(cLog4D,
        	             new string[] {"Classes", "SyncObjs", "Windows", "Winsock", "Contnrs", "SysUtils"},
        	             new string[] {});
        }
        
        [Test]
        public void GetUnits_FDFormat()
        {
        	#region (Test value)

        	const string cFDFormat =
				"{$O+,F+}\r\n" +
				"\r\n" +
				"interface\r\n" +
				"uses\r\n" +
				"    Windows, SYSUTILS , CLASSES , DIALOGS\r\n" +
				"    , DBunit\r\n" +
				"    , GDeclare\r\n" +
				"    , DateTime\r\n" +
				"    , MYSTD\r\n" +
				"    , STDATMOS\r\n" +
				"    , ERRORSio\r\n" +
				"    , CODEio\r\n" +
				"    , ACATCio\r\n" +
				"    , ACLOGio\r\n" +
				"    , ACWGTSio\r\n" +
				"    , APTio\r\n" +
				"    , Bufferio\r\n" +
				"    , CTFFOTio\r\n" +
				"    , OldCTFFOTio\r\n" +
				"    , FDAPTio\r\n" +
				"    , FDBOOKio\r\n" +
				"    , FDRELio\r\n" +
				"    , FDRMKSio\r\n" +
				"    , FFSIio\r\n" +
				"    , FPCFPio\r\n" +
				"    , FPCFPI\r\n" +
				"    , FPFORMio\r\n" +
				"    , FPGlobal // added to get DispatchPolicyType\r\n" +
				"    , FPLITEio\r\n" +
				"    , FPMASKio\r\n" +
				"    , FPMONio\r\n" +
				"    , FSDLYio\r\n" +
				"    , FFJUMPio\r\n" +
				"    , METW2io\r\n" +
				"    , OPERio\r\n" +
				"    , DPODio\r\n" +
				"    ;\r\n";
        		
        	#endregion
        	
        	TestGetUnits(cFDFormat,
        	             new string[] {"Windows", "SYSUTILS", "CLASSES", "DIALOGS", "DBunit", "GDeclare", "DateTime", 
        	             	"MYSTD", "STDATMOS", "ERRORSio", "CODEio", "ACATCio", "ACLOGio", "ACWGTSio", "APTio", 
        	             	"Bufferio", "CTFFOTio", "OldCTFFOTio", "FDAPTio", "FDBOOKio", "FDRELio", "FDRMKSio", 
        	             	"FFSIio", "FPCFPio", "FPCFPI", "FPFORMio", "FPGlobal", "FPLITEio", "FPMASKio", "FPMONio", 
        	             	"FSDLYio", "FFJUMPio", "METW2io", "OPERio", "DPODio"},
        	             new string[] {});
        }
        
        [Test]
        public void GetUnits_Schedzzz()
        {
        	#region (Test value)

        	const string cSchedzzz =
                "unit SCHEDZZZ;\r\n" +
                "\r\n" +
                "interface\r\n" +
                "uses SYSUTILS\r\n" +
                "    , DBunit {Btrieve Interface                             }\r\n" +
                "    , DateTime {Various Date/time Coversions                  }\r\n" +
                "    , GDeclare {Global Declares and Procedures                }\r\n" +
                "    , GDCL_ALL\r\n" +
                "    , MyStd {Field Manipulation Routines                   }\r\n" +
                "    , MyStdIO {Screen Manipulation Routines                  }\r\n" +
                "    , FLTEdits\r\n" +
                "    , StdEdits {Standards Field Type Edits                    }\r\n" +
                "    {Mouse Routines                                }\r\n" +
                "    , ERRORSio {Retrieve Message for Error Code               }\r\n" +
                "    , ACWGTSio\r\n" +
                "    , FDAPTio\r\n" +
                "    , FSDLYio\r\n" +
                "    , FSDailyu\r\n" +
                "    , CODEIO\r\n" +
                "    , POSTLOG\r\n" +
                "    , oCOMMCTL\r\n" +
                "    , QMSGDESK\r\n" +
                "    , AOPCODio\r\n" +
                "    , FFSIio\r\n" +
                "    , SCHFiles\r\n" +
                "    , Classes\r\n" +
                "    ;\r\n" +
                "\r\n" +
                "{ HISTORY\r\n" +
                "\r\n";

        	#endregion

        	TestGetUnits(cSchedzzz,
        	             new string[] {"SYSUTILS", "DBunit", "DateTime", "GDeclare", "GDCL_ALL", "MyStd", "MyStdIO", "FLTEdits", "StdEdits", "ERRORSio", "ACWGTSio", "FDAPTio", "FSDLYio", "FSDailyu", "CODEIO", "POSTLOG", "oCOMMCTL", "QMSGDESK", "AOPCODio", "FFSIio", "SCHFiles", "Classes"},
        	             new string[] {});
        }
    }
}
