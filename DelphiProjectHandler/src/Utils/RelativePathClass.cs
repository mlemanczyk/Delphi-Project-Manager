using System;
using System.IO;

using DelphiProjectHandler.Dialogs;

namespace DelphiProjectHandler.Dialogs
{
    public class RelativePath
    {
        /// <summary>
        /// Changes the given path to relative path according to given base folder.
        /// It doesn't validates, if the calculation may be made.
        /// </summary>
        /// <param name="iBasePath">Base folder for the path.</param>
        /// <param name="iPath">Folder which must be changed to relative.</param>
        /// <returns>Relative path according to base folder.</returns>
        private static string BuildRelativePath(string iBasePath, string iPath)
        {
            string[] vBaseDirs = PathUtils.SplitPath(iBasePath);
            string[] vRelativeDirs = PathUtils.SplitPath(iPath);

            int vFolderIdx = PathUtils.GetFirstDifferentFolderIdx(vBaseDirs, vRelativeDirs);

            string vBasePath = PathUtils.BuildGoUpPath(vBaseDirs.Length - vFolderIdx);
            string vRelativePathSuffix = PathUtils.ConcatFolders(vFolderIdx, vRelativeDirs);

            return Path.Combine(vBasePath, vRelativePathSuffix);
        }

        /// <summary>
        /// Checks, if the given path is can be made relative. Path may not be changed
        /// to relative if:
        /// 
        /// - Base path is rooted path and validated path is not. Validated path
        /// may not be changed, because it doesn't contain root information and
        /// the starting point for relative path calculation can not be taken.
        /// Validated path is considered to be relative path already.
        /// 
        /// - Base path is not rooted path and validated path is rooted path.
        /// It's similar to previous case. The starting point for relative path
        /// calculations may not be taken.
        /// </summary>
        /// <param name="iBasePath">Based path used for validation.</param>
        /// <param name="iPath">Validated path.</param>
        /// <returns>True, if validated path can be made relative to the base
        /// folder. False otherwise.</returns>
        private static bool CanBeRelative(string iBasePath, string iPath)
        {
            return
                !(
                    (Path.IsPathRooted(iBasePath) && !Path.IsPathRooted(iPath)) ||
                    (!Path.IsPathRooted(iBasePath) && Path.IsPathRooted(iPath))
                );
        }

        /// <summary>
        /// Calculates path relative to the base folder. As addition it
        /// normalizes given paths, so the relative path can be calculated. It also
        /// checks, if relative path can be calculated. If not, it returns iPath.
        /// 
        /// The relative path may not be calculated, if:
        /// - root path of base and converted paths are different
        /// - base path is rooted while converted path is not
        /// - converted path is rooted while base path is not.
        /// </summary>
        /// <param name="iBasePath">Base path for the calculation.</param>
        /// <param name="iPath">Path that should be changed to relative.</param>
        /// <returns>Relative path, if possible. If not, iPath is returned.</returns>
        protected static string DoGetRelativePath(string iBasePath, string iPath)
        {
            if (iPath == "")
                return iBasePath;

            iBasePath = PathUtils.RemoveEndingDirectorySeparator(iBasePath);
            iPath = PathUtils.RemoveEndingDirectorySeparator(iPath);

            iBasePath = PathUtils.FixRootPath(iPath, iBasePath);
            iPath = PathUtils.FixRootPath(iBasePath, iPath);

            if (!PathUtils.HaveSameRoots(iBasePath, iPath) || !CanBeRelative(iBasePath, iPath))
                return iPath;

            return BuildRelativePath(iBasePath, iPath);
        }

        /// <summary>
        /// Changes path to be relative to the base folder. It checks, if relative 
        /// path can be calculated. If not, it returns iPath.
        /// 
        /// The relative path may not be calculated, if:
        /// - root path of base and converted paths are different
        /// - base path is rooted while converted path is not
        /// - converted path is rooted while base path is not.
        /// </summary>
        /// <param name="iBasePath">Base path for the calculation.</param>
        /// <param name="iPath">Path that should be changed to relative.</param>
        /// <returns>Relative path, if possible. If not, iPath is returned.
        /// 
        /// The path is always ended with directory separator character.</returns>
        public static string GetRelativePath(string iBasePath, string iPath)
        {
            return PathUtils.AddEndingDirectorySeparator(DoGetRelativePath(iBasePath, iPath));
        }

        /// <summary>
        /// Changes filename to be relative to the base filename. It checks, if 
        /// relative path can be calculated. If not, it returns iFilename.
        /// 
        /// The relative filename may not be calculated, if:
        /// - root path of base and converted filenames are different
        /// - base filename is rooted while converted filename is not
        /// - converted filename is rooted while base filename is not.
        /// </summary>
        /// <param name="iBaseFileName">Base filename for the calculation.</param>
        /// <param name="iFileName">Filename that should be changed to relative.</param>
        /// <returns>Relative filename, if possible. If not, iFileName is returned.</returns>
        public static string GetRelativeFileName(string iBaseFileName, string iFileName)
        {
            if (iFileName.Equals(""))
                return "";

            string vBasePath = Path.GetDirectoryName(iBaseFileName);
            return GetRelativeFilenameByFolder(vBasePath, iFileName);
        }

        /// <summary>
        /// Changes filename to be relative to the base path. It checks, if 
        /// relative path can be calculated. If not, it returns iPath.
        /// 
        /// The relative filename may not be calculated, if:
        /// - root path of base and converted paths are different
        /// - base path is rooted while converted filename is not
        /// - converted filename is rooted while base path is not.
        /// </summary>
        /// <param name="iBaseFileName">Base filename for the calculation.</param>
        /// <param name="iFileName">Filename that should be changed to relative.</param>
        /// <returns>Relative filename, if possible. If not, iFileName is returned.</returns>
        public static string GetRelativeFilenameByFolder(string iBasePath, string iFileName)
        {
            string vPath = Path.GetDirectoryName(iFileName);
            string vFileName = Path.GetFileName(iFileName);

            return Path.Combine(DoGetRelativePath(iBasePath, vPath), vFileName);
        }
    }
}
