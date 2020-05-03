using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Collections.Generic;

namespace RemoveNuget.Tests
{
    [TestClass()]
    public class rmnupkgTests
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }
        string TestBaseDir;

        [TestInitialize]
        public void TestInit()
        {
            List<string> semVersions = new List<string> { "1.0.0", "1.0", "1.0.1-alpha", "1.0.1-beta.5", "1.0.1-build.23", "1.0.0-alpha+001" };
            TestBaseDir = Environment.GetEnvironmentVariable("DUMMY_NUGET_ROOT");
            if(string.IsNullOrEmpty(TestBaseDir) == true)
            {
                TestBaseDir = Path.Combine(TestContext.TestDir, ".nuget", "packages", "System.Data.SqlClient");
            }
            Directory.CreateDirectory(TestBaseDir);
            semVersions.ForEach(x =>
            {
                string verDir = Path.Combine(TestBaseDir, x);
                Directory.CreateDirectory(verDir);
            });

        }

        [TestMethod()]
        public void RemoveNupkgTest()
        {
            List<string> expectSemVersions = new List<string> { "1.0.0", "1.0.1-alpha", "1.0.1-beta.5", "1.0.1-build.23", "1.0.0-alpha+001" };
            var folders = rmnupkg.CollectNupkgFolder(TestBaseDir, 1);

            foreach (var item in folders)
            {
                expectSemVersions.Remove(item);
            }
            Assert.IsTrue(expectSemVersions.Count != 0, "SemVer is not compatible");
        }

    }
}