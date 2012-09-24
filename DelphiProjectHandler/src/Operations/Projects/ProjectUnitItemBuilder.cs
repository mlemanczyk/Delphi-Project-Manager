using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DelphiProjectHandler.Dialogs;

namespace DelphiProjectHandler.Operations.Projects
{
    /// <summary>
    /// UnitItem builder for creating complete project file unit item. New UnitItem will have all properties set that are normally used in a project Delphi file. It will also 
    /// search for corresponding dfm file.
    /// </summary>
    class ProjectUnitItemBuilder: IUnitItemBuilder
    {
        /// <summary>
        /// Complete fileName or path of the project file. It is used to calculate new unit item path relative to project file location.
        /// </summary>
        public string BasePath
        {
            get { return fBasePath; }
        }
        private string fBasePath;

        /// <summary>
        /// Indicates, if unit paths should be added in a project file style, when converting the unit list to uses clause.
        /// </summary>
        public bool UsePaths
        {
            get { return fUsePaths; }
        }
        private bool fUsePaths;

        /// <summary>
        /// Creates new instance of the ProjectItemBuilder.
        /// </summary>
        /// <param name="aBasePath">Complete fileName or path of the project file. It is used to calculate new unit item path relative to project file location.</param>
        public ProjectUnitItemBuilder(string aBasePath, bool aUsePaths)
        {
            fBasePath = aBasePath;
            fUsePaths = aUsePaths;
        }

        /// <summary>
        /// Creates new UnitItem object and fills it will all required information that can be used in a project file. Requires valid RelativePath builder and UnitFormFinder.
        /// It finds corresponding form for the unit automatically. Path is always set relatively to the BasePath.
        /// </summary>
        /// <param name="aUnitFileName"></param>
        /// <returns></returns>
        public UnitItem Create(string aUnitFileName)
        {
            UnitItem vResult = new UnitItem();
            vResult.Name = Path.GetFileNameWithoutExtension(aUnitFileName);
            vResult.UsePath = UsePaths;
            if (!aUnitFileName.Equals(""))
                vResult.Path = Path.GetDirectoryName(aUnitFileName);
            vResult.Path = RelativePath.GetRelativeFileName(BasePath, vResult.Path);
            vResult.Form = UnitFormReader.GetUnitForm(aUnitFileName);

            return vResult;
        }
    }
}
