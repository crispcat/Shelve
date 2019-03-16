namespace Shelve.Runtime
{
    using System;
    using System.IO;
    using Shelve.IO;
    using System.Reflection;

    internal static class Configuration
    {
        public static string AssemblyDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);

                return Path.GetDirectoryName(path);
            }
        }

        private const string fileName = "shelve_config.json";

        private static ParsedConfiguration Properties;

        static Configuration()
        {
            var configPath = AssemblyDirectory + fileName;

            if (File.Exists(configPath))
            {
                var content = File.ReadAllText(configPath);
                Properties = JsonPacker.ExtractDataAs<ParsedConfiguration>(content);
            }
            else
            {
                Properties = new ParsedConfiguration
                {
                    InputRootPath = AssemblyDirectory
                };
            }
        }

        public static void Serialize() => 
            JsonPacker.StoreData(Properties, AssemblyDirectory + fileName);

        public static string GetValueFor(string propertyName) =>
            typeof(ParsedConfiguration).GetProperty(propertyName).GetValue(Properties, null).ToString();
    }
}
