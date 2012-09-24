using System;
using NUnit.Framework;

using DelphiProjectHandler;

namespace DelphiProjectHandler.Tests
{
    [TestFixture]
    public class UnitItemClassTests
    {
        protected void DoTestCreate_Name(string iExpectedName)
        {
            UnitItem vUnit = new UnitItem(iExpectedName);
            Assert.AreEqual(iExpectedName, vUnit.Name, "Name wasn't set properly");
            Assert.AreEqual("", vUnit.Path, "Path wasn't set to empty string");
        }

        protected void DoTestNameChange(string iInitialName, string iNewName)
        {
            UnitItem vUnit = new UnitItem(iInitialName);
            DoTestNameChange(vUnit, iInitialName, iNewName);
        }

        protected void DoTestNameChange(UnitItem iUnitItem, string iInitialName, string iNewName)
        {
            Assert.AreEqual(iInitialName, iUnitItem.Name, "Name wasn't set properly.");
            iUnitItem.Name = iNewName;
            Assert.AreEqual(iNewName, iUnitItem.Name, "Name wasn't changed correctly.");
        }

        protected void DoTestPathChange(UnitItem iUnitItem, string iInitialPath, string iNewPath, string iMessage)
        {
            Assert.AreEqual(iInitialPath, iUnitItem.Path, "Path wasn't set properly. " + iMessage);
            iUnitItem.Path = iNewPath;
            Assert.AreEqual(iNewPath, iUnitItem.Path, "Path wasn't changed correctly. " + iMessage);
        }

        protected void DoTestPathChange(string iInitialPath, string iNewPath)
        {
            UnitItem vUnit = new UnitItem("unit_name", iInitialPath, true, "");
            DoTestPathChange(vUnit, iInitialPath, iNewPath, "");

            vUnit = new UnitItem("", iInitialPath, true, "");
            DoTestPathChange(vUnit, iInitialPath, iNewPath, "Created with empty name.");
        }

        protected void DoTestToString_WithConditions(bool iAddConditions, string iSeparator, bool iSeparatorLast, string iExpected)
        {
            UnitItem vUnit = new UnitItem("unit_name", "unit_path", true, "");
            if (iAddConditions)
            {
                vUnit.StartingConditions = "{$condition1}";
                vUnit.EndingConditions = "{$condition2}";
            }

            string vActual = vUnit.ToString("  ", iSeparator, iSeparatorLast);

            Assert.AreEqual(iExpected, vActual);
        }

        protected void DoTestHasConditions(bool iExpected, bool iActualHasConditions, string iExpectedConditions, string iActualConditions, string iMessage)
        {
            Assert.AreEqual(iExpected, iActualHasConditions, "HasConditions value is wrong: " + iMessage);
            Assert.AreEqual(iExpectedConditions, iActualConditions, "Conditions value is wrong: " + iMessage);
        }

        protected void DoTestHasStartingConditions(string iConditions, bool iExpected)
        {
            UnitItem vUnit = new UnitItem("unit_name");
            vUnit.StartingConditions = iConditions;
            DoTestHasConditions(iExpected, vUnit.HasStartingConditions, iConditions, vUnit.StartingConditions, "Starting");
        }

        protected void DoTestHasEndingConditions(string iConditions, bool iExpected)
        {
            UnitItem vUnit = new UnitItem("unit_name");
            vUnit.EndingConditions = iConditions;
            DoTestHasConditions(iExpected, vUnit.HasEndingConditions, iConditions, vUnit.EndingConditions, "Ending");
        }

        protected void DoTestHasForm(string iForm, bool iExpected)
        {
            UnitItem vUnit = new UnitItem("Created with unit name only");
            vUnit.Form = iForm;
            DoTestHasForm(vUnit, iForm, iExpected);

            vUnit = new UnitItem("Created with empty path and form", "", false, iForm);
            DoTestHasForm(vUnit, iForm, iExpected);
        }

        protected void DoTestHasForm(UnitItem iUnitItem, string iExpectedFormName, bool iExpectedHasForm)
        {
            Assert.AreEqual(iExpectedHasForm, iUnitItem.HasForm, "HasForm value is wrong. " + iUnitItem.Name);
            Assert.AreEqual(iExpectedFormName, iUnitItem.Form, "Form value is wrong. " + iUnitItem.Name);
        }

        [Test]
        public void Create_NameAndPath()
        {
            UnitItem vUnit = new UnitItem("unit_name", "unit_path", true, "");
            Assert.AreEqual("unit_name", vUnit.Name, "Name wasn't set properly");
            Assert.AreEqual("unit_path", vUnit.Path, "Path wasn't set properly");
        }

        [Test]
        public void Create_Name()
        {
            DoTestCreate_Name("unit_name");
        }

        [Test]
        public void Create_EmptyName()
        {
            DoTestCreate_Name("");
        }

        [Test]
        public void NameChange()
        {
            DoTestNameChange("unit_name", "new_unit_name");
            DoTestNameChange("unit_name", "");
            DoTestNameChange("", "new_unit_name");
        }

        [Test]
        public void PathChange()
        {
            DoTestPathChange("unit_path", "new_unit_path");
        }

        [Test]
        public void HasStartingConditions()
        {
            DoTestHasStartingConditions("", false);
            DoTestHasStartingConditions("{$IFDEF test_condition}", true);
        }

        [Test]
        public void HasEndingConditions()
        {
            DoTestHasEndingConditions("", false);
            DoTestHasEndingConditions("{$ENDIF}", true);
        }

        [Test]
        public void HasForm()
        {
            DoTestHasForm("form1", true);
            DoTestHasForm("", false);
        }

        [Test]
        public void PathChange_ToEmpty()
        {
            DoTestPathChange("unit_path", "");
        }

        [Test]
        public void PathChange_FromEmpty()
        {
            DoTestPathChange("", "new_unit_path");
        }

        [Test]
        public void PathChange_ToSamePath()
        {
            DoTestPathChange("same_path", "same_path");
        }

        [Test]
        public void ToString_WithoutPath()
        {
            UnitItem vUnit = new UnitItem("unit_name");
            Assert.AreEqual("unit_name", vUnit.ToString("", "", false));
        }

        [Test]
        public void ToString_WithPath_WithoutBacklash()
        {
            UnitItem vUnit = new UnitItem("unit_name", @"c:\temp", true, "");
            string vExpected = "unit_name in 'c:\\temp\\unit_name.pas'";
            string vActual = vUnit.ToString("", "", false);
            Assert.AreEqual(vExpected, vActual);
        }

        [Test]
        public void ToString_WithPath_WithBackslash()
        {
            UnitItem vUnit = new UnitItem("unit_name", @"c:\temp\", true, "");
            string vExpected = "unit_name in 'c:\\temp\\unit_name.pas'";
            string vActual = vUnit.ToString("", "", false);
            Assert.AreEqual(vExpected, vActual);
        }

        [Test]
        public void ToString_RelativePath_WithBackslash()
        {
            UnitItem vUnit = new UnitItem("unit_name", @"..\..\", true, "");
            string vExpected = "unit_name in '..\\..\\unit_name.pas'";
            string vActual = vUnit.ToString("", "", false);
            Assert.AreEqual(vExpected, vActual);
        }

        [Test]
        public void ToString_RelativePath_WithoutBackslash()
        {
            UnitItem vUnit = new UnitItem("unit_name", @"..\..", true, "");
            string vExpected = "unit_name in '..\\..\\unit_name.pas'";
            string vActual = vUnit.ToString("", "", false);
            Assert.AreEqual(vExpected, vActual);
        }

        [Test]
        public void ToString_RelativePath_BlankPath()
        {
            UnitItem vUnit = new UnitItem("unit_name", "", true, "");
            string vExpected = "unit_name in 'unit_name.pas'";
            string vActual = vUnit.ToString("", "", false);
            Assert.AreEqual(vExpected, vActual);
        }

        [Test]
        public void ToString_WithConditions()
        {
            string vExpected =
                "  {$condition1}\n" +
                "    unit_name in 'unit_path\\unit_name.pas'\n" +
                "  {$condition2}";

            DoTestToString_WithConditions(true, "", false, vExpected);
        }

        [Test]
        public void ToString_WithSeparator_First()
        {
            string vExpected =
                "  {$condition1}\n" +
                "    unit_name in 'unit_path\\unit_name.pas',\n" +
                "  {$condition2}";

            DoTestToString_WithConditions(true, ",", false, vExpected);
        }

        [Test]
        public void ToString_WithSeparator_Last()
        {
            string vExpected =
                "  {$condition1}\n" +
                "    unit_name in 'unit_path\\unit_name.pas'\n" +
                "  {$condition2};";

            DoTestToString_WithConditions(true, ";", true, vExpected);
        }

        [Test]
        public void ToString_WithSeparator_WithoutConditions_First()
        {
            string vExpected = "  unit_name in 'unit_path\\unit_name.pas',";
            DoTestToString_WithConditions(false, ",", false, vExpected);
        }

        [Test]
        public void ToString_WithSeparator_WithoutConditions_Last()
        {
            string vExpected = "  unit_name in 'unit_path\\unit_name.pas';";
            DoTestToString_WithConditions(false, ";", true, vExpected);
        }

        [Test]
        public void ToString_StartingConditionOnly()
        {
            string vExpected =
                "  {$begin}\n" +
                "    unit_name;";

            UnitItem vUnit = new UnitItem("unit_name");
            vUnit.StartingConditions = "{$begin}";
            string vActual = vUnit.ToString("  ", ";", false);

            Assert.AreEqual(vExpected, vActual);
        }

        [Test]
        public void ToString_EndingConditionOnly()
        {
            string vExpected =
                "    unit_name,\n" +
                "  {$end}";

            UnitItem vUnit = new UnitItem("unit_name");
            vUnit.EndingConditions = "{$end}";
            string vActual = vUnit.ToString("  ", ",", false);

            Assert.AreEqual(vExpected, vActual);
        }

        [Test]
        public void ToString_WithForm()
        {
            string vExpected =
                "  unit_name {unit_name};";

            UnitItem vUnit = new UnitItem("unit_name", "", false, "unit_name");
            string vActual = vUnit.ToString("  ", ";", true);
            Assert.AreEqual(vExpected, vActual);
        }
    }
}
