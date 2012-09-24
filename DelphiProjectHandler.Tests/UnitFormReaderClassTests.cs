using System;
using NUnit.Framework;

using DelphiProjectHandler;

namespace DelphiProjectHandler.Tests
{
    [TestFixture]
    public class UnitFormReaderClassTests
    {
        protected void TestGetBinaryFormName(string iTestValue, string iExpected, string iMessage)
        {
            string vActual = UnitFormReader.GetBinaryFormName(iTestValue);
            Assert.AreEqual(iExpected, vActual, iMessage);
        }

        [Test]
        public void GetBinaryFormName()
        {
            #region (Test values)

            string vTestValue1 = @"ÿ TFRMACFTDISCREPANCIES 0É  TPF0ñTfrmAcftDiscrepanciesfrmAcftDiscrepanciesLeftî TopiWidth™Height¸ CaptionAircraft DiscrepanciesVisibleOnClose	FormCloseOnCloseQueryFormCloseQuery";

            string vTestValue2 = @"ÿ TFRMAIRCRAFTINFO 02  TPF0TfrmAircraftInfofrmAircraftInfoLeftÖ Top";

            string vTestValue3 = @"ÿ TFRMBASESI 0í  TPF0ñTfrmBaseSI   frmBaseSILeftUTop° CaptionfrmBa";

            #endregion

            TestGetBinaryFormName(vTestValue1, "frmAcftDiscrepancies", "1");
            TestGetBinaryFormName(vTestValue2, "frmAircraftInfo", "2");
            TestGetBinaryFormName(vTestValue3, "frmBaseSI", "3");
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void GetBinaryFormName_FormNameIsLast()
        {
            string vTestValue = @"ÿ TFRMBASESI 0í  TPF0ñTfrmBase";
            TestGetBinaryFormName(vTestValue, "", "");
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void GetBinaryFromName_WithoutMarker()
        {
            string vTestValue = @"ÿ TFRMACFTDISCREPANCIES 0É  ñTfrmAcftDiscrepanciesfrmAcftDiscrepanciesLeftî TopiWidth™Height¸ CaptionAircraft DiscrepanciesVisibleOnClose	FormCloseOnCloseQueryFormCloseQuery";
            TestGetBinaryFormName(vTestValue, "", "");
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void GetBinaryFormName_WithoutFormName()
        {
            string vTestValue = @"ÿ TFRMACFTDISCREPANCIES 0É  TPF0ñ";
            TestGetBinaryFormName(vTestValue, "", "");
        }
    }
}
