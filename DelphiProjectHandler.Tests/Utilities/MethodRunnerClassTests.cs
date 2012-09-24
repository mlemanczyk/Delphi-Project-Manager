using System;
using NUnit.Framework;

namespace DelphiProjectHandler.Tests.Utilities
{
    [TestFixture]
    public class MethodRunnerClassTests
    {
        protected class MethodRunnerTester
        {
            protected static int Calculate(int iLeft, int iRight, int iMultiplier)
            {
                return (iLeft + iRight) * iMultiplier;
            }

            public MethodRunnerTester(int iOffset)
            {
                Offset = iOffset;
            }

            public int Offset;

            public void VoidFunction(int iLeft, int iRight, ref int ivTotal)
            {
                ivTotal = Calculate(iLeft, iRight, 2) + Offset;
            }

            public int Function(int iLeft, int iRight)
            {
                return Calculate(iLeft, iRight, 3) + Offset;
            }

            public static void StaticVoidFunction(int iLeft, int iRight, ref int ivTotal)
            {
                ivTotal = Calculate(iLeft, iRight, 4);
            }

            public static int StaticFunction(int iLeft, int iRight)
            {
                return Calculate(iLeft, iRight, 5);
            }
        }

        [Test]
        public void RunInstanceFunction()
        {
            MethodRunnerTester vTester = new MethodRunnerTester(1);
            int vActual = (int)MethodRunner.RunInstance(vTester, "Function", new object[] { 2, 3 });
            Assert.AreEqual(16, vActual, "Wrong result for instance function");
        }

        [Test]
        public void RunInstanceVoidFunction()
        {
            MethodRunnerTester vTester = new MethodRunnerTester(2);
            int vActual = 0;
            object[] vParams = new object[] { 2, 3, vActual };
            MethodRunner.RunInstance(vTester, "VoidFunction", vParams);
            Assert.AreEqual(12, vParams[2], "Wrong result for void instance function");
        }

        [Test]
        public void RunStaticFunction()
        {
            int vActual = (int)MethodRunner.RunStatic(typeof(MethodRunnerTester), "StaticFunction", new object[] { 2, 3 });
            Assert.AreEqual(25, vActual, "Wrong result for instance function");
        }

        [Test]
        public void RunStaticVoidFunction()
        {
            int vActual = 0;
            object[] vParams = new object[] { 2, 3, vActual };
            MethodRunner.RunStatic(typeof(MethodRunnerTester), "StaticVoidFunction", vParams);
            Assert.AreEqual(20, vParams[2], "Wrong result for instance function");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void RunNonExistingMethod()
        {
            MethodRunnerTester vTester = new MethodRunnerTester(1);
            MethodRunner.RunInstance(vTester, "NonExistingMethod", null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void RunNonExistingStaticMethod()
        {
            MethodRunner.RunStatic(typeof(MethodRunnerTester), "NonExistingMethod", null);
        }
    }
}
