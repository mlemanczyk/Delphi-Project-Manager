using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DelphiProjectHandler.Dialogs
{
    public class PathUtils
    {
        /// <summary>
        /// Contatets folders from the list, starting on specific index. 
        /// The returned path always contains ending directory separator character.
        /// </summary>
        /// <param name="iStartingIdx">Index of the folder in the array. Concatanation
        /// will start on the given folder. Empty path will be returned, if index is 
        /// greater than the number of folders in the array.</param>
        /// <param name="iDirectories">Array of folders which should be concatanated.
        /// </param>
        /// <returns>Empty path if iStartingIdx is outside the bounds of the folders
        /// array. Concatanated folders path ended with directory separator
        /// character.</returns>
        public static string ConcatFolders(int iStartingIdx, string[] iDirectories)
        {
            string vPath = "";
            if (iStartingIdx < 0)
                return vPath;

            for (; iStartingIdx < iDirectories.Length; iStartingIdx++)
                vPath = Path.Combine(vPath, iDirectories[iStartingIdx]);

            return AddEndingDirectorySeparator(vPath);
        }

        /// <summary>
        /// Builds a list of "go-up" folders (..) which can be used in relative
        /// paths. The path is always ended with directory separator character.
        /// </summary>
        /// <param name="iCount">Number of "go-up" folders to return.</param>
        /// <returns>Path containing "go-up" folders.</returns>
        public static string BuildGoUpPath(int iCount)
        {
            StringBuilder vPath = new StringBuilder();
            for (int vPathIdx = 0; vPathIdx < iCount; vPathIdx++)
                vPath.Append(@"..\");

            return AddEndingDirectorySeparator(vPath.ToString());
        }

        /// <summary>
        /// Splits list of folders into array of folder names. 
        /// 
        /// This function doesn't recognize if the path ends with file name, or not. 
        /// If it does, the file name will be included on the list. It also doesn't 
        /// verify, if the path is valid.
        /// 
        /// If the path starts and/or ends with directory separator character, the 
        /// first and/or last array's element will be empty string.
        /// </summary>
        /// <param name="iPath">Path to split.</param>
        /// <returns>Array of folder names.</returns>
        public static string[] SplitPath(string iPath)
        {
            if (iPath == "")
                return new string[] { };

            return iPath.Split(Path.DirectorySeparatorChar);
        }

        /// <summary>
        /// Adds a root path to the given path.
        /// 
        /// Before adding the root path, the function verifies if iCorrectPath 
        /// contains a root path itself.
        /// 
        /// It also verifies, if iPathToFix parameter starts with directory separator.
        /// If not, iPathToFix is treated as relative path and no root path is added.
        /// </summary>
        /// <param name="iCorrectPath">Correct path, containing root path.</param>
        /// <param name="iPathToFix">Path, which doesn't contain complete root path
        /// and should be fixed.</param>
        /// <returns>Fixed path containing root path. 
        /// 
        /// If iPathToFix is relative path or iCorrectPath doesn't contain root path, not modified iPathToFix string
        /// is returned.</returns>
        public static string FixRootPath(string iCorrectPath, string iPathToFix)
        {
            if (
                    Path.IsPathRooted(iCorrectPath) && 
                    !iCorrectPath.StartsWith(Path.DirectorySeparatorChar.ToString()) && 
                    iPathToFix.StartsWith(Path.DirectorySeparatorChar.ToString())
                )
                return Combine(Path.GetPathRoot(iCorrectPath), iPathToFix);
            else
                return iPathToFix;
        }

        /// <summary>
        /// Add one path to another. The paths are combined, even if the right path
        /// starts with directory separator.
        /// 
        /// If the appended path starts with drive and path to which you append is not 
        /// empty - exception will be raised. If it's empty, the appended path will
        /// be returned.
        /// 
        /// If the path to which you append is empty, the appended path will be
        /// returned, too.
        /// </summary>
        /// <exception cref="ArgumentException"
        /// <param name="iLeftPath">Left path to add to.</param>
        /// <param name="iRightPath">Right path which will be added</param>
        /// <returns>Appended path, if the path to which you append is empty.
        /// 
        /// Appended path if it's contains root path and the path to which you
        /// append is empty.
        /// 
        /// If appended path doesn't contain root path, concatated path will be
        /// returned.
        /// </returns>
        public static string Combine(string iLeftPath, string iRightPath)
        {
            iRightPath = iRightPath.TrimStart(Path.DirectorySeparatorChar);
            if (Path.IsPathRooted(iRightPath) && iLeftPath != "")
                throw new ArgumentException("Path containing root path can not be appended to another path");
            else
                if (Path.IsPathRooted(iRightPath))
                    return iRightPath;

            iLeftPath = AddEndingDirectorySeparator(iLeftPath);
            return iLeftPath + iRightPath;
        }

        /// <summary>
        /// Removes ending directory separator character from the given path. It
        /// doesn't remove the ending path, if the is root path - "c:\" or "\"
        /// </summary>
        /// <param name="iPath">Path from which ending directory separator 
        /// character should be removed.</param>
        /// <returns>Path without ending directory separator character.</returns>
        public static string RemoveEndingDirectorySeparator(string iPath)
        {
            if ((iPath.Length != 3 && iPath != @"\") || (iPath.Length == 3 && iPath[1] != ':'))
                iPath = iPath.TrimEnd(Path.DirectorySeparatorChar);
            return iPath;
        }

        /// <summary>
        /// Adds ending directory separator character, if iPath doesn't contain it
        /// already. Empty strings are ignored and returned without modification.
        /// </summary>
        /// <param name="iPath">Path to which ending separator character must be 
        /// added.</param>
        /// <returns>Path containing ending directory separator character.</returns>
        public static string AddEndingDirectorySeparator(string iPath)
        {
            if (iPath != "" && !iPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                iPath += Path.DirectorySeparatorChar;

            return iPath;
        }
        
        /// <summary>
        /// Adds ending directory separator character to all paths on the given list, if the
        /// given path doesn't contain it already. Empty strings are ignored and returned without
        /// modification.
        /// </summary>
        /// <param name="iPaths">List of path to which directory separator must be added.</param>
        /// <returns>New list containing paths with directory separator.</returns>
        public static IList<string> AddEndingDirectorySeparator(IList<string> iPaths)
        {
        	IList<string> vResult = new List<string>(iPaths.Count);
        	foreach (string vPath in iPaths)
        		vResult.Add(AddEndingDirectorySeparator(vPath));
        	return vResult;
        }

        /// <summary>
        /// Checks, if two path have same root paths.
        /// 
        /// If one of the path doesn't contain root path, they are  considered the 
        /// same, because one of them is considered to be relative path.
        /// 
        /// If both don't contain root paths, they are considered the same, because
        /// both are relative and are assumed to be in the same root folder.
        /// </summary>
        /// <param name="iLeftPath">First path to compare.</param>
        /// <param name="iRightPath">Second path to compare.</param>
        /// <returns>True if root paths and they are the same. False otherwise.</returns>
        public static bool HaveSameRoots(string iLeftPath, string iRightPath)
        {
            return !((Path.IsPathRooted(iLeftPath)) &&
                   (Path.IsPathRooted(iRightPath)) &&
                   (Path.GetPathRoot(iLeftPath).ToLower() != Path.GetPathRoot(iRightPath).ToLower()));
        }

        /// <summary>
        /// Compares two arrays of folder names and returns the index of the first 
        /// folder, which is different. iLeftCompare is treated as primary.
        /// 
        /// If arrays contain different number of elements, the last index in
        /// iLeftCompare or iRightCompare will be returned, depending which one is
        /// lower.
        /// 
        /// This function is case insensitive.
        /// </summary>
        /// <param name="iLeftCompare">Main array for the comparision.</param>
        /// <param name="iRightCompare">Compared array of folders.</param>
        /// <returns>Index of first different folder in given arrays.</returns>
        public static int GetFirstDifferentFolderIdx(string[] iLeftCompare, string[] iRightCompare)
        {
            int vFolderIdx = 0;
            while (
                vFolderIdx < iLeftCompare.Length &&
                vFolderIdx < iRightCompare.Length &&
                iLeftCompare[vFolderIdx].ToLower() == iRightCompare[vFolderIdx].ToLower()
            )
                vFolderIdx++;

            return vFolderIdx;
        }

        /// <summary>
        /// Validates, if the given path is a valid path.
        /// </summary>
        /// <param name="iPath">Path to validate.</param>
        /// <returns>true, if path is valid. false, if path is invalid.</returns>
        public static bool IsValidPath(string iPath)
        {
        	return Directory.Exists(iPath);
        }
        
        /// <summary>
        /// Removes all invalid paths from the given list of paths.
        /// </summary>
        /// <param name="iPaths">List of paths to validate.</param>
        /// <returns>New list, containing only valid paths.</returns>
        public static IList<string> RemoveInvalidPaths(IList<string> iPaths)
        {
        	List<string> vResult = new List<string>();
        	foreach (string vPath in iPaths)
        		if (IsValidPath(vPath))
        			vResult.Add(vPath);
        	
        	return vResult;
        }
        
        /// <summary>
        /// Normalizes given path list, by:
        /// - making all paths lower case
        /// - removing invalid and empty paths
        /// - removing duplicates
        /// - adding directory separator to each path
        /// </summary>
        /// <param name="iPaths"></param>
        /// <returns></returns>
        public static IList<string> Normalize(IList<string> iPaths)
        {
        	IList<string> vResult = RemoveInvalidPaths(iPaths);
        	vResult = AddEndingDirectorySeparator(vResult);
        	vResult = StringListUtils.Normalize(vResult);
        	return vResult;
        }
    }
}
