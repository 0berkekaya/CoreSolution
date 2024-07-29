using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace CoreLibrary
{
    public static class Tools
    {
        private static JsonSerializerOptions _jsonSerializerOptions = new()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            WriteIndented = true
        };
        public static string? ObjectToJsonString(object paramObject) => TryCatch.Run(() => JsonSerializer.Serialize(paramObject, _jsonSerializerOptions)).Result;
        public static T? JsonStringToObject<T>(string jsonString) => TryCatch.Run(() => JsonSerializer.Deserialize<T>(jsonString, _jsonSerializerOptions)).Result;

        public static void SerializeToJsonFile<T>(T obj, string filePath) => TryCatch.Run(() =>
        {
            string jsonString = JsonSerializer.Serialize(obj, _jsonSerializerOptions);
            File.WriteAllText(filePath, jsonString);
        });

        public static T? DeserializeFromJsonFile<T>(string filePath) => TryCatch.Run(() =>
        {
            string jsonString = File.ReadAllText(filePath);
            return JsonStringToObject<T>(jsonString);
        }).Result;

        public static Dictionary<string, object>? ConvertToDictionary(object obj) => TryCatch.Run(() =>
        {
            string? jsonString = ObjectToJsonString(obj);
            if (jsonString != null) return JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString, _jsonSerializerOptions) ?? [];
            else return null;
        }).Result;

        public static Guid StringToGuid(string paramString) => TryCatch.Run(() => Guid.Parse(paramString)).Result;

    }
}
