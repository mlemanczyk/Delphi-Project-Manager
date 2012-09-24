/*
 * Created by SharpDevelop.
 * User: SG0894652
 * Date: 8/10/2009
 * Time: 8:45 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Globalization;

namespace DelphiProjectHandler.Dialogs
{
	public class DescendingComparer: IComparer<string>
	{
		protected StringComparer Comparer
		{
			get { return fComparer; }
		}
		private StringComparer fComparer;
		
		public DescendingComparer(CultureInfo iCulture, bool iIgnoreCase)
		{
			
			fComparer = StringComparer.Create(iCulture, iIgnoreCase);
		}		

		public int Compare(string x, string y)
		{
			return -Comparer.Compare(x, y);
		}
	}
	/// <summary>
	/// Description of StringListUtils.
	/// </summary>
	public class StringListUtils
	{
		public static IList<string> RemoveEmpty(IList<string> iStrings)
		{
			List<string> vResult = new List<string>(iStrings.Count);
			foreach (string vString in iStrings)
			{
				if (vString != null && !vString.Trim().Equals(""))
					vResult.Add(vString);
			}
			
			return vResult;
		}
		
		public static IList<string> ToLower(IList<string> iStrings)
		{
			List<string> vResult = new List<string>(iStrings.Count);
			foreach (string vString in iStrings)
			{
				string vNewValue = vString;
				if (vNewValue != null)
					vNewValue = vNewValue.ToLower();
				vResult.Add(vNewValue);
			}
			
			return vResult;
		}
		
		public static IList<string> ToUpper(IList<string> iStrings)
		{
			List<string> vResult = new List<string>(iStrings.Count);
			foreach (string vString in iStrings)
			{
				string vNewValue = vString;
				if (vNewValue != null)
					vNewValue = vNewValue.ToUpper();
				vResult.Add(vNewValue);
			}
			
			return vResult;
		}
		
		public static IList<string> RemoveDuplicates(IList<string> iStrings)
		{
			return ListUtils.RemoveDuplicates<string>(iStrings);
		}
		
		public static IList<string> Normalize(IList<string> iList)
		{
			IList<string> vResult = RemoveEmpty(iList);
			vResult = ToLower(vResult);
			vResult = ListUtils.Normalize<string>(vResult);
			
			return Sort(vResult, true, false);
		}
		
		public static IList<string> Sort(IList<string> iStrings, bool iAscending, bool iIgnoreCase)
		{
			List<string> vResult = new List<string>(iStrings.Count);
			vResult.AddRange(iStrings);
			switch (iAscending)
			{
				case true:
					vResult.Sort(StringComparer.Create(CultureInfo.CurrentCulture, iIgnoreCase));
					break;
				case false:
					vResult.Sort(new DescendingComparer(CultureInfo.CurrentCulture, iIgnoreCase));
					break;
			}
			return vResult;
		}
	}
}
