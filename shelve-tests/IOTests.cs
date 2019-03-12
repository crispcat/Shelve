namespace Shelve.IO.Tests
{
    using System;
    using Shelve.IO;
    using NUnit.Framework;

    [TestFixture]
    public class IOTests
    {
        private static readonly string path = AppDomain.CurrentDomain.BaseDirectory + "InputSetsForTests";

        [Test] public void PreprocessorMerges()
        {
            var preprocessor = new Preprocessor(path);

            string merged = preprocessor.MergeAllFiles();

            Assert.IsTrue(merged.Contains("distance += xPosition + V * T"));
            Assert.IsTrue(merged.Contains("\"xPosition\" : \"0\""));
            Assert.IsTrue(merged.Contains("\"simpleIterator\" : [ \"it = [1, + 1]\" ]"));
            Assert.IsTrue(merged.Contains("f += f1 + f2"));
        }

        [Test] public void ParserExtracts()
        {
            var preprocessor = new Preprocessor(path);
            var parsedData = JsonParser.ExtractData(preprocessor.MergeAllFiles());
        }
    }
}
