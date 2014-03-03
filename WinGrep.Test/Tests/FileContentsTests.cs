
using NUnit.Framework;
using System;
using System.Linq;
using WinGrep.Core.Services;

namespace WinGrep.Test.Tests
{
    [TestFixture]
    public class FileContentsTests
    {

        [Test]
        public void TestFileContents()
        {
            // Test the location of the string 'TestFileContents'
            var root = Environment.CurrentDirectory + @"\..\..\..\";
            var regex = ".cs$";
            var contents = "TestFileContents";
            
            // Verify three locations found.
            var results = new FileLocator().FindInFiles(root, regex, contents, true);
            Assert.AreEqual(3, results.Count());

            // Verity no locations found without recursive sub directories.
            results = new FileLocator().FindInFiles(root, regex, contents, false);
            Assert.AreEqual(0, results.Count());
        }

        [Test]
        public void TestMatching()
        {
            var root = Environment.CurrentDirectory + @"\..\..\..\";
            var regex = @"\.txt";
            var contents = @"(\d{5})\s([a-z|A-Z]+)";

            var results = new FileLocator().FindInFiles(root, regex, contents, true);
            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(3, results.ElementAt(0).CaptureGroups.Count());
        }

    }
}
