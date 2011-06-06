using System;
using System.Collections.Generic;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.Meta;

namespace PocoDb.Specs
{
    public class when_a_poco_is_built : with_a_new_BasicPocoBuilder
    {
        Establish c = () => {
            meta = fake.an<IPocoMeta>();
            A.CallTo(() => meta.Type).Returns(typeof (DummyObject));
            A.CallTo(() => meta.Properties).Returns(new Dictionary<IProperty, object>());
        };

        Because of = () => poco = sut.Build(meta);

        It should_not_be_null = () => poco.ShouldNotBeNull();
        It should_be_the_correct_type = () => (poco is DummyObject).ShouldBeTrue();

        static IPocoMeta meta;
        static object poco;
    }


    public class when_a_poco_with_a_value_property_is_built : with_a_new_BasicPocoBuilder
    {
        Establish c = () => {
            meta = fake.an<IPocoMeta>();
            A.CallTo(() => meta.Type).Returns(typeof (DummyObject));
            A.CallTo(() => meta.Properties).Returns(new Dictionary<IProperty, object>()
                                                    {{new Property<DummyObject, string>(p => p.FirstName), "value"}});
        };

        Because of = () => poco = sut.Build(meta) as DummyObject;

        It should_have_its_property_set = () => poco.FirstName.ShouldEqual("value");

        static IPocoMeta meta;
        static DummyObject poco;
    }
}