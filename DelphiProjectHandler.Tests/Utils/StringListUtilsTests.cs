/*
 * Created by SharpDevelop.
 * User: SG0894652
 * Date: 8/10/2009
 * Time: 9:20 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using NUnit.Framework;

using DelphiProjectHandler.Dialogs;

namespace DelphiProjectHandler.Tests.Utils
{
	[TestFixture]
	public class StringListUtilsTests
	{
		#region (Test methods)
		
		protected void CompareLists(IList<string> iActual, IList<string> iExpected, string iErrorMsg)
		{
			Assert.AreEqual(iExpected.Count, iActual.Count, "Different no. of items returned.");
			for (int vItemIdx = 0; vItemIdx < iActual.Count; vItemIdx++)
				Assert.AreEqual(iExpected[vItemIdx], iActual[vItemIdx], "Different items found. Item: " + vItemIdx.ToString() + ". " + iErrorMsg);
		}
		
		protected void TestRemoveEmpty(IList<string> iStrings, IList<string> iExpected, string iErrorMsg)
		{
			IList<string> vActual = StringListUtils.RemoveEmpty(iStrings);
			CompareLists(vActual, iExpected, iErrorMsg);
		}
		
		protected void TestToLower(IList<string> iStrings, IList<string> iExpected, string iErrorMsg)
		{
			IList<string> vActual = StringListUtils.ToLower(iStrings);
			CompareLists(vActual, iExpected, iErrorMsg);
		}
		
		protected void TestToUpper(IList<string> iStrings, IList<string> iExpected, string iErrorMsg)
		{
			IList<string> vActual = StringListUtils.ToUpper(iStrings);
			CompareLists(vActual, iExpected, iErrorMsg);
		}
		
		protected void TestRemoveDuplicates(IList<string> iStrings, IList<string> iExpected, string iErrorMsg)
		{
			IList<string> vActual = StringListUtils.RemoveDuplicates(iStrings);
			CompareLists(vActual, iExpected, iErrorMsg);
		}
		
		protected void TestNormalize(IList<string> iStrings, IList<string> iExpected, string iErrorMsg)
		{
			IList<string> vActual = StringListUtils.Normalize(iStrings);
			CompareLists(vActual, iExpected, iErrorMsg);
		}
		
		protected void TestSort(IList<string> iStrings, IList<string> iExpected, bool iAscending, bool iIgnoreCase, string iErrorMsg)
		{
			IList<string> vActual = StringListUtils.Sort(iStrings, iAscending, iIgnoreCase);
			CompareLists(vActual, iExpected, iErrorMsg);
		}
		
		#endregion
		
		[Test]
		public void RemoveEmpty()
		{
			TestRemoveEmpty(
				new string[] {"", "1", " ", "2", "  ", "3 ", null, "4", null, "5"},
				new string[] {"1", "2", "3 ", "4", "5"}, "Test 1"
			);
		}
		
		[Test]
		public void ToLower()
		{
			TestToLower(
				new string[] {"ABC", null, "DeF", "", "ghi"},
				new string[] {"abc", null, "def", "", "ghi"}, "Test 1"
			);
		}

		[Test]
		public void ToUpper()
		{
			TestToUpper(
				new string[] {"ABC", null, "DeF", "", "ghi"},
				new string[] {"ABC", null, "DEF", "", "GHI"}, "Test 1"
			);
		}
		
		[Test]
		public void Normalize()
		{
			TestNormalize(
				new string[] {"ABC", null, "DeF", "", "ghi", "abc", "def", "", "ghi", null},
				new string[] {"abc", "def", "ghi"}, "Test 1"
			);
		}

		[Test]
		public void RemoveDuplicates()
		{
			TestRemoveDuplicates(
				new string[] {"123", "234", "345", "abc", "def", "ghi", "123", "234", "345", "abc", "def", "ghi", "123", "234", "345", "abc", "def", "ghi", null, null},
				new string[] {"123", "234", "345", "abc", "def", "ghi", null}, "Test 2");
		}
		
		[Test]
		public void Sort()
		{
			TestSort(
				new string[] {"g", "f", "e", "d", "c", "b", "a", "G", "F", "E", "D", "C", "B", "A"},
				new string[] {"A", "a", "b", "B", "C", "c", "D", "d", "E", "e", "f", "F", "g", "G"},
				true, true, "Test 1"
			);

			TestSort(
				new string[] {"A", "a", "B", "b", "C", "c", "D", "d", "E", "e", "F", "f", "G", "g"},
				new string[] {"G", "g", "F", "f", "E", "e", "d", "D", "C", "c", "B", "b", "A", "a"},
				false, true, "Test 2"
			);

			TestSort(
				new string[] {"g", "f", "e", "d", "c", "b", "a", "G", "F", "E", "D", "C", "B", "A"},
				new string[] {"a", "A", "b", "B", "c", "C", "d", "D", "e", "E", "f", "F", "g", "G"},
				true, false, "Test 3"
			);

			TestSort(
				new string[] {"A", "a", "B", "b", "C", "c", "D", "d", "E", "e", "F", "f", "G", "g"},
				new string[] {"G", "g", "F", "f", "E", "e", "D", "d", "C", "c", "B", "b", "A", "a"},
				false, false, "Test 4"
			);
		}
	}
}
