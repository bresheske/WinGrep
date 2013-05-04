using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinGrep.Core.Services;

namespace WinGrep.Test
{
    [TestFixture]
    public class FileLocatorTests
    {
        [Test]
        public void TestFileLocator()
        {
            var root = @"C:\Projects\WinGrep";
            var regex = ".sln$";
            var recur = false;

            var files = new FileLocator().FindFiles(root, regex, recur);
            Assert.AreEqual(1, files.Count());
            Assert.AreEqual(@"C:\Projects\WinGrep\WinGrep.sln", files.ElementAt(0));

            files = new FileLocator().FindFiles(root, "^W", false);
            Assert.AreEqual(2, files.Count());

            files = new FileLocator().FindFiles(root, ".exe$", true);
            Assert.AreEqual(4, files.Count());
        }

        [Test]
        public void TestFileContents()
        {
            var root = @"C:\Projects\WinGrep";
            var regex = ".cs$";
            var contents = "TestFileContents";
            var recur = true;

            var results = new FileLocator().FindInFiles(root, regex, contents, recur);
            Assert.AreEqual(2, results.Count());
        }
    }
}