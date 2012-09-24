/*
 * Created by SharpDevelop.
 * User: SG0894652
 * Date: 8/10/2009
 * Time: 9:01 PM
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
	public class ListUtilsTests
	{
		#region (Test methods)
		
		protected void CompareLists<T>(IList<T> iActual, IList<T> iExpected, string iErrorMsg)
		{
			Assert.AreEqual(iExpected.Count, iActual.Count, "Different no. of items returned.");
			for (int vItemIdx = 0; vItemIdx < iActual.Count; vItemIdx++)
				Assert.AreEqual(iExpected[vItemIdx], iActual[vItemIdx], "Different items found. Item: " + vItemIdx.ToString() + ". " + iErrorMsg);
		}
		
		protected void TestRemoveDuplicates<T>(T[] iValues, T[] iExpected, string iErrorMsg)
		{
			IList<T> vActual = ListUtils.RemoveDuplicates<T>(iValues);
			CompareLists(vActual, iExpected, iErrorMsg);		
		}

		protected void TestNormalize<T>(T[] iValues, T[] iExpected, string iErrorMsg)
		{
			IList<T> vActual = ListUtils.Normalize<T>(iValues);
			CompareLists(vActual, iExpected, iErrorMsg);
		}
		
		#endregion
		
		[Test]
		public void RemoveDuplicates()
		{
			TestRemoveDuplicates<int>(
				new int[] {123, 234, 345, 456, 123, 123, 234, 234, 345, 345, 456, 456},
				new int[] {123, 234, 345, 456}, "Test 1"
			);
			TestRemoveDuplicates<string>(
				new string[] {"123", "234", "345", "abc", "def", "ghi", "123", "234", "345", "abc", "def", "ghi", "123", "234", "345", "abc", "def", "ghi", null, null},
				new string[] {"123", "234", "345", "abc", "def", "ghi", null}, "Test 2");
		}
		
		[Test]
		public void Normalize()
		{
			TestNormalize<int>(
				new int[] {123, 234, 345, 456, 123, 123, 234, 234, 345, 345, 456, 456},
				new int[] {123, 234, 345, 456}, "Test 1"
			);
			TestNormalize<string>(
				new string[] {"123", "234", "345", "abc", "def", "ghi", "123", "234", "345", "abc", "def", "ghi", "123", "234", "345", "abc", "def", "ghi", null, null},
				new string[] {"123", "234", "345", "abc", "def", "ghi", null}, "Test 2");
		}
	}
}
