using System;
using Newtonsoft.Json;
using PocoDb.Meta;

namespace PocoDb.Serialisation
{
    public class PropertyConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer) {}

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                        Newtonsoft.Json.JsonSerializer serializer) {
            try {
                do reader.Read(); while (reader.TokenType != JsonToken.String);

                var propertyType = Type.GetType(reader.Value.ToString()).GetGenericArguments()[0];

                do reader.Read(); while (reader.TokenType != JsonToken.String);

                var propertyName = reader.Value.ToString();

                var propertyInfo = propertyType.GetProperty(propertyName);

                do reader.Read(); while (reader.TokenType != JsonToken.EndObject);

                do reader.Read(); while (reader.TokenType != JsonToken.EndObject);

                return Property.Create(propertyInfo);
            }
            catch (Exception ex) {
                throw new InvalidOperationException("Failed to deserialise IProperty", ex);
            }
        }

        public override bool CanConvert(Type objectType) {
            return objectType == typeof (IProperty);
        }
    }
}