namespace Shelve.IO
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;

    public class ParsedSet
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Declares")]
        public Dictionary<string, string[]> Declares { get; set; }

        [JsonProperty("Expressions")]
        public string[] Expressions { get; set; }
    }
}
