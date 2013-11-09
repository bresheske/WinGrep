
using NUnit.Framework;
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
            var root = @"C:\Projects\WinGrep";
            var regex = ".cs$";
            var contents = "TestFileContents";
            
            // Verify three locations found.
            var results = new FileLocator().FindInFiles(root, regex, contents, true);
            Assert.AreEqual(3, results.Count());

            // Verity no locations found without recursive sub directories.
            results = new FileLocator().FindInFiles(root, regex, contents, false);
            Assert.AreEqual(0, results.Count());
        }

    }
}
