using System;
using Newtonsoft.Json;
using PocoDb.Meta;

namespace PocoDb.Serialisation
{
    public class PropertyConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer) {
            var serializedProperty = new SerializedProperty {
                                                                TypeName = value.GetType().FullName,
                                                                PropertyName = ((IProperty) value).PropertyName
                                                            };

            serializer.Serialize(writer, serializedProperty);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                        Newtonsoft.Json.JsonSerializer serializer) {
            try {
                var serializedProperty = serializer.Deserialize<SerializedProperty>(reader);

                var pocoType = Type.GetType(serializedProperty.TypeName).GetGenericArguments()[0];

                var propertyInfo = pocoType.GetProperty(serializedProperty.PropertyName);

                return Property.Create(propertyInfo);
            }
            catch (Exception ex) {
                throw new InvalidOperationException("Failed to deserialise IProperty", ex);
            }
        }

        public override bool CanConvert(Type objectType) {
            return objectType == typeof (IProperty) ||
                   (objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof (Property<,>));
        }

        public class SerializedProperty
        {
            public string TypeName { get; set; }
            public string PropertyName { get; set; }
        }
    }
}