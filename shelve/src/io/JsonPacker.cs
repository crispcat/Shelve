namespace Shelve.IO
{
    using System.IO;
    using Shelve.Runtime;
    using Newtonsoft.Json;

    internal static class JsonPacker
    {
        public static T ExtractDataAs<T>(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (JsonException jex)
            {
                throw new System.Exception($"Input data was arrived in wrong format." +
                    $"\nTrace: {jex.StackTrace}" +
                    $"\nMessage: {jex.Message}");
            }
        }

        public static void StoreData(object data, string fileName)
        {
            var path = Configuration.AssemblyDirectory + fileName;
            var json = JsonConvert.SerializeObject(data);

            File.WriteAllText(path, json);
        }
    }
}
