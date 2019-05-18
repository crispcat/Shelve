namespace Shelve.Runtime
{
    using System;
    using Shelve.IO;
    using Shelve.Core;
    using System.Collections.Generic;

    internal static class DataManager
    {
        private static Dictionary<string, SetTranslator> translators;

        static DataManager()
        {
            translators = new Dictionary<string, SetTranslator>();
            ProcessInput();
        }

        private static void ProcessInput()
        {
            var inputPath = Configuration.GetValueFor("InputRootPath");
            var mergedSets = new Preprocessor(inputPath).MergeAllFiles();
            var parsedSets = JsonPacker.ExtractDataAs<ParsedSet[]>(mergedSets);

            foreach (var parsedSet in parsedSets)
            {
                translators.Add(parsedSet.Name, new SetTranslator(parsedSet));
            }
        }

        internal static VariableSet GetDataBySetName(string name)
        {
            if (!translators.ContainsKey(name))
            {
                throw new InvalidOperationException($"Variable set {name} has not processed during " +
                    $"the config translation. Make sure that passed name match set name in config file and " +
                    $"input root path in shelve_config.json is correct");
            }

            return translators[name].Translate();
        }
    }
}
