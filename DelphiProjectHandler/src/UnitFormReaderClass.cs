using System;
using System.IO;
using System.Text.RegularExpressions;

namespace DelphiProjectHandler
{
    public class UnitFormReader
    {
        protected static string cFormName = @"(inherited|object)\s+([^:]*):";

        protected static string FindAndLoadFormFile(string iUnitFileName)
        {
            string vFormFileName = Path.ChangeExtension(iUnitFileName, ".dfm");
            if (!File.Exists(vFormFileName))
                throw new FileNotFoundException("File was not found.", vFormFileName);

            return File.ReadAllText(vFormFileName);
        }

        public static string GetUnitForm(string iUnitFileName)
        {
            string vResult = "";

            string vFileContent = "";
            try
            {
                vFileContent = FindAndLoadFormFile(iUnitFileName);
            }
            catch (FileNotFoundException)
            {
                return vResult;
            }

            Regex vRegExpr = new Regex(cFormName);
            if (vRegExpr.IsMatch(vFileContent))
                vResult = vRegExpr.Match(vFileContent).Groups[2].Value;
            else
                vResult = GetBinaryFormName(vFileContent);

            return vResult;
        }

        public static string GetBinaryFormName(string iFileContent)
        {
            int vPos = iFileContent.IndexOf("TPF0");
            if (vPos < 0)
                throw new Exception("Can't read form name from DFM file.");

            vPos += 5;

            // Find 1st character of the name
            while (vPos < iFileContent.Length)
            {
                byte vByte = Convert.ToByte(iFileContent[vPos]);
                if (
                    (vByte >= 65 && vByte <= 90) ||
                    (vByte >= 97 && vByte <= 122)
                   )
                    break;

                vPos++;
            }
            if (vPos == iFileContent.Length)
                throw new Exception("Can't read form name from binary DFM file.");

            int vStart = vPos;
            // Start reading name
            while (vPos < iFileContent.Length)
            {
                byte vByte = Convert.ToByte(iFileContent[vPos]);
                if (
                    (vByte < 48 || vByte > 57) &&
                    (vByte < 65 || vByte > 90) &&
                    (vByte < 97 || vByte > 122) &&
                    (iFileContent[vPos] != '_')
                   )
                    break;

                vPos++;
            }

            if (vPos == iFileContent.Length)
                throw new Exception("Can't read form name from binary DFM file.");

            string vFormTypeName = iFileContent.Substring(vStart, vPos - vStart);
            if (vFormTypeName.StartsWith("T"))
                vFormTypeName = vFormTypeName.Substring(1);

            return vFormTypeName;
        }
    }
}
