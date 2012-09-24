using System;
using System.Reflection;

namespace DelphiProjectHandler.Tests.Utilities
{
    /// <summary>
    /// Runs static and non-static private methods.
    /// </summary>
    public class MethodRunner
    {
        /// <summary>
        /// Runs a private method of an object instance.
        /// </summary>
        /// <param name="iObject">Existing object instance.</param>
        /// <param name="iMethodName">Method to run.</param>
        /// <param name="iParams">Parameters for the method.</param>
        /// <returns>Result of running the method.</returns>
        public static object RunInstance(object iObject, string iMethodName, object[] iParams)
        {
            BindingFlags vFlags = BindingFlags.Instance;
            return RunMethod(iObject.GetType(), iObject, vFlags, iMethodName, iParams);
        }

        /// <summary>
        /// Runs a private static method of a class.
        /// </summary>
        /// <param name="iType">Class type.</param>
        /// <param name="iMethodName">Method to run.</param>
        /// <param name="iParams">Parameters for the method.</param>
        /// <returns>Result of running the method.</returns>
        public static object RunStatic(Type iType, string iMethodName, object[] iParams)
        {
            BindingFlags vFlags = BindingFlags.Static;
            return RunMethod(iType, null, vFlags, iMethodName, iParams);
        }

        /// <summary>
        /// Runs a private static or non-static method of a class or existing object instance.
        /// </summary>
        /// <param name="iType">Class type.</param>
        /// <param name="iObject">Existing object instance.</param>
        /// <param name="iBindingFlags">Flags used when searching for the method.
        /// By default BindingFlags.NonPublic and BindingFlags.Public will be
        /// added.</param>
        /// <param name="iMethodName">Method to run.</param>
        /// <param name="iParams">Parameters for the method.</param>
        /// <returns>Result of running the method.</returns>
        protected static object RunMethod(Type iType, object iObject, BindingFlags iBindingFlags, string iMethodName, params object[] iParams)
        {
            iBindingFlags = iBindingFlags | BindingFlags.NonPublic | BindingFlags.Public;
            MethodInfo vMethodInfo = iType.GetMethod(iMethodName, iBindingFlags);
            if (vMethodInfo == null)
                throw new ArgumentException("Private method " + iMethodName + " was not found in type " + iType.ToString());

            return vMethodInfo.Invoke(iObject, iParams);
        }
    }
}
