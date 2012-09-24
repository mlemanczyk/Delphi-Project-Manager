using System;
using System.IO;

namespace DelphiProjectHandler
{
    public class UnitItem
    {
        /// <summary>
        /// Name of the unit in uses clause.
        /// </summary>
        public string Name
        {
            get { return fName; }
            set { fName = value; }
        }
        protected string fName;

        /// <summary>
        /// Path, where the unit is placed - if it's given with the unit name.
        /// </summary>
        public string Path
        {
            get { return fPath; }
            set { fPath = value; }
        }
        protected string fPath = "";

        /// <summary>
        /// Indicates, if the path is included together with the unit name
        /// </summary>
        public bool UsePath
        {
            get { return fUsePath; }
            set { fUsePath = value; }
        }
        protected bool fUsePath;

        public bool HasForm
        {
            get { return Form != ""; }
        }

        public string Form
        {
            get { return fForm; }
            set { fForm = value; }
        }
        protected string fForm = "";

        public string StartingConditions
        {
            get { return fStartingConditions; }
            set { fStartingConditions = value.TrimEnd(' ', '\n').TrimStart('\n'); }
        }
        protected string fStartingConditions = "";

        public string EndingConditions
        {
            get { return fEndingConditions; }
            set 
            {
                fEndingConditions = value.TrimEnd(' ', '\n');
                fEndingConditions = EndingConditions.TrimStart(' ', '\n'); 
            }
        }
        protected string fEndingConditions = "";

        public bool HasStartingConditions
        {
            get { return (StartingConditions != ""); }
        }

        public bool HasEndingConditions
        {
            get { return (EndingConditions != ""); }
        }

        /// <summary>
        /// Initializes the object with given unit name and path.
        /// </summary>
        /// <param name="iName">Name of the unit in the uses clause.</param>
        /// <param name="iPath">Path to the unit in the uses clause.</param>
        protected void Initialize(string iName, string iPath, bool iUsePath, string iForm)
        {
            fName = iName;
            UsePath = iUsePath;
            if (UsePath)
                Path = iPath;
            Form = iForm;
        }

        /// <summary>
        /// Creates new instance of the unit information object.
        /// </summary>
        /// <param name="iName">Name of the unit in the uses clause.</param>
        /// <param name="iPath">Path to the unit in the uses clause.</param>
        /// <param name="iUsePath">Indicates, if the path is included together
        /// with the unit name.</param>
        public UnitItem(string iName, string iPath, bool iUsePath, string iForm)
        {
            Initialize(iName, iPath, iUsePath, iForm);
        }

        /// <summary>
        /// Creates new instance of the unit information object.
        /// </summary>
        /// <param name="iName">Name of the unit in the uses clause.</param>
        public UnitItem(string iName)
        {
            Initialize(iName, "", false, "");
        }

        public UnitItem()
        {
            Initialize("", "", false, "");
        }

        /// <summary>
        /// Returns the unit name and path (if given) as a string, which can be used
        /// to construct "uses" clause in Delphi.
        /// </summary>
        /// <param name="iIndent">Indention of the output value.</param>
        /// <param name="iUnitSeparator">Unit separator that must be added after unit 
        /// name and/or path. Usually it is collon, but the last unit is separated
        /// with semicolon</param>
        /// <param name="iSeparatorLast">Specifies, if unit separator must be added
        /// as the last character in the output value, after any conditionals. If not,
        /// it will be added before EndingConditionals.</param>
        /// <returns>Unit name with unit path (if given)</returns>
        public string ToString(string iIndent, string iUnitSeparator, bool iSeparatorLast)
        {
            string vResult = "";
            if (StartingConditions != "")
                vResult += iIndent + StartingConditions + "\n";

            if (StartingConditions != "" || EndingConditions != "")
                vResult += iIndent;

            vResult += iIndent + Name;
            if (UsePath)
                vResult += " in '" + System.IO.Path.Combine(Path, Name) + ".pas'";
            if (HasForm)
                vResult += " {" + Form + "}";
            if (!iSeparatorLast)
                vResult += iUnitSeparator;

            if (EndingConditions != "")
                vResult += "\n" + iIndent + EndingConditions;
            if (iSeparatorLast)
                vResult += iUnitSeparator;

            return vResult;
        }


    }
}
