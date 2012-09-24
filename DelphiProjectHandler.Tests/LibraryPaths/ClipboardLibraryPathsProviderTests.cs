using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using NUnit.Framework;

using DelphiProjectHandler.LibraryPaths;

namespace DelphiProjectHandler.Tests.LibraryPaths
{
    [TestFixture]
    public class ClipboardLibraryPathsProviderTests
    {
        protected string cPaths = @"$(DELPHI)\Lib;$(DELPHI)\Bin;$(DELPHI)\Imports;$(DELPHI)\Projects\Bpl;$(DM_VIEW)\Lib;$(DM_TP)\abbrevia\source;$(DM_TP)\TGlobe\SOURCE;$(DM_TP)\AsyncPRO\source;$(DM_TP)\RegExpr\Source;$(DM_TP)\BTrieveSDK;$(DM_TP)\PxLib;$(DM_TP)\ExcMagicD6;$(DM_TP)\JWA\Win32Api;$(DM_TP)\XMLParser;$(DM_TP)\XMLPRO\source;$(DM_TP)\CalcExpress;$(DM_TP)\Indy9;$(DM_VIEW)\I;$(DM_VIEW)\Maps;$(DM_VIEW)\Forms;$(DM_VIEW)\U;$(DM_VIEW)\U\Brokers;$(DM_VIEW)\U\Eram;$(DM_VIEW)\U\FPValidators;$(BASA_DM);$(BASA_DM)\UUtils;$(BASA_DM)\UBtrieve;$(BASA_DM)\UBtrieve\UBroker;$(BASA_DM)\UFacilitator;$(BASA_DM)\USQL\UBroker;$(BASA_DM)\USQL\UOperation;$(BASA_DM)\USQL\UQueryBuilder;$(BASA_DM)\USQL\UUtil;$(BASA_DM)\USQL\UUtil\VersionInfo;$(BASA_DM)\USQLCache;$(BASA_DM)\USQLCache\UOperation;$(BASA_DM)\USQLCache\UUtil;$(BASA_DM)\UStream;$(BASA_DM)\UTransfer;$(BASA_DM)\USQL;$(DM_TP)\FCSUtils;$(DM_TP)\FCSUtils\System;$(DM_TP)\FCSUtils\DateUtils;$(DM_TP)\FCSUtils\DbUtils;$(DM_TP)\FCSUtils\Logging;$(DM_TP)\FCSUtils\Security;$(DM_TP)\FCSUtils\XMLRTTI;$(DM_TP)\ZipMaster;$(DM_TP)\KeyHelp;$(DM_TP)\DelphiKit;$(DM_VIEW)\U\EuroATCPlanValidation;$(DM_VIEW)\U\EuroATCPlanValidation\CFMU;$(DM_TP)\Guggenheim\Dcu;$(DM_TP)\JEDI\JCL\lib\d6;$(DM_TP)\JEDI\JCL\source;$(DM_TP)\JEDI\jvcl\lib\D6;$(DM_TP)\JEDI\jvcl\common;$(DM_TP)\JEDI\jvcl\Resources";

        [Test]
        [STAThread]
        public void List()
        {
            ILibraryPathsProvider vProvider = new ClipboardLibraryPathsProvider();
            Clipboard.SetText(cPaths);
            IList<string> vPaths = vProvider.List();
            Assert.AreEqual(58, vPaths.Count);
            Assert.GreaterOrEqual(vPaths.IndexOf(@"$(DELPHI)\Lib"), 0, "Test 1");
            Assert.GreaterOrEqual(vPaths.IndexOf(@"$(DM_TP)\JEDI\jvcl\Resources"), 0, "Test 2");
            Assert.AreEqual(-1, vPaths.IndexOf(";"), "Test 3");
        }
    }
}
