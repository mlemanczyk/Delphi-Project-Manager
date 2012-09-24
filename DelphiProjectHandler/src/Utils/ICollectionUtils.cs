using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelphiProjectHandler.Utils
{
    public static class CollectionUtils
    {
        public static void Add<T>(this ICollection<T> aList, ICollection<T> aListToAppend)
        {
            foreach (T vObject in aListToAppend)
                aList.Add(vObject);
        }
    }
}
