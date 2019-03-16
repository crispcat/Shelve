namespace Shelve.IO
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    internal class ParsedSet
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Declares")]
        public Dictionary<string, string[]> Declares { get; set; }

        [JsonProperty("Expressions")]
        public string[] Expressions { get; set; }
    }
}
