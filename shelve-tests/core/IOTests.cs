namespace Shelve.Core.Tests
{
    using System.IO;
    using Shelve.IO;
    using NUnit.Framework;

    [TestFixture]
    public class IOTests
    {
        string GetDirectory => Directory.GetCurrentDirectory();

        [Test]
        public void Combine()
        {
            var preprocessor = new Preprocessor(GetDirectory);

            string combinedFiles = preprocessor.Combine();

            Assert.IsTrue(combinedFiles.Contains("distance += V * T"));
            Assert.IsTrue(combinedFiles.Contains("xPosition = 0"));
            Assert.IsTrue(combinedFiles.Contains("simpleIterator = [1, + 1]"));
            Assert.IsTrue(combinedFiles.Contains("f1 = [0, + f1]"));
        }
    }
}
