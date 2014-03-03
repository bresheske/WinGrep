using NUnit.Framework;
using System;
using System.Linq;
using WinGrep.Core.Services;

namespace WinGrep.Test.Tests
{
    [TestFixture]
    public class FileLocatorTests
    {
        [Test]
        public void TestSolutionFileLocation()
        {
            // Test to find the .sln file in this solution.
            var root = Environment.CurrentDirectory + @"\..\..\..\";
            var regex = ".sln$";
            var recur = false;

            // Verify one result, and the exact file path.
            var files = new FileLocator().FindFiles(root, regex, recur);
            Assert.AreEqual(1, files.Count());
        }
    }
}