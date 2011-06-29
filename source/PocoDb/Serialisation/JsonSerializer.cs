using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PocoDb.Commits;
using PocoDb.Extensions;

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

    public class CommitIdConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer) {
            var id = value as CommitId;
            if (id == null) throw new InvalidOperationException("value is not a CommitId");

            serializer.Serialize(writer, "{0},{1}".Fmt(id.Created.Ticks.ToString(), id.Id.ToString()));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                        Newtonsoft.Json.JsonSerializer serializer) {
            var serializedId = serializer.Deserialize<string>(reader);

            if (serializedId == null)
                return null;

            var values = serializedId.Split(',');

            return new CommitId(Guid.Parse(values[1]), new DateTime(long.Parse(values[0])));
        }

        public override bool CanConvert(Type objectType) {
            return typeof (ICommitId).IsAssignableFrom(objectType);
        }

        public class SerializedCommitId
        {
            public string Id { get; set; }
        }
    }
}