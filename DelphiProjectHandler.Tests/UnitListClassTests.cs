using System;
using NUnit.Framework;

using DelphiProjectHandler;

namespace DelphiProjectHandler.Tests
{
    [TestFixture]
    public class UnitListClassTests
    {
        #region (Properties)

        protected UnitList TestList
        {
            get { return fTestList; }
        }
        private UnitList fTestList;

        #endregion
        #region (Helper methods)

        protected UnitList CreateList()
        {
            return new UnitList();
        }

        protected UnitList CreateDefaultList(int iCount, bool iWithPath, bool iWithConditions)
        {
            UnitList vList = CreateList();
            for (int vItemIdx = 0; vItemIdx < iCount; vItemIdx++)
            {
                UnitItem vUnit = CreateItem("DefaultName" + vItemIdx.ToString(), "DefaultPath" + vItemIdx.ToString(), iWithPath);
                if (iWithConditions)
                {
                    vUnit.StartingConditions = "{$starting_condition}";
                    vUnit.EndingConditions = "{$ending_condition}";
                }
                vList.Add(vUnit);
            }

            return vList;
        }

        protected UnitItem CreateItem(string iName, string iPath, bool iWithPath)
        {
            return new UnitItem(iName, iPath, iWithPath, "");
        }

        protected UnitItem CreateItem(string iPrefix, int iIndex, bool iWithPath)
        {
            return CreateItem(iPrefix + "Name" + iIndex.ToString(), iPrefix + "Path" + iIndex.ToString(), iWithPath);
        }

        #endregion
        #region (Test methods)

        protected void DoTestInsert(UnitList iList, UnitItem iUnitItem, int iIndex)
        {
            int vOldCount = iList.Count;
            iList.Insert(iIndex, iUnitItem);
            Assert.AreEqual(vOldCount + 1, iList.Count, "Item wasn't inserted properly");
            Assert.AreEqual(iUnitItem, iList[iIndex], "Item wasn't assigned correctly");
        }

        protected void DoTestContains_NotExisting(UnitList iList, string iUnitPrefix, int iUnitIdx)
        {
            UnitItem vNotExisting = CreateItem(iUnitPrefix, iUnitIdx, true);
            Assert.IsFalse(iList.Contains(vNotExisting.Name), "Wrong Contains result. Item shouldn't be on the list: " + vNotExisting.Name);
        }

        protected void DoTestToString_MultipleUnitsInConditionals(UnitList iList, int iStartingItemIdx, int iEndingItemIdx, string iExpected)
        {
            iList[iStartingItemIdx].StartingConditions = "{$starting_condition}";
            iList[iEndingItemIdx].EndingConditions = "{$ending_condition}";

            string vActual = iList.ToString();
            Assert.AreEqual(iExpected, vActual);
        }

        #endregion
        #region (Setup)

        [SetUp]
        protected void SetUp()
        {
            fTestList = CreateDefaultList(10, true, false);
        }

        [TearDown]
        protected void TearDown()
        {
            fTestList = null;
        }

        #endregion

        [Test]
        public void Create()
        {
            UnitList vList = CreateList();
            Assert.AreEqual(0, vList.Count, "List after creation is not empty");
        }

        [Test]
        public void Add()
        {
            UnitList vList = CreateList();

            for (int vItemIdx = 0; vItemIdx < 10; vItemIdx++)
            {
                UnitItem vExpected = CreateItem("unit ", vItemIdx, true);
                vList.Add(vExpected);
                Assert.AreEqual(vItemIdx + 1, vList.Count, "Unit item wasn't added to the list");
                Assert.AreEqual(vExpected, vList[vItemIdx], "Unit item wasn't set properly");
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Add_Duplicates()
        {
            UnitList vList = CreateList();
            UnitItem vItem = CreateItem("duplicated", 0, true);

            vList.Add(vItem);
            vList.Add(vItem);
        }

        [Test]
        public void Insert_AtTheBeginning()
        {
            for (int vItemIdx = 0; vItemIdx < 5; vItemIdx++)
            {
                UnitItem vExpected = CreateItem("unit ", vItemIdx, true);
                DoTestInsert(TestList, vExpected, 0);
            }
        }

        [Test]
        public void Insert_AtTheEnd()
        {
            for (int vItemIdx = 0; vItemIdx < 5; vItemIdx++)
            {
                UnitItem vExpected = CreateItem("unit ", vItemIdx, true);
                DoTestInsert(TestList, vExpected, TestList.Count);
            }
        }

        [Test]
        public void Remove_ByItem()
        {
            while (TestList.Count > 0)
            {
                UnitItem vToDelete = TestList[0];
                int vOldCount = TestList.Count;
                TestList.Remove(vToDelete);
                Assert.AreEqual(vOldCount - 1, TestList.Count, "Item wasn't deleted from the list");
                Assert.IsFalse(TestList.Contains(vToDelete), "Item wasn't deleted. Still exists on the list");
            }
        }

        [Test]
        public void Remove_ByName()
        {
            while (TestList.Count > 0)
            {
                UnitItem vToDelete = TestList[0];
                int vOldCount = TestList.Count;
                TestList.Remove(vToDelete.Name);
                Assert.AreEqual(vOldCount - 1, TestList.Count, "Item wasn't deleted from the list");
                Assert.IsFalse(TestList.Contains(vToDelete), "Item wasn't deleted. Still exists on the list");
            }
        }

        [Test]
        public void Set()
        {
            for (int vUnitIdx = 0; vUnitIdx < TestList.Count; vUnitIdx++)
            {
                UnitItem vExpected = CreateItem("NewItem", vUnitIdx, true);
                TestList[vUnitIdx] = vExpected;
                Assert.AreEqual(vExpected, TestList[vUnitIdx], "Item wasn't set properly");
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Set_Duplicated()
        {
            UnitList vList = CreateDefaultList(2, true, false);
            UnitItem vItem = CreateItem("duplicated", 0, true);

            vList[0] = vItem;
            vList[1] = vItem;
        }

        [Test]
        public void Contains_Existing()
        {
            for (int vUnitIdx = 0; vUnitIdx < TestList.Count; vUnitIdx++)
            {
                UnitItem vExpected = TestList[vUnitIdx];
                Assert.IsTrue(TestList.Contains(vExpected.Name), "Wrong Contains result. Item should be on the list");
            }
        }

        [Test]
        public void Contains_NonExisting()
        {
            for (int vUnitIdx = 0; vUnitIdx < TestList.Count; vUnitIdx++)
            {
                DoTestContains_NotExisting(TestList, "DiffDefault", vUnitIdx);
                DoTestContains_NotExisting(TestList, "Default", vUnitIdx + TestList.Count + 1);
            }
        }

        [Test]
        public void IndexOf_Name_Existing()
        {
            for (int vUnitIdx = 0; vUnitIdx < TestList.Count; vUnitIdx++)
            {
                UnitItem vExpected = TestList[vUnitIdx];
                Assert.AreEqual(vUnitIdx, TestList.IndexOf(vExpected.Name), "Wrong IndexOf by name result. Item should be found");
            }
        }

        [Test]
        public void IndexOf_Name_NonExisting()
        {
            for (int vUnitIdx = 0; vUnitIdx < TestList.Count; vUnitIdx++)
            {
                UnitItem vExpected = CreateItem("NotExisting", vUnitIdx, true);
                Assert.AreEqual(-1, TestList.IndexOf(vExpected.Name), "Wrong IndexOf by name result. Item shouldn't be on the list");
            }
        }

        [Test]
        public void IndexOf_NameStart_Existing()
        {
            for (int vUnitIdx = 0; vUnitIdx < TestList.Count; vUnitIdx++)
            {
                UnitItem vExpected = TestList[vUnitIdx];
                Assert.AreEqual(vUnitIdx, TestList.IndexOf(vExpected.Name, vUnitIdx), "Wrong IndexOf by name result. Item should be found");
                Assert.AreEqual(-1, TestList.IndexOf(vExpected.Name, vUnitIdx + 1), "Wrong IndexOf by name result. Item shouldn't be found");
            }
        }

        [Test]
        public void IndexOf_NameStart_NonExisting()
        {
            for (int vUnitIdx = 0; vUnitIdx < TestList.Count; vUnitIdx++)
            {
                UnitItem vExpected = CreateItem("NotExisting", vUnitIdx, true);
                Assert.AreEqual(-1, TestList.IndexOf(vExpected.Name, vUnitIdx), "Wrong IndexOf by name result. Item shouldn't be on the list");
            }
        }

        [Test]
        public void IndexOf_NameStartCount_Existing()
        {
            for (int vUnitIdx = 0; vUnitIdx < TestList.Count; vUnitIdx++)
            {
                UnitItem vExpected = TestList[vUnitIdx];
                Assert.AreEqual(vUnitIdx, TestList.IndexOf(vExpected.Name, vUnitIdx, 1), "Wrong IndexOf by name result");
                Assert.AreEqual(-1, TestList.IndexOf(vExpected.Name, vUnitIdx + 1, 1), "Wrong IndexOf by name result. Item shouldn't be found. vUnitIdx + 1");
                Assert.AreEqual(-1, TestList.IndexOf(vExpected.Name, vUnitIdx, 0), "Wrong IndexOf by name result. Item shouldn't be found. Count = 0");
            }
        }

        [Test]
        public void IndexOf_NameStartCount_NonExisting()
        {
            for (int vUnitIdx = 0; vUnitIdx < TestList.Count; vUnitIdx++)
            {
                UnitItem vExpected = CreateItem("NotExisting", vUnitIdx, true);
                Assert.AreEqual(-1, TestList.IndexOf(vExpected.Name, vUnitIdx, 1), "Wrong IndexOf by name result. Item shouldn't be on the list");
            }
        }

        [Test]
        public void LastIndexOf_Name_Existing()
        {
            for (int vUnitIdx = 0; vUnitIdx < TestList.Count; vUnitIdx++)
            {
                UnitItem vExpected = TestList[vUnitIdx];
                Assert.AreEqual(vUnitIdx, TestList.LastIndexOf(vExpected.Name), "Wrong IndexOf by name result. Item should be found");
            }
        }

        [Test]
        public void LastIndexOf_Name_NonExisting()
        {
            for (int vUnitIdx = 0; vUnitIdx < TestList.Count; vUnitIdx++)
            {
                UnitItem vExpected = CreateItem("NotExisting", vUnitIdx, true);
                Assert.AreEqual(-1, TestList.LastIndexOf(vExpected.Name), "Wrong IndexOf by name result. Item shouldn't be on the list");
            }
        }

        [Test]
        public void LastIndexOf_NameStart_Existing()
        {
            for (int vUnitIdx = 0; vUnitIdx < TestList.Count; vUnitIdx++)
            {
                UnitItem vExpected = TestList[vUnitIdx];
                Assert.AreEqual(vUnitIdx, TestList.LastIndexOf(vExpected.Name, vUnitIdx), "Wrong IndexOf by name result. Item should be found");
                Assert.AreEqual(-1, TestList.LastIndexOf(vExpected.Name, vUnitIdx - 1), "Wrong IndexOf by name result. Item shouldn't be found");
            }
        }

        [Test]
        public void LastIndexOf_NameStart_NonExisting()
        {
            for (int vUnitIdx = 0; vUnitIdx < TestList.Count; vUnitIdx++)
            {
                UnitItem vExpected = CreateItem("NotExisting", vUnitIdx, true);
                Assert.AreEqual(-1, TestList.LastIndexOf(vExpected.Name, vUnitIdx), "Wrong IndexOf by name result. Item shouldn't be on the list");
            }
        }

        [Test]
        public void LastIndexOf_NameStartCount_Existing()
        {
            for (int vUnitIdx = 0; vUnitIdx < TestList.Count; vUnitIdx++)
            {
                UnitItem vExpected = TestList[vUnitIdx];
                Assert.AreEqual(vUnitIdx, TestList.LastIndexOf(vExpected.Name, vUnitIdx, 1), "Wrong IndexOf by name result");
                Assert.AreEqual(-1, TestList.LastIndexOf(vExpected.Name, vUnitIdx - 1, 1), "Wrong IndexOf by name result. Item shouldn't be found. vUnitIdx + 1");
                Assert.AreEqual(-1, TestList.LastIndexOf(vExpected.Name, vUnitIdx, 0), "Wrong IndexOf by name result. Item shouldn't be found. Count = 0");
            }
        }

        [Test]
        public void LastIndexOf_NameStartCount_NonExisting()
        {
            for (int vUnitIdx = 0; vUnitIdx < TestList.Count; vUnitIdx++)
            {
                UnitItem vExpected = CreateItem("NotExisting", vUnitIdx, true);
                Assert.AreEqual(-1, TestList.LastIndexOf(vExpected.Name, vUnitIdx, 1), "Wrong IndexOf by name result. Item shouldn't be on the list");
            }
        }

        [Test]
        public void ToString_WithPaths()
        {
            string vExpected =
                "uses\n" +
                "  DefaultName0 in 'DefaultPath0\\DefaultName0.pas',\n" +
                "  DefaultName1 in 'DefaultPath1\\DefaultName1.pas';";

            UnitList vList = CreateDefaultList(2, true, false);
            string vActual = vList.ToString();
            Assert.AreEqual(vExpected, vActual);
        }

        [Test]
        public void ToString_WithoutPaths()
        {
            string vExpected =
                "uses\n" +
                "  DefaultName0,\n" +
                "  DefaultName1;";

            UnitList vList = CreateDefaultList(2, false, false);
            string vActual = vList.ToString();
            Assert.AreEqual(vExpected, vActual);
        }

        [Test]
        public void ToString_WithPaths_WithConditions()
        {
            string vExpected =
                "uses\n" +
                "  {$starting_condition}\n" +
                "    DefaultName0 in 'DefaultPath0\\DefaultName0.pas',\n" +
                "  {$ending_condition}\n" +
                "  {$starting_condition}\n" +
                "    DefaultName1 in 'DefaultPath1\\DefaultName1.pas'\n" + 
                "  {$ending_condition};";

            UnitList vList = CreateDefaultList(2, true, true);
            string vActual = vList.ToString();
            Assert.AreEqual(vExpected, vActual);
        }

        [Test]
        public void ToString_WithoutPaths_WithConditions()
        {
            string vExpected =
                "uses\n" +
                "  {$starting_condition}\n" +
                "    DefaultName0,\n" +
                "  {$ending_condition}\n" +
                "  {$starting_condition}\n" +
                "    DefaultName1\n" +
                "  {$ending_condition};";

            UnitList vList = CreateDefaultList(2, false, true);
            string vActual = vList.ToString();
            Assert.AreEqual(vExpected, vActual);
        }

        [Test]
        public void ToString_MultipleUnitsInConditionals_AtEnd()
        {
            string vExpected =
                "uses\n" +
                "  DefaultName0,\n" +
                "  {$starting_condition}\n" +
                "    DefaultName1,\n" +
                "    DefaultName2\n" +
                "  {$ending_condition};";

            UnitList vList = CreateDefaultList(3, false, false);
            DoTestToString_MultipleUnitsInConditionals(vList, 1, 2, vExpected);
        }

        [Test]
        public void ToString_MultipleUnitsInConditionals_AtBeginning()
        {
            string vExpected =
                "uses\n" +
                "  {$starting_condition}\n" +
                "    DefaultName0,\n" +
                "    DefaultName1,\n" +
                "  {$ending_condition}\n" +
                "  DefaultName2;";

            UnitList vList = CreateDefaultList(3, false, false);
            DoTestToString_MultipleUnitsInConditionals(vList, 0, 1, vExpected);
        }
    }
}
