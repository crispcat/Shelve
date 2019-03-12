namespace Shelve.IO.Tests
{
    using System;
    using Shelve.IO;
    using NUnit.Framework;

    [TestFixture]
    public class IOTests
    {
        private static readonly string path = AppDomain.CurrentDomain.BaseDirectory + "InputSetsForTests";

        [Test] public void ParserExtractsPreprocessorMerges()
        {
            var preprocessor = new Preprocessor(path);
            var parsedData = JsonParser.ExtractData(preprocessor.MergeAllFiles());
        }
    }
}
