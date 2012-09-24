/*
 * Created by SharpDevelop.
 * User: SG0894652
 * Date: 8/11/2009
 * Time: 2:41 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using NUnit.Framework;

using DelphiProjectHandler;

namespace DelphiProjectHandler.Tests
{
	[TestFixture]
	public class IcarusAnalyzerTests
	{
		protected void CompareUnitCount(SuggestedUnitStructure iList, int iExpectedInterface, int iExpectedImplementation, int iExpectedMovedCount, int iExpectedDeletedCount, string iErrorMsg)
		{
			Assert.AreEqual(iExpectedInterface, iList.Uses.Interface.Count, "Invalid no. of returned interface units. " + iErrorMsg);
			Assert.AreEqual(iExpectedImplementation, iList.Uses.Implementation.Count, "Invalid no. of returned implementation units. " + iErrorMsg);
			Assert.AreEqual(iExpectedMovedCount, iList.MoveToInterface.Count, "Invalid no. of units moved to implementation. " + iErrorMsg);
			Assert.AreEqual(iExpectedDeletedCount, iList.ToDelete.Count, "Invalid no. of deleted units. " + iErrorMsg);
		}
		
		[Test]
		public void Parse()
		{
			#region (Test value)

			const string cReport =
				"****************************************************************************\r\n" +
				"*                             Uses Report for                              *\r\n" +
				"*            D:\\DM\\SG894652_MAIN_CCRC\\DISPMGR\\DPR\\ACARSAPI.DPR             *\r\n" +
				"*                           8/11/2009 1:21:21 AM                           *\r\n" +
				"****************************************************************************\r\n" +
				"\r\n" +
				"Usage:\r\n" +
				"----------------------------------------------------------------------------\r\n" +
				"\r\n" +
				"Module AbsoluteIdUtils uses:\r\n" +
				"\r\n" +
				"  Units used in implementation:\r\n" +
				"\r\n" +
				"      SysUtils in implementation (has initialization, used by unit with init)\r\n" +
				"      StrUtils in implementation (used by unit with init)\r\n" +
				"      gdeclare in implementation (has initialization, used by unit with init)\r\n" +
				"      DBStatusCodes in implementation (used by unit with init)\r\n" +
				"      Aptio in implementation (has initialization, used by unit with init)\r\n" +
				"      Navio in implementation (used by unit with init)\r\n" +
				"      Wptio in implementation (used by unit with init)\r\n" +
				"\r\n" +
				"Module AbstractCacheStreamerClass uses:\r\n" +
				"\r\n" +
				"  Units used in interface:\r\n" +
				"\r\n" +
				"  --> SqlCacheUtils in implementation (used by unit with init)\r\n" +
 				"      Logger in interface (has initialization, used by unit with init)\r\n" +
				"  ==> DBUNIT unnecessary (has initialization, used by unit with init)\r\n" +
				"      ConnectionInfoClass in interface (used by unit with init)\r\n" +
				"  --> DatabaseQueryFactoryClass in implementation (used by unit with init)\r\n" +
				"\r\n" +
				"Module AbstractCacheStreamerClass uses:\r\n" +
				"\r\n" +
				"  Units used in implementation:\r\n" +
				"\r\n" +
				"      Windows source not found\r\n" +
				"      SysUtils in implementation (has initialization, used by unit with init)\r\n" +
				"      StrUtils in implementation (used by unit with init)\r\n" +
				"  ==> ActiveX unnecessary (used by unit with init)\r\n" +
				"      gdeclare in implementation (has initialization, used by unit with init)\r\n" +
				"      DBStatusCodes in implementation (used by unit with init)\r\n" +
				"      Aptio in implementation (has initialization, used by unit with init)\r\n" +
				"      Navio in implementation (used by unit with init)\r\n" +
				"      Wptio in implementation (used by unit with init)\r\n" +
				"Module AbstractTable uses:\r\n" +
				"\r\n" +
				"  Units used in implementation:\r\n" +
				"\r\n" +
				"      SysUtils in implementation (has initialization, used by unit with init)\r\n" +
				"      LoggerCategories in implementation (used by unit with init)\r\n" +
				"\r\n" +
				"Library ACARSAPI uses:\r\n" +
				"\r\n" +
				"  Used units:\r\n" +
				"\r\n" +
				"      Windows source not found\r\n" +
				"  ==> Messages unnecessary (used by unit with init)\r\n" +
				"      SysUtils in implementation (has initialization, used by unit with init)\r\n" +
				"      Classes in implementation (has initialization, used by unit with init)\r\n" +
				"      Controls in implementation (has initialization, used by unit with init)\r\n" +
				"      Dialogs in implementation (has initialization, used by unit with init)\r\n" +
				"  ==> CommonTypes unnecessary (used by unit with init)\r\n" +
				"      DateTime in implementation (has initialization, used by unit with init)\r\n" +
				"      Mystd in implementation (used by unit with init)\r\n" +
				"  ==> DBUNIT unnecessary (has initialization, used by unit with init)\r\n" +
				"      CommonConsts in implementation (used by unit with init)\r\n" +
				"      GDeclare in implementation (has initialization, used by unit with init)\r\n" +
				"      CODEio in implementation (used by unit with init)\r\n" +
				"      ACWGTSio in implementation (used by unit with init)\r\n" +
				"      APTio in implementation (has initialization, used by unit with init)\r\n" +
				"      FltEdits in implementation (has initialization, used by unit with init)\r\n" +
				"      FSDLYio in implementation (has initialization, used by unit with init)\r\n" +
				"      oWait in implementation (used by unit with init)\r\n" +
				"      WINDEV in implementation (has initialization, used by unit with init)\r\n" +
				"\r\n" +
				"Note:\r\n" +
				"Units that are marked as \"unnecessary\" in the project source\r\n" +
				"are not needed in the main code block. However you would\r\n" +
				"normally want to keep them listed anyway, because it tells\r\n" +
				"the Delphi IDE that the unit belongs to the project.\r\n" +
				"\r\n" +
				"Module ActiveX uses:\r\n" +
				"\r\n" +
				"  Units used in interface:\r\n" +
				"\r\n" +
				"      Windows source not found\r\n" +
				"      Messages in interface (used by unit with init)\r\n" +
				"\r\n" +
				"Module ActnList uses:\r\n" +
				"\r\n" +
				"  Units used in interface:\r\n" +
				"\r\n" +
				"      Messages in interface (used by unit with init)\r\n" +
				"      Classes in interface (has initialization, used by unit with init)\r\n" +
				"  ==> Contnrs unnecessary (used by unit with init)\r\n" +
				"      ImgList in interface (used by unit with init)\r\n" +
				"\r\n" +
				"Module ActnList uses:\r\n" +
				"\r\n" +
				"  Units used in implementation:\r\n" +
				"\r\n" +
				"      Windows source not found\r\n" +
				"      SysUtils in implementation (has initialization, used by unit with init)\r\n" +
				"  ==> Graphics unnecessary (has initialization, used by unit with init)\r\n" +
				"      Consts in implementation (used by unit with init)\r\n" +
				"      Menus in implementation (has initialization, used by unit with init)\r\n" +
				"      Controls in implementation (has initialization, used by unit with init)\r\n" +
				"      Forms in implementation (has initialization, used by unit with init)\r\n" +
				"\r\n" +
				"Module ACWGTSBroker uses:\r\n" +
				"\r\n" +
				"  Units used in interface:\r\n" +
				"\r\n" +
				"  --> SysUtils in implementation (has initialization, used by unit with init)\r\n" +
				"  --> Classes in implementation (has initialization, used by unit with init)\r\n" +
				"      Contnrs in interface (used by unit with init)\r\n" +
				"      QueryInterface in interface (used by unit with init)\r\n" +
				"  --> DatabaseQueryFactoryClass in implementation (used by unit with init)\r\n" +
				"  ==> DatabaseProviderFactoryClass unnecessary (has initialization, used by unit with init)\r\n" +
				"      AbstractQuery in interface (used by unit with init)\r\n" +
				"  ==> MetaTableAgentClass unnecessary (has initialization, used by unit with init)\r\n" +
				"      ACWGTSio in interface (used by unit with init)\r\n" +
				"  ==> oConsole unnecessary (used by unit with init)\r\n" +
				"      DMBaseBrokerClass in interface (used by unit with init)\r\n" +
				"  --> DMBaseSQLBrokerClass in implementation (used by unit with init)\r\n" +
				"      DMBaseArchSQLBrokerClass in interface (used by unit with init)\r\n" +
				"\r\n" +
				"Module ACWGTSBroker uses:\r\n" +
				"\r\n" +
				"  Units used in implementation:\r\n" +
				"\r\n" +
				"  ==> Datetime unnecessary (has initialization, used by unit with init)\r\n" +
				"      MyStd in implementation (used by unit with init)\r\n" +
				"      DBUNIT in implementation (has initialization, used by unit with init)\r\n" +
				"      CommonConsts in implementation (used by unit with init)\r\n" +
				"      DMGlobals in implementation (used by unit with init)\r\n" +
				"      gdeclare in implementation (has initialization, used by unit with init)\r\n" +
				"  ==> OracleKeywordClass unnecessary (has initialization, used by unit with init)\r\n" +
				"  ==> DMUtils unnecessary (used by unit with init)\r\n" +
				"      DMBtrvQueryFactory in implementation (has initialization, used by unit with init)\r\n" +
				"      DMSQLQueryFactory in implementation (has initialization, used by unit with init)\r\n" +
				"\r\n" +
				"Module WinSpool uses:\r\n" +
				"\r\n" +
				"  Units used in interface:\r\n" +
				"\r\n" +
				"      Windows source not found\r\n" +
				"\r\n" +
				"Module WPTIO uses:\r\n" +
				"\r\n" +
				"  Units used in interface:\r\n" +
				"\r\n" +
				"  --> Sysutils in implementation (has initialization, used by unit with init)\r\n" +
				"  --> MYSTD in implementation (used by unit with init)\r\n" +
				"  --> DBunit in implementation (has initialization, used by unit with init)\r\n" +
				"      GDeclare in interface (has initialization, used by unit with init)\r\n" +
				"  --> GEODESIC in implementation (used by unit with init)\r\n" +
				"\r\n" +
				"Module WPTIO uses:\r\n" +
				"\r\n" +
				"  Units used in implementation:\r\n" +
				"\r\n" +
				"  ==> CommonTypes unnecessary (used by unit with init)\r\n" +
				"      CommonConsts in implementation (used by unit with init)\r\n" +
				"      DMGlobals in implementation (used by unit with init)\r\n" +
				"      DBOperations in implementation (used by unit with init)\r\n" +
				"      CODEIO in implementation (used by unit with init)\r\n" +
				"\r\n" +
				"\r\n" +
				"\r\n" +
				"\r\n" +
				"Modules that are referenced in the Delphi project file, but not used:\r\n" +
				"----------------------------------------------------------------------------\r\n" +
				"\r\n" +
				"CommonTypes (used by unit with init)\r\n" +
				"DBUNIT (has initialization, used by unit with init)\r\n" +
				"Messages (used by unit with init)\r\n" +
				"\r\n" +
				"Note:\r\n" +
				"Units that are marked as \"unnecessary\" in the project source\r\n" +
				"are not needed in the main code block. However you would\r\n" +
				"normally want to keep them listed anyway, because it tells\r\n" +
				"the Delphi IDE that the unit belongs to the project.\r\n" +
				"\r\n" +
				"\r\n" +
				"Modules that are used but not added to the Delphi project file:\r\n" +
				"----------------------------------------------------------------------------\r\n" +
				"\r\n" +
				"            (none)\r\n" +
				"\r\n" +
				"\r\n" +
				"References:\r\n" +
				"----------------------------------------------------------------------------\r\n" +
				"\r\n" +
				"AbsoluteIdUtils references (7):\r\n" +
				"\r\n" +
				"Aptio, DBStatusCodes, gdeclare, Navio, StrUtils, SysUtils, Wptio\r\n" +
				"\r\n" +
				"AbsoluteIdUtils is referenced by (1):\r\n" +
				"\r\n" +
				"PTEdits\r\n" +
				"\r\n" +
				"Module CommDlg uses:\r\n" +
				"\r\n" +
				"  Units used in interface:\r\n" +
				"\r\n" +
				"      Windows source not found\r\n" +
				"      Messages in interface (used by unit with init)\r\n" +
				"      ShlObj in interface (used by unit with init)\r\n" +
				"\r\n" +
				"Program COMMDRVR uses:\r\n" +
				"\r\n" +
				"  Used units:\r\n" +
				"\r\n" +
				"  ==> ShareMem unnecessary (has initialization)\r\n" +
				"      Forms in implementation (has initialization, used by unit with init)\r\n" +
				"      Logger in implementation (has initialization, used by unit with init)\r\n" +
				"      GDeclare in implementation (has initialization, used by unit with init)\r\n" +
				"  ==> DBUnit unnecessary (has initialization, used by unit with init)\r\n" +
				"  ==> oLOGDISP unnecessary\r\n" +
				"      oLOGCONS in implementation (used by unit with init)\r\n" +
				"      oCOMMLNK in implementation\r\n" +
				"      oCOMMCTL in implementation (used by unit with init)\r\n" +
				"  ==> MessageTypeRecognizer unnecessary (used by unit with init)\r\n" +
				"  ==> GDCL_ALL unnecessary (has initialization, used by unit with init)\r\n" +
				"      BasAbout in implementation\r\n" +
				"  ==> HelpInvoker unnecessary (used by unit with init)\r\n" +
				"  ==> ComCreator unnecessary\r\n" +
				"  ==> AppLauncherInterface unnecessary\r\n" +
				"  ==> AppLauncher unnecessary\r\n" +
				"  ==> RegSvr32Launcher unnecessary\r\n" +
				"  ==> ComObjectLibrary unnecessary (has initialization)\r\n" +
				"  ==> ComCreatorGuiInstallApproval unnecessary\r\n" +
				"  ==> DefaultWebBrowserLauncher unnecessary\r\n" +
				"  ==> UIGlobal unnecessary (has initialization)\r\n" +
				"      oSKEDLST in implementation\r\n" +
				"      oCDMet in implementation\r\n" +
				"  ==> oCDStart unnecessary\r\n" +
				"  ==> SMI unnecessary (has initialization, used by unit with init)\r\n" +
				"  ==> Notamio unnecessary (used by unit with init)\r\n" +
				"  ==> NotamBroker unnecessary (has initialization)\r\n" +
				"  ==> oFWAPSAlert unnecessary (has initialization)\r\n" +
				"  ==> OPMSGio unnecessary (used by unit with init)\r\n" +
				"  ==> EramMessageProcessorInterface unnecessary (used by unit with init)\r\n" +
				"  ==> EramRejectCNLProcessor unnecessary (used by unit with init)\r\n" +
				"  ==> EramFDRelFinder unnecessary\r\n" +
				"  ==> EramCNLAckProcessor unnecessary (used by unit with init)\r\n" +
				"  ==> EramMessageProcessor unnecessary (has initialization)\r\n" +
				"  ==> EramFPLAckProcessor unnecessary (used by unit with init)\r\n" +
				"  ==> EramRejectFPLProcessor unnecessary (used by unit with init)\r\n" +
				"  ==> EramCHGAckProcessor unnecessary (used by unit with init)\r\n" +
				"  ==> EramDEPAckProcessor unnecessary (used by unit with init)\r\n" +
				"  ==> EramDLAAckProcessor unnecessary (used by unit with init)\r\n" +
				"  ==> EramRejectDEPProcessor unnecessary (used by unit with init)\r\n" +
				"  ==> EramRejectDLAProcessor unnecessary (used by unit with init)\r\n" +
				"  ==> EramRejectCHGProcessor unnecessary (used by unit with init)\r\n" +
				"  ==> ERAMConfiguration unnecessary\r\n" +
				"  ==> FunctionalityProvider unnecessary (has initialization, used by unit with init)\r\n" +
				"  ==> FlightWxImagesFunctionality unnecessary (has initialization)\r\n" +
				"  ==> NOTAMContinuityAlerterTypes unnecessary\r\n" +
				"  ==> NOTAMserialNumberAlerterCSN unnecessary\r\n" +
				"  ==> NOTAMContinuityCheckRequester unnecessary\r\n" +
				"  ==> NOTAMContinuityLogger unnecessary\r\n" +
				"  ==> NOTAMCheckFacility unnecessary\r\n" +
				"  ==> WsiFileTypes unnecessary (used by unit with init)\r\n" +
				"  ==> ErrorsysExceptionHandler unnecessary\r\n" +
				"  ==> Nmsg_zzz unnecessary\r\n" +
				"  ==> BulletinProcessors unnecessary\r\n" +
				"  ==> NORviaNOTAMreformatter unnecessary\r\n" +
				"  ==> AutoCorrectList unnecessary\r\n" +
				"  ==> AutoCorrect unnecessary\r\n" +
				"  ==> IncomingOTSrollbackManager unnecessary\r\n" +
				"  ==> IncomingNATMessageProcessor unnecessary\r\n" +
				"  ==> IncomingNORMessageProcessor unnecessary\r\n" +
				"  ==> FLMsgQueue unnecessary\r\n" +
				"  ==> OMSG_ZZZ unnecessary\r\n" +
				"  ==> OPMSGBroker unnecessary (has initialization)\r\n" +
				"      OCOMDRVR in implementation\r\n" +
				"  ==> MQIN_ZZZ unnecessary\r\n" +
				"  ==> RECV_ZZZ unnecessary\r\n" +
				"  ==> FlightPlanReview unnecessary\r\n" +
				"  ==> FlightPlanReviewHandler unnecessary\r\n" +
				"  ==> ETAtoRESsender unnecessary\r\n" +
				"\r\n" +
				"Note:\r\n" +
				"Units that are marked as \"unnecessary\" in the project source\r\n" +
				"are not needed in the main code block. However you would\r\n" +
				"normally want to keep them listed anyway, because it tells\r\n" +
				"the Delphi IDE that the unit belongs to the project.\r\n" +
				"\r\n" +
				"Module COMMGRIB uses:\r\n" +
				"\r\n" +
				"  Units used in interface:\r\n" +
				"\r\n" +
				"  --> SYSUTILS in implementation (has initialization, used by unit with init)\r\n" +
				"  ==> MYSTD unnecessary (used by unit with init)\r\n" +
				"  --> GDeclare in implementation (has initialization, used by unit with init)\r\n" +
				"  ==> DateTime unnecessary (has initialization, used by unit with init)\r\n" +
				"  ==> dbunit unnecessary (has initialization, used by unit with init)\r\n" +
				"  --> QMSGDESK in implementation (used by unit with init)\r\n" +
				"  ==> DYNEXEC unnecessary (used by unit with init)\r\n" +
				"  --> uGRIBSIT in implementation\r\n" +
				"\r\n" +
	            "Module COMMGRIB uses:\r\n" +
	            "\r\n" +
	            "  Units used in implementation:\r\n" +
	            "\r\n" +
	            "  ==> CommonConsts unnecessary (used by unit with init)\r\n" +
	            "  ==> CommonTypes unnecessary (used by unit with init)\r\n";
	
			#endregion
			
			SuggestedUnitStructureList vActual = IcarusAnalyzerReportParser.Parse(cReport);
			Assert.AreEqual(10, vActual.Count, "Invalid no. of unit analyzes returned");
			CompareUnitCount(vActual["absoluteidutils"], 0, 7, 0, 0, "Test 1");
			CompareUnitCount(vActual["abstractcachestreamerclass"], 2, 10, 2, 2, "Test 2");
			CompareUnitCount(vActual["abstracttable"], 0, 2, 0, 0, "Test 3");
			CompareUnitCount(vActual["activex"], 2, 0, 0, 0, "Test 4");
			CompareUnitCount(vActual["actnlist"], 3, 6, 0, 2, "Test 5");
			CompareUnitCount(vActual["acwgtsbroker"], 6, 11, 4, 6, "Test 6");
			CompareUnitCount(vActual["winspool"], 1, 0, 0, 0, "Test 7");
			CompareUnitCount(vActual["wptio"], 1, 8, 4, 1, "Test 8");
			CompareUnitCount(vActual["commdlg"], 3, 0, 0, 0, "Test 9");
			CompareUnitCount(vActual["commgrib"], 0, 4, 4, 6, "Test 10");
		}
	}
}
