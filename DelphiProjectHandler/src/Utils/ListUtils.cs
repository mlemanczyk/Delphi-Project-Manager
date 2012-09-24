/*
 * Created by SharpDevelop.
 * User: SG0894652
 * Date: 8/10/2009
 * Time: 8:34 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace DelphiProjectHandler.Dialogs
{
	/// <summary>
	/// Contains basic methods for manipulating list. E.g. removing duplicates, removing empty strings etc.
	/// </summary>
	public class ListUtils
	{
        public static IList<T> Normalize<T>(IList<T> iList)
        {
            return RemoveDuplicates(iList);
        }

        public static IList<T> RemoveDuplicates<T>(IList<T> iList)
		{
			List<T> vResult = new List<T>(iList.Count);
			vResult.AddRange(iList);
			for (int vItemIdx = vResult.Count - 1; vItemIdx >= 0; vItemIdx--)
			{
				T vItem = vResult[vItemIdx];
				for (int vCompareIdx = vItemIdx - 1; vCompareIdx >= 0; vCompareIdx--)
				{
					if ((vItem == null && vResult[vCompareIdx] == null) || (vItem != null && vItem.Equals(vResult[vCompareIdx])))
					{
						vResult.RemoveAt(vItemIdx);
						break;
					}
				}
			}
			return vResult;
		}
	}
}
