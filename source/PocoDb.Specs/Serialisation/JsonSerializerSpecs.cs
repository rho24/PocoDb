using System;
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
}