namespace Shelve.IO
{
    using Newtonsoft.Json;

    internal class ParsedConfiguration
    {
        [JsonProperty("InputRootPath")]
        public string InputRootPath { get; set; }
    }
}
