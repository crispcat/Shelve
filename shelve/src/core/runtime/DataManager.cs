namespace Shelve.Runtime
{
    using System;
    using System.IO;
    using Shelve.IO;
    using Shelve.Core;
    using System.Reflection;
    using System.Collections.Generic;

    internal static class DataManager
    {
        private static Dictionary<string, VariableSet> translatedSets;

        static DataManager()
        {
            translatedSets = new Dictionary<string, VariableSet>();
            ProcessInput();
        }

        private static void ProcessInput()
        {
            var inputPath = Configuration.GetValueFor("InputRootPath");
            var mergedSets = new Preprocessor(inputPath).MergeAllFiles();
            var parsedSets = JsonPacker.ExtractDataAs<ParsedSet[]>(mergedSets);

            foreach (var parsedSet in parsedSets)
            {
                var setCompiler = new SetCompiler(parsedSet);

                setCompiler.Compile();

                translatedSets.Add(setCompiler.TranslatedSet.Name, setCompiler.TranslatedSet);
            }
        }

        internal static VariableSet GetDataBySetName(string name)
        {
            if (!translatedSets.ContainsKey(name))
            {
                throw new InvalidOperationException($"Variable set {name} has not processed during " +
                    $"the config translation. Make sure that passed name match set name in config file and " +
                    $"input root path in shelve_config.json is correct");
            }

            return translatedSets[name];
        }
    }
}
