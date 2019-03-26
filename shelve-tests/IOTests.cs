namespace Shelve.Tests
{
    using System;
    using Shelve.IO;
    using NUnit.Framework;

    [TestFixture]
    public class IOTests
    {
        private static readonly string path = AppDomain.CurrentDomain.BaseDirectory + "InputSetsForTests";

        [Test] public void PackerExtractsPreprocessorMerges()
        {
            var preprocessor = new Preprocessor(path);
            var parsedData = JsonPacker.ExtractDataAs<ParsedSet[]>(preprocessor.MergeAllFiles());
        }
    }
}
