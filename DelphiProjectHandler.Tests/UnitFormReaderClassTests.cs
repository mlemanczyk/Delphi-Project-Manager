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

            string vTestValue1 = @"� TFRMACFTDISCREPANCIES 0�  TPF0�TfrmAcftDiscrepanciesfrmAcftDiscrepanciesLeft� TopiWidth�Height� CaptionAircraft DiscrepanciesVisibleOnClose	FormCloseOnCloseQueryFormCloseQuery";

            string vTestValue2 = @"� TFRMAIRCRAFTINFO 02  TPF0TfrmAircraftInfofrmAircraftInfoLeft� Top";

            string vTestValue3 = @"� TFRMBASESI 0�  TPF0�TfrmBaseSI   frmBaseSILeftUTop� CaptionfrmBa";

            #endregion

            TestGetBinaryFormName(vTestValue1, "frmAcftDiscrepancies", "1");
            TestGetBinaryFormName(vTestValue2, "frmAircraftInfo", "2");
            TestGetBinaryFormName(vTestValue3, "frmBaseSI", "3");
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void GetBinaryFormName_FormNameIsLast()
        {
            string vTestValue = @"� TFRMBASESI 0�  TPF0�TfrmBase";
            TestGetBinaryFormName(vTestValue, "", "");
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void GetBinaryFromName_WithoutMarker()
        {
            string vTestValue = @"� TFRMACFTDISCREPANCIES 0�  �TfrmAcftDiscrepanciesfrmAcftDiscrepanciesLeft� TopiWidth�Height� CaptionAircraft DiscrepanciesVisibleOnClose	FormCloseOnCloseQueryFormCloseQuery";
            TestGetBinaryFormName(vTestValue, "", "");
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void GetBinaryFormName_WithoutFormName()
        {
            string vTestValue = @"� TFRMACFTDISCREPANCIES 0�  TPF0�";
            TestGetBinaryFormName(vTestValue, "", "");
        }
    }
}
