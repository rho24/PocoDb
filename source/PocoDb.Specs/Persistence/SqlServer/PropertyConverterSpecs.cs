using System;
using Machine.Specifications;
using Newtonsoft.Json;
using PocoDb.Meta;

namespace PocoDb.Specs.Persistence.SqlServer
{
    public class when_a_property_is_serialised : with_a_new_PropertyConverter
    {
        Establish c = () => { property = new Property<DummyObject, string>(d => d.FirstName); };

        Because of = () => json = JsonConvert.SerializeObject(property, Formatting.None, settings);

        It should_equal_the_correct_value =
            () =>
            json.ShouldEqual(
                "{\"$type\":\"PocoDb.Meta.Property`2[[PocoDb.Specs.DummyObject, PocoDb.Specs],[System.String, mscorlib]], PocoDb\",\"Info\":{\"Name\":\"FirstName\",\"AssemblyName\":\"PocoDb.Specs, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"ClassName\":\"PocoDb.Specs.DummyObject\",\"Signature\":\"System.String FirstName\",\"MemberType\":16,\"GenericArguments\":null}}");

        static Property<DummyObject, string> property;
        static string json;
    }

    public class when_a_property_is_deserialised : with_a_new_PropertyConverter
    {
        Establish c = () => {
            json =
                "{\"$type\":\"PocoDb.Meta.Property`2[[PocoDb.Specs.DummyObject, PocoDb.Specs],[System.String, mscorlib]], PocoDb\",\"Info\":{\"Name\":\"FirstName\",\"AssemblyName\":\"PocoDb.Specs, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"ClassName\":\"PocoDb.Specs.DummyObject\",\"Signature\":\"System.String FirstName\",\"MemberType\":16,\"GenericArguments\":null}}";
        };

        Because of = () => property = JsonConvert.DeserializeObject<IProperty>(json, settings);

        It should_not_be_null = () => property.ShouldNotBeNull();
        It should_be_the_correct_type = () => property.ShouldBeOfType<Property<DummyObject, string>>();

        It should_be_the_correct_property =
            () =>
            (property as Property<DummyObject, string>).Info.ShouldEqual(
                new Property<DummyObject, string>(d => d.FirstName).Info);

        static IProperty property;
        static string json;
    }
}