using System;
using System.Collections.Generic;
using Machine.Specifications;
using PocoDb.Commits;
using PocoDb.Meta;

namespace PocoDb.Specs.Serialisation
{
    public class when_a_property_is_serialized : with_a_new_JsonSerializer
    {
        Establish c = () => { property = new Property<DummyObject, string>(d => d.FirstName); };

        Because of = () => json = sut.Serialize(property);

        It should_equal_the_correct_value =
            () =>
            json.ShouldEqual(
                "{\"$type\":\"PocoDb.Serialisation.PropertyConverter+SerializedProperty, PocoDb\",\"TypeName\":\"PocoDb.Meta.Property`2[[PocoDb.Specs.DummyObject, PocoDb.Specs, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null],[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]\",\"PropertyName\":\"FirstName\"}");

        static Property<DummyObject, string> property;
        static string json;
    }

    public class when_a_property_is_deserialized : with_a_new_JsonSerializer
    {
        Establish c = () => {
            json =
                "{\"$type\":\"PocoDb.Serialisation.PropertyConverter+SerializedProperty, PocoDb\",\"TypeName\":\"PocoDb.Meta.Property`2[[PocoDb.Specs.DummyObject, PocoDb.Specs, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null],[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]\",\"PropertyName\":\"FirstName\"}";
        };

        Because of = () => property = sut.Deserialize<IProperty>(json);

        It should_not_be_null = () => property.ShouldNotBeNull();
        It should_be_the_correct_type = () => property.ShouldBeOfType<Property<DummyObject, string>>();

        It should_be_the_correct_property =
            () =>
            ((Property<DummyObject, string>) property).Info.ShouldEqual(
                new Property<DummyObject, string>(d => d.FirstName).Info);

        static IProperty property;
        static string json;
    }


    public class when_a_CommitId_is_serialized : with_a_new_JsonSerializer
    {
        Establish c =
            () => {
                id = new CommitId(Guid.Parse("12345600-0000-0000-0000-000000123456"),
                                  new DateTime(2011, 06, 29, 19, 35, 23, 455));
            };

        Because of = () => json = sut.Serialize(id);

        It should_be_the_correct_value = () => json.ShouldEqual(
            "\"634449729234550000,12345600-0000-0000-0000-000000123456\"");

        static ICommitId id;
        static string json;
    }

    public class when_a_CommitId_is_deserialized : with_a_new_JsonSerializer
    {
        Establish c = () => {
            json =
                "\"634449729234550000,12345600-0000-0000-0000-000000123456\"";
        };

        Because of = () => id = sut.Deserialize<ICommitId>(json);

        It should_not_be_null = () => id.ShouldNotBeNull();
        It should_be_the_correct_type = () => id.ShouldBeOfType<CommitId>();

        It should_be_have_the_correct_id =
            () => ((CommitId) id).Id.ShouldEqual(Guid.Parse("12345600-0000-0000-0000-000000123456"));

        It should_be_have_the_correct_created_time =
            () => ((CommitId) id).Created.ShouldEqual(new DateTime(2011, 06, 29, 19, 35, 23, 455));

        static ICommitId id;
        static string json;
    }

    public class when_a_generic_dictionary_is_serialized : with_a_new_JsonSerializer
    {
        Establish c = () => {
            dictionary = new Dictionary<int, string>();
            dictionary.Add(1, "one");
            dictionary.Add(2, "two");
        };

        Because of = () => json = sut.Serialize(dictionary);

        It should_serialize_into_list_of_key_value_pairs =
            () => json.ShouldEqual("[{\"Key\":1,\"Value\":\"one\"},{\"Key\":2,\"Value\":\"two\"}]");

        static Dictionary<int, string> dictionary;
        static string json;
    }


    public class when_a_generic_dictionary_is_deserialized : with_a_new_JsonSerializer
    {
        Establish c = () => { json = "[{\"Key\":1,\"Value\":\"one\"},{\"Key\":2,\"Value\":\"two\"}]"; };

        Because of = () => dictionary = sut.Deserialize<IDictionary<int, object>>(json);

        It should_not_return_null = () => dictionary.ShouldNotBeNull();
        It should_contain_the_first_value = () => dictionary[1].ShouldEqual("one");
        It should_contain_the_second_value = () => dictionary[2].ShouldEqual("two");

        static string json;
        static IDictionary<int, object> dictionary;
    }


    public class when_a_PocoMeta_is_serialized : with_a_new_JsonSerializer
    {
        Establish c = () => {
            meta = new PocoMeta(new PocoId(Guid.Empty), typeof (DummyObject));
            meta.Properties.Add(new Property<DummyObject, string>(d => d.FirstName), "value1");

            meta.Collection.Add("value2");
        };

        Because of = () => json = sut.Serialize(meta);

        It should_be_the_correct_value =
            () =>
            json.ShouldEqual(
                "{\"$type\":\"PocoDb.Meta.PocoMeta, PocoDb\",\"Id\":{\"$type\":\"PocoDb.Meta.PocoId, PocoDb\",\"Id\":\"00000000-0000-0000-0000-000000000000\"},\"Properties\":[{\"Key\":{\"$type\":\"PocoDb.Serialisation.PropertyConverter+SerializedProperty, PocoDb\",\"TypeName\":\"PocoDb.Meta.Property`2[[PocoDb.Specs.DummyObject, PocoDb.Specs, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null],[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]\",\"PropertyName\":\"FirstName\"},\"Value\":\"value1\"}],\"Collection\":[\"value2\"],\"Type\":\"PocoDb.Specs.DummyObject, PocoDb.Specs, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\"}");

        static PocoMeta meta;
        static string json;
    }


    public class when_a_PocoMeta_is_deserialized : with_a_new_JsonSerializer
    {
        Establish c =
            () => {
                json =
                    "{\"$type\":\"PocoDb.Meta.PocoMeta, PocoDb\",\"Id\":{\"$type\":\"PocoDb.Meta.PocoId, PocoDb\",\"Id\":\"12300000-0000-0000-0000-000000000000\"},\"Properties\":[{\"Key\":{\"$type\":\"PocoDb.Serialisation.PropertyConverter+SerializedProperty, PocoDb\",\"TypeName\":\"PocoDb.Meta.Property`2[[PocoDb.Specs.DummyObject, PocoDb.Specs, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null],[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]\",\"PropertyName\":\"FirstName\"},\"Value\":\"value1\"}],\"Collection\":[\"value2\"],\"Type\":\"PocoDb.Specs.DummyObject, PocoDb.Specs, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\"}";
            };

        Because of = () => meta = sut.Deserialize<IPocoMeta>(json);

        It should_not_return_null = () => meta.ShouldNotBeNull();
        It should_not_have_a_null_id = () => meta.Id.ShouldNotBeNull();
        It should_have_correct_type = () => meta.Type.ShouldEqual(typeof (DummyObject));
        It should_have_a_value_in_properties = () => meta.Properties.Count.ShouldEqual(1);

        It should_have_the_correct_property =
            () => meta.Properties.ContainsKey(new Property<DummyObject, string>(d => d.FirstName)).ShouldBeTrue();

        It should_have_the_correct_property_value =
            () => meta.Properties[new Property<DummyObject, string>(d => d.FirstName)].ShouldEqual("value1");

        It should_have_a_value_in_collection = () => meta.Collection.ShouldContainOnly("value2");


        static string json;
        static IPocoMeta meta;
    }
}