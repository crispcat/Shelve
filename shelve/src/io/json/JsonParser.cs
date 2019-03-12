namespace Shelve.IO
{
    using Newtonsoft.Json;

    public static class JsonParser
    {
        public static ParsedSet[] ExtractData(string jSetArray)
        {
            try
            {
                return JsonConvert.DeserializeObject<ParsedSet[]>(jSetArray);
            }
            catch (JsonException jex)
            {
                throw new System.Exception($"Input data was arrived in wrong format." +
                    $"\nTrace: {jex.StackTrace}" +
                    $"\nMessage: {jex.Message}");
            }
        }
    }
}
