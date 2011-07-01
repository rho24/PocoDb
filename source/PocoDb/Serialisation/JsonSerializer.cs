using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PocoDb.Serialisation
{
    public class JsonSerializer : ISerializer
    {
        public JsonSerializerSettings JsonSettings { get; private set; }

        public JsonSerializer() {
            JsonSettings = new JsonSerializerSettings() {TypeNameHandling = TypeNameHandling.Objects};
            JsonSettings.Converters.Add(new PropertyConverter());
            JsonSettings.Converters.Add(new IsoDateTimeConverter());
            JsonSettings.Converters.Add(new CommitIdConverter());
        }

        public string Serialize(object obj) {
            return JsonConvert.SerializeObject(obj, Formatting.None, JsonSettings);
        }

        public object Deserialize(string str) {
            return JsonConvert.DeserializeObject(str, JsonSettings);
        }

        public T Deserialize<T>(string str) {
            return JsonConvert.DeserializeObject<T>(str, JsonSettings);
        }
    }
}