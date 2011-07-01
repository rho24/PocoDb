using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PocoDb.Extensions;
using PocoDb.Meta;

namespace PocoDb.Serialisation
{
    public class JsonSerializer : ISerializer
    {
        public JsonSerializerSettings JsonSettings { get; private set; }

        public JsonSerializer() {
            JsonSettings = new JsonSerializerSettings() {TypeNameHandling = TypeNameHandling.Objects};
            JsonSettings.Converters.Add(new GenericDictionaryConverter());
            JsonSettings.Converters.Add(new PropertyConverter());
            JsonSettings.Converters.Add(new CommitIdConverter());
            JsonSettings.Converters.Add(new PocoMetaConverter());
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

    public class PocoMetaConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer) {
            throw new InvalidOperationException("Should only be used for reading");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                        Newtonsoft.Json.JsonSerializer serializer) {
            return serializer.Deserialize<PocoMeta>(reader);
        }

        public override bool CanConvert(Type objectType) {
            return objectType == typeof (IPocoMeta);
        }
    }

    public class GenericDictionaryConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer) {
            GenericHelper.InvokeGeneric(() => WriteJson<object, object>(writer, value, serializer),
                                        value.GetType().GetGenericArguments());
        }


        void WriteJson<K, V>(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer) {
            var dictionary = value as IDictionary<K, V>;
            if (dictionary == null)
                throw new InvalidOperationException("value is not correct dictionary type");

            serializer.Serialize(writer, dictionary.Select(kvp => kvp));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                        Newtonsoft.Json.JsonSerializer serializer) {
            return
                GenericHelper.InvokeGeneric(
                    () => ReadJson<object, object>(reader, objectType, existingValue, serializer),
                    objectType.GetGenericArguments());
        }

        object ReadJson<K, V>(JsonReader reader, Type objectType, object existingValue,
                              Newtonsoft.Json.JsonSerializer serializer) {
            var serializedDictionary = serializer.Deserialize<IEnumerable<KeyValuePair<K, V>>>(reader);

            return serializedDictionary.ToDictionary(keyValuePair => keyValuePair.Key,
                                                     keyValuePair => keyValuePair.Value);
        }

        public override bool CanConvert(Type objectType) {
            if (objectType.IsGenericType &&
                (objectType.GetGenericTypeDefinition() == typeof (IDictionary<,>)
                 || objectType.GetGenericTypeDefinition() == typeof (Dictionary<,>))) {
                return objectType.GetGenericArguments()[0] != typeof (string);
            }

            return false;
        }
    }
}