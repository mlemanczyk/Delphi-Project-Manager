using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DelphiProjectHandler.Operations.Units
{
    public class DelphiUnitItemBuilder: IUnitItemBuilder
    {
        public UnitItem Create(string aUnitFileName)
        {
            UnitItem vResult = new UnitItem();
            vResult.Name = Path.GetFileNameWithoutExtension(aUnitFileName);

            return vResult;
        }
    }
}
