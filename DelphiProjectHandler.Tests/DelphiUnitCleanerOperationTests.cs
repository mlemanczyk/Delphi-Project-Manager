/*
 * Created by SharpDevelop.
 * User: SG0894652
 * Date: 8/12/2009
 * Time: 11:23 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using NUnit.Framework;
using DelphiProjectHandler.Operations.Units;

namespace DelphiProjectHandler.Tests
{
	[TestFixture]
	public class DelphiUnitCleanerOperationTests
	{
		#region (Test Entries)

		protected const string cReport =
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
			
		
		protected const string cAbsoluteIdUtils = 
			"unit AbsoluteIdUtils;\r\n" +
			"\r\n" +
			"interface\r\n" +
			"\r\n" +
			"type\r\n" +
			"  TAbsoluteIdUtils = class(TObject)\r\n" +
			"  protected\r\n" +
			"    class procedure AbsId2AirportHemisphere(\r\n" +
			"            const iAbsoluteId: AnsiString;\r\n" +
			"              var ivLatHemisphere: AnsiString;\r\n" +
			"              var ivLonHemisphere: AnsiString);\r\n" +
			"    class procedure AbsId2NavaidHemisphere(\r\n" +
			"            const iAbsoluteId: AnsiString;\r\n" +
			"              var ivLatHemisphere: AnsiString;\r\n" +
			"              var ivLonHemisphere: AnsiString);\r\n" +
			"    class procedure AbsId2WaypointHemisphere(\r\n" +
			"            const iAbsoluteId: AnsiString;\r\n" +
			"              var ivLatHemisphere: AnsiString;\r\n" +
			"              var ivLonHemisphere: AnsiString);\r\n" +
			"    class function HemisphereSign(const iHemisphere: AnsiString): Integer;\r\n" +
			"\r\n" +
			"    class procedure NormalizeCoords(\r\n" +
			"            const iDays  : Char;\r\n" +
			"            const iMins  : Char;\r\n" +
			"              var ivDays : Integer;\r\n" +
			"              var ivMins : Integer;\r\n" +
			"              var ivSecs : Integer);\r\n" +
			"    class procedure CalculateSignForPoint(\r\n" +
			"            const iAbsoluteId : AnsiString;\r\n" +
			"              var ivLatSecs   : Integer;\r\n" +
			"              var ivLonSecs   : Integer);\r\n" +
			"    class function BuildCoord(\r\n" +
			"            const iDays : Integer;\r\n" +
			"            const iMins : Integer;\r\n" +
			"            const iSecs : Integer): Extended;\r\n" +
			"    class procedure AbsId2Seconds(\r\n" +
			"            const iAbsoluteId : AnsiString;\r\n" +
			"              var ivLatSecs   : Integer;\r\n" +
			"              var ivLonSecs   : Integer);\r\n" +
			"    class procedure AbsId2Hemisphere(\r\n" +
			"            const iAbsoluteId    : AnsiString;\r\n" +
			"              var ivLatHemisphere : AnsiString;\r\n" +
			"              var ivLonHemisphere : AnsiString);\r\n" +
			"  public\r\n" +
			"    class procedure AbsId2LatLon(\r\n" +
			"            const iAbsoluteId : AnsiString;\r\n" +
			"              var ivLatitude  : Real;\r\n" +
			"              var ivLongitude : Real);\r\n" +
			"  end;\r\n" +
			"\r\n" +
			"implementation\r\n" +
			"\r\n" +
			"uses\r\n" +
			"  StrUtils, Aptio, Navio, Wptio, DBStatusCodes, gdeclare, SysUtils;\r\n" +
			"\r\n" +
			"{ TAbsoluteIdUtils }\r\n" +
			"\r\n" +
			"class procedure TAbsoluteIdUtils.AbsId2AirportHemisphere(\r\n" +
			"        const iAbsoluteId: AnsiString;\r\n" +
			"          var ivLatHemisphere: AnsiString;\r\n" +
			"          var ivLonHemisphere: AnsiString);\r\n" +
			"var\r\n" +
			"  vAirportRecord :AirportRecordType;\r\n" +
			"begin\r\n" +
			"  if not ((GetAbsAirport(iAbsoluteId, UserRecord.CompanyCode, vAirportRecord) = B_SUCCESS) or\r\n" +
			"     (GetAbsAirport(iAbsoluteId, Master, vAirportRecord) = B_SUCCESS)) then\r\n" +
			"    Exit;\r\n" +
			"\r\n" +
			"  ivLatHemisphere := vAirportRecord.LatitudeHemisphere;\r\n" +
			"  ivLonHemisphere := vAirportRecord.LongitudeHemisphere;\r\n" +
			"end;\r\n" +
			"\r\n" +
			"class procedure TAbsoluteIdUtils.AbsId2Hemisphere(\r\n" +
			"        const iAbsoluteId    : AnsiString;\r\n" +
			"          var ivLatHemisphere : AnsiString;\r\n" +
			"          var ivLonHemisphere : AnsiString);\r\n" +
			"var\r\n" +
			"  vPointType: AnsiString;\r\n" +
			"begin\r\n" +
			"  ivLatHemisphere := '';\r\n" +
			"  ivLonHemisphere := '';\r\n" +
			"  vPointType := copy(iAbsoluteId,6,1);\r\n" +
			"\r\n" +
			"  case AnsiIndexText(vPointType, ['P', 'D', 'E']) of\r\n" +
			"    0: AbsId2AirportHemisphere(iAbsoluteId, ivLatHemisphere, ivLonHemisphere);\r\n" +
			"    1: AbsId2NavaidHemisphere(iAbsoluteId, ivLatHemisphere, ivLonHemisphere);\r\n" +
			"    2: AbsId2WaypointHemisphere(iAbsoluteId, ivLatHemisphere, ivLonHemisphere);\r\n" +
			"  end;\r\n" +
			"end;\r\n" +
			"\r\n";

		protected const string cAbstractCacheStreamerClass =
			"unit AbstractCacheStreamerClass;\r\n" +
            "\r\n" +
            "interface\r\n" +
            "\r\n" +
            "uses\r\n" +
            "  SqlCacheUtils,\r\n" +
			"  DatabaseQueryFactoryClass;\r\n" +
            "\r\n" +
            "type\r\n" +
            "  TAbstractSqlCacheStreamer = class(TObject)\r\n" +
            "  protected\r\n" +
            "    function GetFileName(\r\n" +
            "               const iCacheDir  : AnsiString;\r\n" +
            "               const iTableName : AnsiString;\r\n" +
            "               const iVersion   : AnsiString;\r\n" +
            "               const iExtension : AnsiString): AnsiString;\r\n" +
            "    procedure SaveToFile(const iFileName: AnsiString; const iVersion: AnsiString); virtual; abstract;\r\n" +
            "    procedure DoSave(const iCacheDir: AnsiString; const iTableName: AnsiString; const iVersion: AnsiString);\r\n" +
            "  public\r\n" +
            "  end;\r\n" +
            "\r\n" +
            "implementation\r\n" +
            "\r\n" +
            "uses\r\n" +
            "  Windows,\r\n" +
            "  SysUtils;\r\n" +
            "\r\n" +
            "{ TAbstractSqlCacheStreamer }\r\n" +
            "\r\n" +
            "procedure TAbstractSqlCacheStreamer.DoSave(const iCacheDir: AnsiString; const iTableName: AnsiString; const iVersion: AnsiString);\r\n" +
            "var\r\n" +
            "  vFileExt      : AnsiString;\r\n" +
            "  vTempFileName : AnsiString;\r\n" +
            "  vNewFileName  : AnsiString;\r\n" +
            "begin\r\n" +
            "  // make sure there's a valid directory\r\n" +
            "  Assert(iCacheDir <> '', 'Error: you have to specify a valid path for storing the cache');\r\n" +
            "  // make sure there is no \"Direcrory doesn't exists\" a problem\r\n" +
            "  ForceDirectories(iCacheDir);\r\n" +
            "\r\n" +
            "\r\n";
		
		protected const string cAbstractTable =
			"(************************************************\r\n" +
            "// $History: AbstractTable.pas $\r\n" +
            "//\r\n" +
            "// *****************  Version 3  *****************\r\n" +
            "// User: Mgw          Date: 9/23/05    Time: 9:11a\r\n" +
            "// Updated in $/All/USQL\r\n" +
            "// Added/modified logging\r\n" +
            "//\r\n" +
            "// *****************  Version 2  *****************\r\n" +
            "// User: Mgw          Date: 6/06/05    Time: 10:57a\r\n" +
            "// Updated in $/All/USQL\r\n" +
            "// Connection Pooler (2nd version)\r\n" +
            "//\r\n" +
            "************************************************)\r\n" +
            "unit AbstractTable;\r\n" +
            "\r\n" +
            "interface\r\n" +
            "\r\n" +
            "uses\r\n" +
            "  Logger,\r\n" +
            "	DB;\r\n" +
            "\r\n" +
            "type\r\n" +
            "	TAbstractTable = class(TObject)\r\n" +
            "  protected\r\n" +
            "  	fTheTable : TDataSet;\r\n" +
            "    FIsOracle : boolean;\r\n" +
            "    FLogger   : TLogger;\r\n" +
            "    function getTableName: string; virtual; abstract;\r\n" +
            "    procedure setTableName(const Value: string); virtual; abstract;\r\n" +
            "    function GetMasterSource: TDataSource; virtual; abstract;\r\n" +
            "    procedure SetMasterSource(const Value: TDataSource); virtual; abstract;\r\n" +
            "    function GetMasterFields: AnsiString; virtual; abstract;\r\n" +
            "    procedure SetMasterFields(const Value: AnsiString); virtual; abstract;\r\n" +
            "    function GetDetailFields: AnsiString; virtual;\r\n" +
            "    procedure SetDetailFields(const Value: AnsiString); virtual;\r\n" +
            "  public\r\n" +
            "  	Constructor Create (const iConnection : TCustomConnection ); virtual;\r\n" +
            "    Destructor Destroy; override;\r\n" +
            "//!    procedure SetIndexFields(const iIndexName : shortstring; const IndexMetaData : TIndexMetaData); virtual; abstract;\r\n" +
            "    procedure RefreshAfterPost; virtual;\r\n" +
            "    procedure SetDateFilter( const FieldNameIn : AnsiString;\r\n" +
            "                             const StartDateIn : TDateTime;\r\n" +
            "                             const EndDateIn   : TDateTime ); virtual;\r\n" +
            "    procedure SetFilterWithVersion( const FilterClauseIn : AnsiString );\r\n" +
            "    procedure RefreshDataSet; virtual;\r\n" +
            "    procedure FindNearest(const KeyValues: array of const); virtual;\r\n" +
            "    function GetIndexFieldCount : integer; virtual;\r\n" +
            "    property DataSet : TDataSet read fTheTable;\r\n" +
            "    property TableName : string read getTableName write setTableName;\r\n" +
            "    property MasterSource : TDataSource read GetMasterSource write SetMasterSource;\r\n" +
            "    property MasterFields : AnsiString read GetMasterFields write SetMasterFields;\r\n" +
            "    property DetailFields : AnsiString read GetDetailFields write SetDetailFields;\r\n" +
            "    property IsOracle : boolean read FIsOracle write FIsOracle;\r\n" +
            "  end;\r\n" +
            "\r\n" +
            "implementation\r\n" +
            "\r\n" +
            "uses\r\n" +
            "  LoggerCategories,\r\n" +
            "  SysUtils;\r\n" +
            "\r\n" +
            "const\r\n" +
            "  FILENAME = 'AbstractTable';\r\n" +
            "\r\n" +
            "{ TAbstractTable }\r\n" +
            "\r\n" +
            "constructor TAbstractTable.Create(const iConnection: TCustomConnection);\r\n" +
            "begin\r\n" +
            "	inherited Create;\r\n" +
            "  FLogger := TLogger.Create(LC_HELPER);\r\n" +
            "end;\r\n" +
            "\r\n";
		
		protected const string cActiveX =
            "{ *********************************************************************** }\r\n" +
            "{                                                                         }\r\n" +
            "{ Delphi Runtime Library                                                  }\r\n" +
            "{                                                                         }\r\n" +
            "{ Copyright (c) 1997-2001 Borland Software Corporation                    }\r\n" +
            "{                                                                         }\r\n" +
            "{ *********************************************************************** }\r\n" +
            "\r\n" +
            "{ ActiveX / OLE 2 Interface Unit }\r\n" +
            "\r\n" +
            "unit ActiveX;\r\n" +
            "\r\n" +
            "interface\r\n" +
            "\r\n" +
            "uses Windows, Messages;\r\n" +
            "\r\n" +
            "(*$HPPEMIT '' *)\r\n" +
            "(*$HPPEMIT '#include <olectl.h>' *)\r\n" +
            "(*$HPPEMIT '#include <docobj.H>' *)\r\n" +
            "(*$HPPEMIT '#include <oleauto.h>' *)\r\n" +
            "(*$HPPEMIT '#include <propidl.h>' *) // WIN2K\r\n" +
            "(*$HPPEMIT '' *)\r\n" +
            "(*$HPPEMIT '' *)\r\n" +
            "\r\n" +
            "{ Do not WEAKPACKAGE this unit.\r\n" +
            "  This unit requires startup code to initialize constants. }\r\n" +
            "\r\n" +
            "const\r\n" +
            "\r\n" +
            "{ from WTYPES.H }\r\n" +
            "  {$EXTERNALSYM MEMCTX_TASK}\r\n" +
            "  MEMCTX_TASK      = 1;\r\n" +
            "  {$EXTERNALSYM MEMCTX_SHARED}\r\n" +
            "  MEMCTX_SHARED    = 2;\r\n" +
            "  {$EXTERNALSYM MEMCTX_MACSYSTEM}\r\n" +
            "  MEMCTX_MACSYSTEM = 3;\r\n" +
            "  {$EXTERNALSYM MEMCTX_UNKNOWN}\r\n" +
            "  MEMCTX_UNKNOWN   = -1;\r\n" +
            "  {$EXTERNALSYM MEMCTX_SAME}\r\n" +
            "  MEMCTX_SAME      = -2;\r\n" +
            "\r\n";
		
		protected const string cActnList =
            "\r\n" +
            "{*******************************************************}\r\n" +
            "{                                                       }\r\n" +
            "{       Borland Delphi Visual Component Library         }\r\n" +
            "{                                                       }\r\n" +
            "{  Copyright (c) 1999-2001 Borland Software Corporation }\r\n" +
            "{                                                       }\r\n" +
            "{*******************************************************}\r\n" +
            "\r\n" +
            "unit ActnList;\r\n" +
            "\r\n" +
            "{$T-,H+,X+}\r\n" +
            "\r\n" +
            "interface\r\n" +
            "\r\n" +
            "uses Classes, Messages, ImgList, Contnrs;\r\n" +
            "\r\n" +
            "type\r\n" +
            "\r\n" +
            "{ TContainedAction }\r\n" +
            "\r\n" +
            "  TCustomActionList = class;\r\n" +
            "\r\n" +
            "  TContainedAction = class(TBasicAction)\r\n" +
            "  private\r\n" +
            "    FCategory: string;\r\n" +
            "    FActionList: TCustomActionList;\r\n" +
            "    function GetIndex: Integer;\r\n" +
            "    function IsCategoryStored: Boolean;\r\n" +
            "    procedure SetCategory(const Value: string);\r\n" +
            "    procedure SetIndex(Value: Integer);\r\n" +
            "    procedure SetActionList(AActionList: TCustomActionList);\r\n" +
            "  protected\r\n" +
            "    procedure ReadState(Reader: TReader); override;\r\n" +
            "    procedure SetParentComponent(AParent: TComponent); override;\r\n" +
            "  public\r\n" +
            "    destructor Destroy; override;\r\n" +
            "    function Execute: Boolean; override;\r\n" +
            "    function GetParentComponent: TComponent; override;\r\n" +
            "    function HasParent: Boolean; override;\r\n" +
            "    function Update: Boolean; override;\r\n" +
            "    property ActionList: TCustomActionList read FActionList write SetActionList;\r\n" +
            "    property Index: Integer read GetIndex write SetIndex stored False;\r\n" +
            "  published\r\n" +
            "    property Category: string read FCategory write SetCategory stored IsCategoryStored;\r\n" +
            "  end;\r\n" +
            "\r\n" +
            "  TContainedActionClass = class of TContainedAction;\r\n" +
            "\r\n" +
            "{ TCustomActionList }\r\n" +
            "\r\n" +
            "  TActionEvent = procedure (Action: TBasicAction; var Handled: Boolean) of object;\r\n" +
            "  TActionListState = (asNormal, asSuspended, asSuspendedEnabled);\r\n" +
            "\r\n" +
            "  TCustomActionList = class(TComponent)\r\n" +
            "  private\r\n" +
            "    FActions: TList;\r\n" +
            "    FImageChangeLink: TChangeLink;\r\n" +
            "    FImages: TCustomImageList;\r\n" +
            "    FOnChange: TNotifyEvent;\r\n" +
            "    FOnExecute: TActionEvent;\r\n" +
            "    FOnUpdate: TActionEvent;\r\n" +
            "    FState: TActionListState;\r\n" +
            "    function GetAction(Index: Integer): TContainedAction;\r\n" +
            "    function GetActionCount: Integer;\r\n" +
            "    procedure ImageListChange(Sender: TObject);\r\n" +
            "    procedure SetAction(Index: Integer; Value: TContainedAction);\r\n" +
            "    procedure SetState(const Value: TActionListState);\r\n" +
            "  protected\r\n" +
            "    procedure AddAction(Action: TContainedAction);\r\n" +
            "    procedure RemoveAction(Action: TContainedAction);\r\n" +
            "    procedure Change; virtual;\r\n" +
            "    procedure GetChildren(Proc: TGetChildProc; Root: TComponent); override;\r\n" +
            "    procedure Notification(AComponent: TComponent;\r\n" +
            "      Operation: TOperation); override;\r\n" +
            "    procedure SetChildOrder(Component: TComponent; Order: Integer); override;\r\n" +
            "    procedure SetImages(Value: TCustomImageList); virtual;\r\n" +
            "    property OnChange: TNotifyEvent read FOnChange write FOnChange;\r\n" +
            "    property OnExecute: TActionEvent read FOnExecute write FOnExecute;\r\n" +
            "    property OnUpdate: TActionEvent read FOnUpdate write FOnUpdate;\r\n" +
            "  public\r\n" +
            "    constructor Create(AOwner: TComponent); override;\r\n" +
            "    destructor Destroy; override;\r\n" +
            "    function ExecuteAction(Action: TBasicAction): Boolean; override;\r\n" +
            "    function IsShortCut(var Message: TWMKey): Boolean;\r\n" +
            "    function UpdateAction(Action: TBasicAction): Boolean; override;\r\n" +
            "    property Actions[Index: Integer]: TContainedAction read GetAction write SetAction; default;\r\n" +
            "    property ActionCount: Integer read GetActionCount;\r\n" +
            "    property Images: TCustomImageList read FImages write SetImages;\r\n" +
            "    property State: TActionListState read FState write SetState default asNormal;\r\n" +
            "  end;\r\n" +
            "\r\n" +
            "{ TActionList }\r\n" +
            "\r\n" +
            "  TActionList = class(TCustomActionList)\r\n" +
            "  published\r\n" +
            "    property Images;\r\n" +
            "    property State;\r\n" +
            "    property OnChange;\r\n" +
            "    property OnExecute;\r\n" +
            "    property OnUpdate;\r\n" +
            "  end;\r\n" +
            "\r\n" +
            "{ TShortCutList }\r\n" +
            "\r\n" +
            "  TShortCutList = class(TStringList)\r\n" +
            "  private\r\n" +
            "    function GetShortCuts(Index: Integer): TShortCut;\r\n" +
            "  public\r\n" +
            "    function Add(const S: String): Integer; override;\r\n" +
            "    function IndexOfShortCut(const Shortcut: TShortCut): Integer;\r\n" +
            "    property ShortCuts[Index: Integer]: TShortCut read GetShortCuts;\r\n" +
            "  end;\r\n" +
            "\r\n" +
            "{ TControlAction }\r\n" +
            "\r\n" +
            "  THintEvent = procedure (var HintStr: string; var CanShow: Boolean) of object;\r\n" +
            "\r\n" +
            "  TCustomAction = class(TContainedAction)\r\n" +
            "  private\r\n" +
            "    FDisableIfNoHandler: Boolean;\r\n" +
            "    FCaption: string;\r\n" +
            "    FChecking: Boolean;\r\n" +
            "    FChecked: Boolean;\r\n" +
            "    FEnabled: Boolean;\r\n" +
            "    FGroupIndex: Integer;\r\n" +
            "    FHelpType: THelpType;\r\n" +
            "    FHelpContext: THelpContext;\r\n" +
            "    FHelpKeyword: string;\r\n" +
            "    FHint: string;\r\n" +
            "    FImageIndex: TImageIndex;\r\n" +
            "    FShortCut: TShortCut;\r\n" +
            "    FVisible: Boolean;\r\n" +
            "    FOnHint: THintEvent;\r\n" +
            "    FSecondaryShortCuts: TShortCutList;\r\n" +
            "    FSavedEnabledState: Boolean;\r\n" +
            "    FAutoCheck: Boolean;\r\n" +
            "    procedure SetAutoCheck(Value: Boolean);\r\n" +
            "    procedure SetCaption(const Value: string);\r\n" +
            "    procedure SetChecked(Value: Boolean);\r\n" +
            "    procedure SetEnabled(Value: Boolean);\r\n" +
            "    procedure SetGroupIndex(const Value: Integer);\r\n" +
            "    procedure SetHelpContext(Value: THelpContext); virtual;\r\n" +
            "    procedure SetHelpKeyword(const Value: string); virtual;\r\n" +
            "    procedure SetHelpType(Value: THelpType);\r\n" +
            "    procedure SetHint(const Value: string);\r\n" +
            "    procedure SetImageIndex(Value: TImageIndex);\r\n" +
            "    procedure SetShortCut(Value: TShortCut);\r\n" +
            "    procedure SetVisible(Value: Boolean);\r\n" +
            "    function GetSecondaryShortCuts: TShortCutList;\r\n" +
            "    procedure SetSecondaryShortCuts(const Value: TShortCutList);\r\n" +
            "    function IsSecondaryShortCutsStored: Boolean;\r\n" +
            "  protected\r\n" +
            "    FImage: TObject;\r\n" +
            "    FMask: TObject;\r\n" +
            "    procedure AssignTo(Dest: TPersistent); override;\r\n" +
            "    procedure SetName(const Value: TComponentName); override;\r\n" +
            "    function HandleShortCut: Boolean; virtual;\r\n" +
            "    property SavedEnabledState: Boolean read FSavedEnabledState write FSavedEnabledState;\r\n" +
            "  public\r\n" +
            "    constructor Create(AOwner: TComponent); override;\r\n" +
            "    destructor Destroy; override;\r\n" +
            "    function DoHint(var HintStr: string): Boolean; dynamic;\r\n" +
            "    function Execute: Boolean; override;\r\n" +
            "    property AutoCheck: Boolean read FAutoCheck write  SetAutoCheck default False;\r\n" +
            "    property Caption: string read FCaption write SetCaption;\r\n" +
            "    property Checked: Boolean read FChecked write SetChecked default False;\r\n" +
            "    property DisableIfNoHandler: Boolean read FDisableIfNoHandler write FDisableIfNoHandler default False;\r\n" +
            "    property Enabled: Boolean read FEnabled write SetEnabled default True;\r\n" +
            "    property GroupIndex: Integer read FGroupIndex write SetGroupIndex default 0;\r\n" +
            "    property HelpContext: THelpContext read FHelpContext write SetHelpContext default 0;\r\n" +
            "    property HelpKeyword: string read FHelpKeyword write SetHelpKeyword;\r\n" +
            "    property HelpType: THelpType read FHelpType write SetHelpType default htKeyword;\r\n" +
            "    property Hint: string read FHint write SetHint;\r\n" +
            "    property ImageIndex: TImageIndex read FImageIndex write SetImageIndex default -1;\r\n" +
            "    property ShortCut: TShortCut read FShortCut write SetShortCut default 0;\r\n" +
            "    property SecondaryShortCuts: TShortCutList read GetSecondaryShortCuts\r\n" +
            "      write SetSecondaryShortCuts stored IsSecondaryShortCutsStored;\r\n" +
            "    property Visible: Boolean read FVisible write SetVisible default True;\r\n" +
            "    property OnHint: THintEvent read FOnHint write FOnHint;\r\n" +
            "  end;\r\n" +
            "\r\n" +
            "  TAction = class(TCustomAction)\r\n" +
            "  public\r\n" +
            "    constructor Create(AOwner: TComponent); override;\r\n" +
            "  published\r\n" +
            "    property AutoCheck;\r\n" +
            "    property Caption;\r\n" +
            "    property Checked;\r\n" +
            "    property Enabled;\r\n" +
            "    property GroupIndex;\r\n" +
            "    property HelpContext;\r\n" +
            "    property HelpKeyword;\r\n" +
            "    property HelpType;\r\n" +
            "    property Hint;\r\n" +
            "    property ImageIndex;\r\n" +
            "    property ShortCut;\r\n" +
            "    property SecondaryShortCuts;\r\n" +
            "    property Visible;\r\n" +
            "    property OnExecute;\r\n" +
            "    property OnHint;\r\n" +
            "    property OnUpdate;\r\n" +
            "  end;\r\n" +
            "\r\n" +
            "{ TActionLink }\r\n" +
            "\r\n" +
            "  TActionLink = class(TBasicActionLink)\r\n" +
            "  protected\r\n" +
            "    function IsCaptionLinked: Boolean; virtual;\r\n" +
            "    function IsCheckedLinked: Boolean; virtual;\r\n" +
            "    function IsEnabledLinked: Boolean; virtual;\r\n" +
            "    function IsGroupIndexLinked: Boolean; virtual;\r\n" +
            "    function IsHelpContextLinked: Boolean; virtual;\r\n" +
            "    function IsHelpLinked: Boolean; virtual;\r\n" +
            "    function IsHintLinked: Boolean; virtual;\r\n" +
            "    function IsImageIndexLinked: Boolean; virtual;\r\n" +
            "    function IsShortCutLinked: Boolean; virtual;\r\n" +
            "    function IsVisibleLinked: Boolean; virtual;\r\n" +
            "    procedure SetAutoCheck(Value: Boolean); virtual;\r\n" +
            "    procedure SetCaption(const Value: string); virtual;\r\n" +
            "    procedure SetChecked(Value: Boolean); virtual;\r\n" +
            "    procedure SetEnabled(Value: Boolean); virtual;\r\n" +
            "    procedure SetGroupIndex(Value: Integer); virtual;\r\n" +
            "    procedure SetHelpContext(Value: THelpContext); virtual;\r\n" +
            "    procedure SetHelpKeyword(const Value: string); virtual;\r\n" +
            "    procedure SetHelpType(Value: THelpType); virtual;\r\n" +
            "    procedure SetHint(const Value: string); virtual;\r\n" +
            "    procedure SetImageIndex(Value: Integer); virtual;\r\n" +
            "    procedure SetShortCut(Value: TShortCut); virtual;\r\n" +
            "    procedure SetVisible(Value: Boolean); virtual;\r\n" +
            "  end;\r\n" +
            "\r\n" +
            "  TActionLinkClass = class of TActionLink;\r\n" +
            "\r\n" +
            "{ Action registration }\r\n" +
            "\r\n" +
            "  TEnumActionProc = procedure (const Category: string; ActionClass: TBasicActionClass;\r\n" +
            "    Info: Pointer) of object;\r\n" +
            "\r\n" +
            "procedure RegisterActions(const CategoryName: string;\r\n" +
            "  const AClasses: array of TBasicActionClass; Resource: TComponentClass);\r\n" +
            "procedure UnRegisterActions(const AClasses: array of TBasicActionClass);\r\n" +
            "procedure EnumRegisteredActions(Proc: TEnumActionProc; Info: Pointer);\r\n" +
            "function CreateAction(AOwner: TComponent; ActionClass: TBasicActionClass): TBasicAction;\r\n" +
            "\r\n" +
            "const\r\n" +
            "  RegisterActionsProc: procedure (const CategoryName: string;\r\n" +
            "    const AClasses: array of TBasicActionClass; Resource: TComponentClass) = nil;\r\n" +
            "  UnRegisterActionsProc: procedure (const AClasses: array of TBasicActionClass) = nil;\r\n" +
            "  EnumRegisteredActionsProc: procedure (Proc: TEnumActionProc; Info: Pointer) = nil;\r\n" +
            "  CreateActionProc: function (AOwner: TComponent; ActionClass: TBasicActionClass): TBasicAction = nil;\r\n" +
            "\r\n" +
            "implementation\r\n" +
            "\r\n" +
            "uses SysUtils, Windows, Forms, Menus, Consts, Graphics, Controls;\r\n" +
            "\r\n" +
            "procedure RegisterActions(const CategoryName: string;\r\n" +
            "  const AClasses: array of TBasicActionClass; Resource: TComponentClass);\r\n" +
            "begin\r\n" +
            "  if Assigned(RegisterActionsProc) then\r\n" +
            "    RegisterActionsProc(CategoryName, AClasses, Resource) else\r\n" +
            "    raise Exception.CreateRes(@SInvalidActionRegistration);\r\n" +
            "end;\r\n" +
            "\r\n" +
            "procedure UnRegisterActions(const AClasses: array of TBasicActionClass);\r\n" +
            "begin\r\n" +
            "  if Assigned(UnRegisterActionsProc) then\r\n" +
            "    UnRegisterActionsProc(AClasses) else\r\n" +
            "    raise Exception.CreateRes(@SInvalidActionUnregistration);\r\n" +
            "end;\r\n" +
            "\r\n";
		
		protected const string cACWGTSBroker =
            "unit ACWGTSBroker;\r\n" +
            "\r\n" +
            "interface\r\n" +
            "\r\n" +
            "uses\r\n" +
            "  oConsole,\r\n" +
            "  // system units\r\n" +
            "  Classes, SysUtils, Contnrs,\r\n" +
            "  // DAL-specific units\r\n" +
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
		
		protected const string cWinSpool =
            "\r\n" +
            "{*******************************************************}\r\n" +
            "{                                                       }\r\n" +
            "{       Borland Delphi Run-time Library                 }\r\n" +
            "{       Win32 printer API Interface Unit                }\r\n" +
            "{                                                       }\r\n" +
            "{       Copyright (c) 1985-1999, Microsoft Corporation  }\r\n" +
            "{                                                       }\r\n" +
            "{       Translator: Borland Software Corporation        }\r\n" +
            "{                                                       }\r\n" +
            "{*******************************************************}\r\n" +
            "\r\n" +
            "unit WinSpool;\r\n" +
            "\r\n" +
            "{$WEAKPACKAGEUNIT}\r\n" +
            "\r\n" +
            "interface\r\n" +
            "\r\n" +
            "uses Windows;\r\n" +
            "\r\n" +
            "(*$HPPEMIT '' *)\r\n" +
            "(*$HPPEMIT '#include <winspool.h>' *)\r\n" +
            "(*$HPPEMIT '' *)\r\n" +
            "\r\n" +
            "type\r\n" +
            "  PPrinterInfo1A = ^TPrinterInfo1A;\r\n" +
            "  PPrinterInfo1W = ^TPrinterInfo1W;\r\n" +
            "  PPrinterInfo1 = PPrinterInfo1A;\r\n" +
            "  {$EXTERNALSYM _PRINTER_INFO_1A}\r\n" +
            "  _PRINTER_INFO_1A = record\r\n" +
            "    Flags: DWORD;\r\n" +
            "    pDescription: PAnsiChar;\r\n" +
            "    pName: PAnsiChar;\r\n" +
            "    pComment: PAnsiChar;\r\n" +
            "  end;\r\n" +
            "  {$EXTERNALSYM _PRINTER_INFO_1W}\r\n" +
            "  _PRINTER_INFO_1W = record\r\n" +
            "    Flags: DWORD;\r\n" +
            "    pDescription: PWideChar;\r\n" +
            "    pName: PWideChar;\r\n" +
            "\r\n";
		
		protected const string cWPTIO =
            "{*******************************************************************\r\n" +
            "$History: Wptio.pas $\r\n" +
            "//\r\n" +
            "// *****************  Version 14  *****************\r\n" +
            "// User: 899469       Date: 7/01/05    Time: 8:07p\r\n" +
            "// Updated in $/DISPMGR/6.0/U\r\n" +
            "// SQL Changes\r\n" +
            "//\r\n" +
            "// *****************  Version 12  *****************\r\n" +
            "// User: Sg0618311    Date: 6/20/05    Time: 4:02p\r\n" +
            "// Updated in $/DISPMGR/5.3/U\r\n" +
            "// Modified OpenWAYPOINTFileAccelerated to use UNCPath (LOT 59450)\r\n" +
            "//\r\n" +
            "// *****************  Version 11  *****************\r\n" +
            "// User: Sg0618311    Date: 5/31/05    Time: 7:20p\r\n" +
            "// Updated in $/DISPMGR/5.3/U\r\n" +
            "// Initialize the recordsize for OpenFileAccelerated (SBG 59115)\r\n" +
            "//\r\n" +
            "// *****************  Version 10  *****************\r\n" +
            "// User: Sg0618311    Date: 3/09/05    Time: 5:30a\r\n" +
            "// Updated in $/DISPMGR/5.3/U\r\n" +
            "// Added code for layer synchronizer\r\n" +
            "//\r\n" +
            "// *****************  Version 8  *****************\r\n" +
            "// User: 899482       Date: 7/18/03    Time: 5:55p\r\n" +
            "// Updated in $/AP360T/_AllSource/U\r\n" +
            "// Wrong waypoint used for terminal waypoint. (SI 1-9B2FX ARINC)\r\n" +
            "//\r\n" +
            "// *****************  Version 7  *****************\r\n" +
            "// User: 982760       Date: 3/24/03    Time: 4:43a\r\n" +
            "// Updated in $/AP360T/_AllSource/U\r\n" +
            "// Merged with V3.6 as at 24Mar03 - KB\r\n" +
            "//\r\n" +
            "// *****************  Version 6  *****************\r\n" +
            "// User: 899469       Date: 3/06/03    Time: 9:49p\r\n" +
            "// Updated in $/AP360T/_AllSource/U\r\n" +
            "// Merge 5 w EAGLE SP5\r\n" +
            "//\r\n" +
            "// *****************  Version 7  *****************\r\n" +
            "// User: Imp          Date: 2/12/03    Time: 10:26a\r\n" +
            "// Updated in $/GUI/eaglegui/u\r\n" +
            "// Resolve duplicate terminal waypoints(ARINC 2179)\r\n" +
            "//\r\n" +
            "// *****************  Version 5  *****************\r\n" +
            "// User: 982760       Date: 11/25/02   Time: 10:31p\r\n" +
            "// Updated in $/FP1/_AllSource/U\r\n" +
            "// Merged with V3.6.3 - KB\r\n" +
            " *\r\n" +
            " * *****************  Version 4  *****************\r\n" +
            " * User: 618382       Date: 6/26/02    Time: 4:34p\r\n" +
            " * Updated in $/FP1/_AllSource/U\r\n" +
            " * Merged changes made to Airpath 3.6 during merge phase.\r\n" +
            " *\r\n" +
            " * *****************  Version 3  *****************\r\n" +
            " * User: 899469       Date: 5/07/02    Time: 10:22p\r\n" +
            " * Updated in $/FP1/_AllSource/U\r\n" +
            " * Removed unneeded var - IP\r\n" +
            " *\r\n" +
            " * *****************  Version 2  *****************\r\n" +
            " * User: 899469       Date: 3/27/02    Time: 9:01p\r\n" +
            " * Updated in $/FP1/CODE/U\r\n" +
            " * Merged - IP\r\n" +
            "//\r\n" +
            "// *****************  Version 6  *****************\r\n" +
            "// User: Imp          Date: 5/27/99    Time: 9:16a\r\n" +
            "// Updated in $/GUI/eaglegui/u\r\n" +
            "// OAS 558 (3806) Search on Tape ID not ChartID to find duplicates\r\n" +
            "//\r\n" +
            "// *****************  Version 5  *****************\r\n" +
            "// User: Wek          Date: 7/05/98    Time: 10:49p\r\n" +
            "// Updated in $/GUI/eaglegui/u\r\n" +
            "// FANS #3...Added handling of +\r\n" +
            "//\r\n" +
            "// *****************  Version 4  *****************\r\n" +
            "// User: Imp          Date: 12/16/97   Time: 3:37p\r\n" +
            "// Updated in $/GUI/eaglegui/u\r\n" +
            "// dRb72 Added History stamp\r\n" +
            "*******************************************************************}\r\n" +
            "{ WPTIO    PAS             18,787 06/15/96  05:21p  DRB 000 }\r\n" +
            "unit WPTIO;\r\n" +
            "\r\n" +
            "interface\r\n" +
            "uses\r\n" +
            "    DBunit\r\n" +
            "    , GDeclare\r\n" +
            "    , MYSTD\r\n" +
            "    , GEODESIC\r\n" +
            "    , Sysutils\r\n" +
            "    ;\r\n" +
            "\r\n" +
            "{$I Waypoint.REC}\r\n" +
            "\r\n" +
            "procedure ClearWAYPOINTRecord(var WAYPOINTRecord :WAYPOINTRecordType);\r\n" +
            "\r\n" +
            "procedure GetWaypoint\r\n" +
            "    (iIdentifier ,\r\n" +
            "    iCC ,\r\n" +
            "    iCompanyCode :string;\r\n" +
            "    var iWaypointRecord :WaypointRecordType);\r\n" +
            "\r\n" +
            "procedure GetTapeWaypoint\r\n" +
            "    (TapeId , CC , Comp :string;\r\n" +
            "    var WaypointRecord :WaypointRecordType);\r\n" +
            "\r\n" +
            "(*  REMARKS:  If the company is blank, then the first record for   *)\r\n" +
            "(*            the company or master that matches the country code  *)\r\n" +
            "(*            and TapeId is returned.                              *)\r\n" +
            "(*            If the country code is also blank, the first record  *)\r\n" +
            "(*            for the requested company or master that matches the *)\r\n" +
            "(*            the TapeId is returned.                              *)\r\n" +
            "\r\n" +
            "\r\n" +
            "function GetAbsWaypointId(var iTapeId , iCC , iCompanyCode :string) :string;\r\n" +
            "\r\n" +
            "(*  PURPOSE:  Locates a Waypoint record and returns its Absolute Id. *)\r\n" +
            "(*                                                                 *)\r\n" +
            "(*  REMARKS:  If the company is blank, then the first record for   *)\r\n" +
            "(*            the company or master that matches the country code  *)\r\n" +
            "(*            and TapeId is used.                                  *)\r\n" +
            "(*            If the country code is also blank, the first index   *)\r\n" +
            "(*            for the requested company or master that matches the *)\r\n" +
            "(*            the TapeId is used.                                  *)\r\n" +
            "\r\n" +
            "function GetAbsWaypointIdWithFirstKey(var iIdentifier , iCC , iCompanyCode :string) :string;\r\n" +
            "\r\n" +
            "function GetEqualAbsWaypointId(var iTapeId , iCC , iCompanyCode :string) :string;\r\n" +
            "\r\n" +
            "function GetEqualAbsWaypointIdWithFirstKey(var iIdentifier , iCC , iCompanyCode :string) :string;\r\n" +
            "\r\n" +
            "function GetAbsTerminalWaypointId\r\n" +
            "    (iTapeId :string;\r\n" +
            "    var iCC :string;\r\n" +
            "    iRegion ,\r\n" +
            "    iCompanyCode :string) :string;\r\n" +
            "(*  PURPOSE:  Locates a Terminal Waypoint record and returns its Absolute Id. *)\r\n" +
            "(*                                                                 *)\r\n" +
            "(*  REMARKS:  If the company is blank, then the first record for   *)\r\n" +
            "(*            the company or master that matches the country code  *)\r\n" +
            "(*            and TapeId is used.                                  *)\r\n" +
            "(*            If the country code is also blank, the first index   *)\r\n" +
            "(*            for the requested company or master that matches the *)\r\n" +
            "(*            the TapeId is used.                                  *)\r\n" +
            "\r\n" +
            "\r\n" +
            "function GetAbsTerminalWaypoint\r\n" +
            "   (iTapeId :string;\r\n" +
            "   var iCC :string;\r\n" +
            "   iRegion ,\r\n" +
            "   iCompanyCode :string;\r\n" +
            "   var WaypointRecord :WaypointRecordType): Integer;\r\n" +
            "(* same as above except it returns the Waypoint record*)\r\n" +
            "\r\n" +
            "function GetAbsWaypoint\r\n" +
            "    (AbsId :AbsoluteIdType;\r\n" +
            "    Comp :string;\r\n" +
            "    var WaypointRecord :WaypointRecordType): Integer;\r\n" +
            "\r\n" +
            "(*  PURPOSE:  Uses the idexed based on the Absolute Id to retrieve *)\r\n" +
            "(*            a Waypoint record.                                   *)\r\n" +
            "\r\n" +
            "\r\n" +
            "procedure GetNextWaypointInArea\r\n" +
            "    (AbsKey :string;\r\n" +
            "    var WaypointRecord :WaypointRecordType);\r\n" +
            "\r\n" +
            "(*  PURPOSE:  Uses the idexed based on the Absolute Id to retrieve *)\r\n" +
            "(*            the Waypoint record which has the next absolute key    *)\r\n" +
            "(*            greater than the one entered.                        *)\r\n" +
            "\r\n" +
            "\r\n" +
            "procedure InsertWaypoint\r\n" +
            "    (WaypointRecord :WaypointRecordType;\r\n" +
            "     iUpdateNavRecord : boolean = false);\r\n" +
            "\r\n" +
            "procedure ReplaceWaypoint\r\n" +
            "    (WaypointRecord :WaypointRecordType;\r\n" +
            "     iUpdateNavRecord : boolean = false);\r\n" +
            "\r\n" +
            "procedure UpdateWaypoint(var iNewWaypointRecord: WaypointRecordType);\r\n" +
            "\r\n" +
            "procedure DeleteWaypoint\r\n" +
            "    (WaypointRecord :WaypointRecordType;\r\n" +
            "     iUpdateNavRecord : boolean = false);\r\n" +
            "\r\n" +
            "procedure Get1stWaypoint\r\n" +
            "    (PointId :string;\r\n" +
            "    var WaypointRecord :WaypointRecordType);\r\n" +
            "\r\n" +
            "procedure Get1stWaypointWithFirstKey\r\n" +
            "    (PointId :string;\r\n" +
            "    var WaypointRecord :WaypointRecordType);\r\n" +
            "\r\n" +
            "procedure GetNextWaypoint(var WaypointRecord :WaypointRecordType);\r\n" +
            "\r\n" +
            "procedure GetNextWaypointWithFirstKey(var WaypointRecord :WaypointRecordType);\r\n" +
            "\r\n" +
            "procedure Get1stWaypointAbs(var WaypointRecord :WaypointRecordType);\r\n" +
            "\r\n" +
            "procedure GetNextWaypointAbs(var WaypointRecord :WaypointRecordType);\r\n" +
            "\r\n" +
            "procedure GetClosestWaypoint\r\n" +
            "    (PointId :string;\r\n" +
            "    AbsPointId :string;\r\n" +
            "    Comp :string;\r\n" +
            "    var WaypointRecord :WaypointRecordType);\r\n" +
            "\r\n" +
            "function ItsALatLon(s :string) :boolean;\r\n" +
            "\r\n" +
            "procedure OpenWAYPOINTFileAccelerated;\r\n" +
            "\r\n" +
            "//This procedure is used in RRAWY load program.\r\n" +
            "procedure GetFirstWaypointRec(var WaypointRec :WaypointRecordType);\r\n" +
            "procedure GetNextWaypointRec(var WaypointRec :WaypointRecordType);\r\n" +
            "\r\n" +
            "procedure CloseWAYPOINTFile;\r\n" +
            "function OpenWaypointFuture(): integer;\r\n" +
            "procedure OpenWAYPOINTFile(KeyNumber :integer = 0);\r\n" +
            "\r\n" +
            "implementation\r\n" +
            "\r\n" +
            "uses\r\n" +
            "    CommonConsts,\r\n" +
            "    DMGlobals,\r\n" +
            "    CODEIO, CommonTypes, DBOperations;\r\n" +
            "\r\n" +
            "const\r\n" +
            "    coTableName = 'WAYPOINT';\r\n" +
            "{$I DBOPEN.INC}\r\n" +
            "{$I ERRMSGS.INC}\r\n" +
            "var\r\n" +
            "    WaypointFCB                              :string[128];\r\n" +
            "    WaypointRecordSize                       :integer;\r\n" +
            "    WaypointKey                              :string[32];\r\n" +
            "    WaypointKey3                             :string[32];\r\n" +
            "                                                            {unit WPTIO}\r\n" +
            "\r\n";
		
		protected const string cCommDlg =
            "\r\n" +
            "{*******************************************************}\r\n" +
            "{                                                       }\r\n" +
            "{       Borland Delphi Run-time Library                 }\r\n" +
            "{       Win32 Common Dialogs interface unit             }\r\n" +
            "{                                                       }\r\n" +
            "{       Copyright (c) 1985-1999, Microsoft Corporation  }\r\n" +
            "{                                                       }\r\n" +
            "{       Translator: Borland Software Corporation        }\r\n" +
            "{                                                       }\r\n" +
            "{*******************************************************}\r\n" +
            "\r\n" +
            "unit CommDlg;\r\n" +
            "\r\n" +
            "{$WEAKPACKAGEUNIT}\r\n" +
            "\r\n" +
            "{$HPPEMIT '#include <commdlg.h>'}\r\n" +
            "\r\n" +
            "interface\r\n" +
            "\r\n" +
            "uses Windows, Messages, ShlObj;\r\n" +
            "\r\n" +
            "type\r\n" +
            "  POpenFilenameA = ^TOpenFilenameA;\r\n" +
            "  POpenFilenameW = ^TOpenFilenameW;\r\n" +
            "  POpenFilename = POpenFilenameA;\r\n" +
            "  {$EXTERNALSYM tagOFNA}\r\n" +
            "  tagOFNA = packed record\r\n" +
            "    lStructSize: DWORD;\r\n" +
            "    hWndOwner: HWND;\r\n" +
            "    hInstance: HINST;\r\n" +
            "    lpstrFilter: PAnsiChar;\r\n" +
            "    lpstrCustomFilter: PAnsiChar;\r\n" +
            "    nMaxCustFilter: DWORD;\r\n" +
            "    nFilterIndex: DWORD;\r\n" +
            "    lpstrFile: PAnsiChar;\r\n" +
            "    nMaxFile: DWORD;\r\n" +
            "    lpstrFileTitle: PAnsiChar;\r\n" +
            "    nMaxFileTitle: DWORD;\r\n" +
            "    lpstrInitialDir: PAnsiChar;\r\n" +
            "    lpstrTitle: PAnsiChar;\r\n" +
            "    Flags: DWORD;\r\n" +
            "    nFileOffset: Word;\r\n" +
            "    nFileExtension: Word;\r\n" +
            "    lpstrDefExt: PAnsiChar;\r\n" +
            "    lCustData: LPARAM;\r\n" +
            "    lpfnHook: function(Wnd: HWND; Msg: UINT; wParam: WPARAM; lParam: LPARAM): UINT stdcall;\r\n" +
            "    lpTemplateName: PAnsiChar;\r\n" +
            "    pvReserved: Pointer;\r\n" +
            "    dwReserved: DWORD;\r\n" +
            "    FlagsEx: DWORD;\r\n" +
            "  end;\r\n" +
            "  {$EXTERNALSYM tagOFNW}\r\n" +
            "  tagOFNW = packed record\r\n" +
            "    lStructSize: DWORD;\r\n" +
            "    hWndOwner: HWND;\r\n" +
            "    hInstance: HINST;\r\n" +
            "    lpstrFilter: PWideChar;\r\n" +
            "    lpstrCustomFilter: PWideChar;\r\n" +
            "    nMaxCustFilter: DWORD;\r\n" +
            "    nFilterIndex: DWORD;\r\n" +
            "    lpstrFile: PWideChar;\r\n" +
            "    nMaxFile: DWORD;\r\n" +
            "    lpstrFileTitle: PWideChar;\r\n" +
            "    nMaxFileTitle: DWORD;\r\n" +
            "    lpstrInitialDir: PWideChar;\r\n" +
            "    lpstrTitle: PWideChar;\r\n" +
            "    Flags: DWORD;\r\n" +
            "    nFileOffset: Word;\r\n" +
            "    nFileExtension: Word;\r\n" +
            "    lpstrDefExt: PWideChar;\r\n" +
            "    lCustData: LPARAM;\r\n" +
            "    lpfnHook: function(Wnd: HWND; Msg: UINT; wParam: WPARAM; lParam: LPARAM): UINT stdcall;\r\n" +
            "    lpTemplateName: PWideChar;\r\n" +
            "    pvReserved: Pointer;\r\n" +
            "    dwReserved: DWORD;\r\n" +
            "    FlagsEx: DWORD;\r\n" +
            "  end;\r\n" +
            "  {$EXTERNALSYM tagOFN}\r\n" +
            "  tagOFN = tagOFNA;\r\n" +
            "  TOpenFilenameA = tagOFNA;\r\n" +
            "  TOpenFilenameW = tagOFNW;\r\n" +
            "  TOpenFilename = TOpenFilenameA;\r\n" +
            "  {$EXTERNALSYM OPENFILENAMEA}\r\n" +
            "  OPENFILENAMEA = tagOFNA;\r\n" +
            "  {$EXTERNALSYM OPENFILENAMEW}\r\n" +
            "  OPENFILENAMEW = tagOFNW;\r\n" +
            "  {$EXTERNALSYM OPENFILENAME}\r\n" +
            "  OPENFILENAME = OPENFILENAMEA;\r\n" +
            "\r\n" +
            "{$EXTERNALSYM GetOpenFileName}\r\n" +
            "function GetOpenFileName(var OpenFile: TOpenFilename): Bool; stdcall;\r\n" +
            "{$EXTERNALSYM GetOpenFileNameA}\r\n" +
            "function GetOpenFileNameA(var OpenFile: TOpenFilenameA): Bool; stdcall;\r\n" +
            "{$EXTERNALSYM GetOpenFileNameW}\r\n" +
            "function GetOpenFileNameW(var OpenFile: TOpenFilenameW): Bool; stdcall;\r\n" +
            "{$EXTERNALSYM GetSaveFileName}\r\n" +
            "function GetSaveFileName(var OpenFile: TOpenFilename): Bool; stdcall;\r\n" +
            "{$EXTERNALSYM GetSaveFileNameA}\r\n" +
            "function GetSaveFileNameA(var OpenFile: TOpenFilenameA): Bool; stdcall;\r\n" +
            "{$EXTERNALSYM GetSaveFileNameW}\r\n" +
            "function GetSaveFileNameW(var OpenFile: TOpenFilenameW): Bool; stdcall;\r\n" +
            "{$EXTERNALSYM GetFileTitle}\r\n" +
            "function GetFileTitle(FileName: PChar; Title: PChar; TitleSize: Word): Smallint; stdcall;\r\n" +
            "{$EXTERNALSYM GetFileTitleA}\r\n" +
            "function GetFileTitleA(FileName: PAnsiChar; Title: PAnsiChar; TitleSize: Word): Smallint; stdcall;\r\n" +
            "\r\n";
		
		protected const string cCOMMGRIB =
            "{*******************************************************************\r\n" +
            "$History: Commgrib.pas $\r\n" +
            " *\r\n" +
            " * *****************  Version 2  *****************\r\n" +
            " * User: 982760       Date: 5/20/02    Time: 4:46a\r\n" +
            " * Updated in $/FP1/FDCOMM/U\r\n" +
            " * Merged - KB\r\n" +
            "//\r\n" +
            "// *****************  Version 5  *****************\r\n" +
            "// User: Wek          Date: 1/07/99    Time: 3:56p\r\n" +
            "// Updated in $/GUI/eaglegui/u\r\n" +
            "// dRb 837 Remove all hardcoding of \\EAGLE\\ and use SystemPath field from\r\n" +
            "// GDECLARE\r\n" +
            "//\r\n" +
            "// *****************  Version 4  *****************\r\n" +
            "// User: Imp          Date: 4/02/98    Time: 4:18p\r\n" +
            "// Updated in $/GUI/eaglegui/u\r\n" +
            "// oas files 4-2-98\r\n" +
            "//\r\n" +
            "// *****************  Version 3  *****************\r\n" +
            "// User: Shl          Date: 1/08/98    Time: 4:50p\r\n" +
            "// Updated in $/GUI/eaglegui/u\r\n" +
            "// RG-OASNEW Changes sent on 02-01-98\r\n" +
            "//\r\n" +
            "// *****************  Version 2  *****************\r\n" +
            "// User: Imp          Date: 12/16/97   Time: 4:22p\r\n" +
            "// Updated in $/GUI/eaglegui/u\r\n" +
            "// dRb72 Added History stamp\r\n" +
            "*******************************************************************}\r\n" +
            "{ COMMGRIB PAS              3,041 07/17/97  03:24p  WEK 010 }\r\n" +
            "{ MOD Added QMSGDESK}\r\n" +
            "{ COMMGRIB PAS              2,712 04/25/95  09:53p  WEK 00 }\r\n" +
            "unit COMMGRIB;\r\n" +
            "{$F+}\r\n" +
            "interface\r\n" +
            "\r\n" +
            "uses SYSUTILS\r\n" +
            "    , dbunit\r\n" +
            "    , GDeclare\r\n" +
            "    , MYSTD\r\n" +
            "    , DateTime\r\n" +
            "    , QMSGDESK\r\n" +
            "    , DYNEXEC\r\n" +
            "    , uGRIBSIT\r\n" +
            "    ;\r\n" +
            "\r\n" +
            "(*\r\n" +
            "HISTORY\r\n" +
            "   30Dec98 TD  Use Global Variable 'SystemPath' Instead of ':\\EAGLE\\'\r\n" +
            "   23Mar98 KB  Findclose fixes\r\n" +
            "*)\r\n" +
            "\r\n" +
            "procedure GRIBReceive\r\n" +
            "    (iMasterDisk :char;\r\n" +
            "    iCompanyCode :string;\r\n" +
            "    iSupervisorDesk :string;\r\n" +
            "    iReturnCode :integer);\r\n" +
            "\r\n" +
            "implementation\r\n" +
            "\r\n" +
            "uses\r\n" +
            "    CommonConsts,\r\n" +
            "    CommonTypes;\r\n" +
            "\r\n" +
            "{$I Errmsgs.INC}\r\n" +
            "\r\n" +
            "\r\n" +
            "procedure GRIBReceive\r\n" +
            "    (iMasterDisk :char;\r\n" +
            "    iCompanyCode :string;\r\n" +
            "    iSupervisorDesk :string;\r\n" +
            "    iReturnCode :integer);\r\n" +
            "var\r\n" +
            "    Parms                                    :string;\r\n" +
            "    PickMask                                 :string;\r\n" +
            "    FileData                                 :Tsearchrec;\r\n" +
            "    GRIB                                     :file;\r\n" +
            "begin\r\n" +
            "    iReturnCode := 0;\r\n" +
            "    PickMask := EnvironmentRecord.MasterEXEDisk\r\n" +
            "        + SystemPath + 'MET\\GRB\\GRIB.DAT';\r\n" +
            "    iReturnCode := findfirst(PickMask , 0 {NormalFilesOnly} , FileData);\r\n" +
            "\r\n" +
            "    if (iReturnCode = 0) then\r\n" +
            "    begin\r\n" +
            "        findclose(FileData);\r\n" +
            "        if FileData.Size > 0 then\r\n" +
            "        begin\r\n" +
            "            Parms := 'N WAKE'\r\n" +
            "                + iMasterDisk\r\n" +
            "                + iMasterDisk\r\n" +
            "                + iMasterDisk\r\n" +
            "                + 'CA'\r\n" +
            "                + iCompanyCode\r\n" +
            "                + iMasterDisk\r\n" +
            "                + 'DDP ' + EnvironmentRecord.SystemName;\r\n" +
            "            GRIBSITAMainProcedure(iReturnCode);\r\n" +
            "\r\n" +
            "            if iReturnCode = 0 then\r\n" +
            "\r\n";
			
		#endregion
		
		protected void CheckSectionUses(string iNewUsesSection, IList<string> iExpected, string iErrorMsg)
		{
			UnitList vActual = new UnitList();
			if (!iNewUsesSection.Equals(""))
				vActual = UsesClauseReader.GetUnits(iNewUsesSection);
			
			Assert.AreEqual(iExpected.Count, vActual.Count, "Different no. of units in the uses clause. " + iErrorMsg);
			for(int vUnitIdx = 0; vUnitIdx < iExpected.Count; vUnitIdx++)
				Assert.AreEqual(iExpected[vUnitIdx].ToLower(), vActual[vUnitIdx].Name.ToLower(), "Unit " + iExpected[vUnitIdx] + " doesn't match correct position on the list. " + iErrorMsg);
		}
		
		protected void CheckUses(string iNewContent, IList<string> iInterface, IList<string> iImplementation, string iErrorMsg)
		{
			string vInterface, vImplementation;
			UsesClauseReader.ExtractUses(iNewContent, out vInterface, out vImplementation);
			CheckSectionUses(vInterface, iInterface, "Interface section. " + iErrorMsg);
			CheckSectionUses(vImplementation, iImplementation, "Implementation section." + iErrorMsg);
		}

		protected void TestExecute(string iUnitContent, SuggestedUnitStructure iSuggestedStructure, IList<string> iExpectedInterfaceUnits, IList<string> iExpectedImplementationUnits, IList<string> iIgnoredUnits, string iErrorMsg)
		{
			DelphiUnitCleanerOperation vOperation = new DelphiUnitCleanerOperation();
			string vActual = vOperation.Execute(iUnitContent, iSuggestedStructure, iIgnoredUnits);
			CheckUses(vActual, iExpectedInterfaceUnits, iExpectedImplementationUnits, iErrorMsg);
		}
		
		[Test]
		public void Execute()
		{
			SuggestedUnitStructureList vSuggested = IcarusAnalyzerReportParser.Parse(cReport);
			TestExecute(cAbsoluteIdUtils, vSuggested["absoluteidutils"],
			          	new string[] {},
			          	new string[] {"StrUtils", "Aptio", "Navio", "Wptio", "DBStatusCodes", "gdeclare", "SysUtils"},
			          	new string[] {}, "Test 1");
			TestExecute(cAbstractCacheStreamerClass, vSuggested["abstractcachestreamerclass"],
			            new string[] {},
			            new string[] {"SqlCacheUtils", "DatabaseQueryFactoryClass", "Windows", "SysUtils"},
			          	new string[] {}, "Test 2");
			TestExecute(cAbstractTable, vSuggested["abstracttable"],
			            new string[] {"Logger", "DB"},
			            new string[] {"LoggerCategories", "SysUtils"},
			          	new string[] {}, "Test 3");
			TestExecute(cActiveX, vSuggested["activex"],
			            new string[] {"Windows", "messages"},
			            new string[] {},
			          	new string[] {}, "Test 4");
			TestExecute(cActnList, vSuggested["actnlist"],
			            new string[] {"Classes", "Messages", "ImgList"},
			            new string[] {"SysUtils", "Windows", "Forms", "Menus", "Consts", "Controls"},
			          	new string[] {}, "Test 5");
			TestExecute(cACWGTSBroker, vSuggested["acwgtsbroker"],
			            new string[] {"Contnrs", "QueryInterface", "DMBaseBrokerClass", "DMBaseArchSQLBrokerClass", "AbstractQuery", "ACWGTSio"},
			            new string[] {"SysUtils", "Classes", "DatabaseQueryFactoryClass", "DMBaseSQLBrokerClass", "MyStd", "DMGlobals", "CommonConsts", "DMSQLQueryFactory", "DMBtrvQueryFactory", "gdeclare", "DBUNIT"},
			          	new string[] {}, "Test 6");
			TestExecute(cWinSpool, vSuggested["winspool"],
			            new string[] {"Windows"},
			            new string[] {},
			          	new string[] {}, "Test 7");
			TestExecute(cWPTIO, vSuggested["wptio"],
			            new string[] {"GDeclare"},
			            new string[] {"Sysutils", "MYSTD", "DBUnit", "GEODESIC", "CommonConsts", "DMGlobals", "CODEIO", "DBOperations"},
			          	new string[] {}, "Test 8");
			TestExecute(cCommDlg, vSuggested["commdlg"],
			            new string[] {"Windows", "Messages", "ShlObj"},
			            new string[] {},
			          	new string[] {}, "Test 9");
			TestExecute(cCOMMGRIB, vSuggested["commgrib"],
			            new string[] {},
			            new string[] {"SYSUTILS", "GDeclare", "QMSGDESK", "uGRIBSIT"},
			          	new string[] {}, "Test 10");
			TestExecute(cCOMMGRIB, vSuggested["commgrib"],
			            new string[] {"SYSUTILS", "DBUNIT"},
			            new string[] {"GDeclare", "QMSGDESK", "uGRIBSIT"},
			          	new string[] {"SYSUTILS", "DBUNIT"}, "Test 11");
		}
	}
}
