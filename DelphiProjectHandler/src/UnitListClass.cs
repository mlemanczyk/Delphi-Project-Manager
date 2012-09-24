using System;
using System.Collections;
using System.Collections.Generic;

namespace DelphiProjectHandler
{
    public class UnitList: ArrayList, IList<string>
    {
        protected int IndexOf(string iUnitName, int iStartIndex, int iCount, bool iDescending)
        {
            iUnitName = iUnitName.ToLower();

            int vOffset;
            if (!iDescending)
                vOffset = 1;
            else
                vOffset = -1;

            for (int vUnitIdx = 0, vCurrIdx = iStartIndex; vUnitIdx < iCount; vUnitIdx++, vCurrIdx += vOffset)
            {
                if (vCurrIdx < 0 || vCurrIdx >= Count)
                    break;

                UnitItem vUnitItem = this[vCurrIdx];
                if (vUnitItem.Name.ToLower().Equals(iUnitName))
                    return vCurrIdx;
            }

            return -1;
        }

        protected void ValidateArgument(object iUnitItem, string iArgumentName)
        {
            if (!(iUnitItem is UnitItem))
                throw new ArgumentOutOfRangeException(iArgumentName, "Argument must be UnitItem");

            string vUnitName = ((UnitItem) iUnitItem).Name;
            if (Contains(vUnitName))
                throw new ArgumentException("Unit \"" + vUnitName + "\" is already on the list", iArgumentName);
        }

        public new UnitItem this[int iIndex]
        {
            get { return (UnitItem) base[iIndex]; }
            set 
            {
                ValidateArgument(value, "value");
                base[iIndex] = value; 
            }
        }

        public override int Add(object iUnitItem)
        {
            ValidateArgument(iUnitItem, "iUnitItem");
            return base.Add(iUnitItem);
        }

        public bool Contains(string iUnitName)
        {
            return IndexOf(iUnitName) >= 0;
        }

        public int IndexOf(string iUnitName, int iStartIndex, int iCount)
        {
            return IndexOf(iUnitName, iStartIndex, iCount, false);
        }
        
        public int IndexOf(string iUnitName, int iStartIndex)
        {
            return IndexOf(iUnitName, iStartIndex, Count - iStartIndex);
        }

        public int IndexOf(string iUnitName)
        {
            return IndexOf(iUnitName, 0, Count);
        }

        public override void Insert(int iIndex, object iUnitItem)
        {
            ValidateArgument(iUnitItem, "iValue");
            base.Insert(iIndex, iUnitItem);
        }

        public int LastIndexOf(string iUnitName)
        {
            return LastIndexOf(iUnitName, Count - 1, Count);
        }

        public int LastIndexOf(string iUnitName, int iStartIndex)
        {
            return LastIndexOf(iUnitName, iStartIndex, Count - iStartIndex);
        }

        public int LastIndexOf(string iUnitName, int iStartIndex, int iCount)
        {
            return IndexOf(iUnitName, iStartIndex, iCount, true);
        }

        public bool Remove(string iUnitName)
        {
            int vUnitIdx = IndexOf(iUnitName);
            if (vUnitIdx < 0)
                return false;

            RemoveAt(vUnitIdx);
            return true;
        }

        public override string ToString()
        {
            string vResult = "";
            if (Count == 0)
                return vResult;

            string vSeparator = ",";
            bool vSeparatorLast = false;

            vResult = "uses\n";
            for (int vUnitIdx = 0; vUnitIdx < Count; vUnitIdx++)
            {
                UnitItem vUnit = this[vUnitIdx];

                if (vUnitIdx == Count - 1)
                {
                    vSeparator = ";";
                    vSeparatorLast = true;
                }

                vResult += this[vUnitIdx].ToString("  ", vSeparator, vSeparatorLast) + "\n";
            }

            return vResult.TrimEnd('\n');
        }

        #region ICollection<string> Members

        void ICollection<string>.Add(string item)
        {
            Add(new UnitItem(item));
        }

        void ICollection<string>.CopyTo(string[] array, int arrayIndex)
        {
            string[] vResult = new string[Count];
            for (int vItemIdx = 0; vItemIdx < Count; vItemIdx++)
                vResult[vItemIdx] = this[vItemIdx].Name;

            vResult.CopyTo(array, arrayIndex);
        }

        #endregion

        protected class UnitListEnumerator : IEnumerator<string>
        {
            protected UnitList List;
            protected int CurrentIdx = -1;

            public UnitListEnumerator(UnitList iList)
            {
                List = new UnitList();
                List.AddRange(iList);
            }

            #region IEnumerator<string> Members

            public string Current
            {
                get
                {
                    if (CurrentIdx >= 0 && CurrentIdx < List.Count)
                        return List[CurrentIdx].Name;
                    else
                        return null;
                }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                List = null;
            }

            #endregion

            #region IEnumerator Members

            object IEnumerator.Current
            {
                get 
                {
                    if (CurrentIdx >= 0 && CurrentIdx < List.Count)
                        return List[CurrentIdx].Name;
                    else
                        return null;
                }
            }

            public bool MoveNext()
            {
                if (CurrentIdx >= List.Count - 1)
                    return false;

                CurrentIdx++;
                return true;
            }

            public void Reset()
            {
                if (List.Count > 0)
                    CurrentIdx = 0;
                else
                    CurrentIdx = -1;
            }

            #endregion
        }

        #region IList<string> Members


        void IList<string>.Insert(int index, string item)
        {
            Insert(index, new UnitItem(item));
        }

        string IList<string>.this[int index]
        {
            get
            {
                return this[index].Name;
            }
            set
            {
                this[index].Name = value;
            }
        }

        #endregion

        #region IEnumerable<string> Members

        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            return new UnitListEnumerator(this);
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new UnitListEnumerator(this);
        }

        #endregion
    }
}
