using System;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.Meta;

namespace PocoDb.Specs.Persistence
{
    public class when_a_new_meta_is_saved : with_a_new_InMemoryMetaStore
    {
        Establish c = () => {
            meta = fake.an<IPocoMeta>();
            id = fake.an<IPocoId>();

            A.CallTo(() => meta.Id).Returns(id);
        };

        Because of = () => sut.AddNew(meta);

        It should_be_retrievable = () => sut.Get(id).ShouldEqual(meta);

        static IPocoMeta meta;
        static IPocoId id;
    }

    public class when_retrieving_a_non_existence_meta : with_a_new_InMemoryMetaStore
    {
        Because of = () => meta = sut.Get(fake.an<IPocoId>());

        It should_return_null = () => meta.ShouldBeNull();

        static IPocoMeta meta;
    }

    public class when_a_meta_is_updated : with_populated_InMemoryMetaStore
    {
        Establish c = () => {
            property = new Property<DummyObject, string>(d => d.FirstName);

            sut_setup.run(sut => {
                meta = sut.GetWritable(id);
                meta.Properties.Add(property, null);
            });
        };

        Because of = () => sut.Update(meta);

        It should_be_updated = () => sut.Get(id).Properties.ContainsKey(property).ShouldBeTrue();

        static IPocoMeta meta;
        static Property<DummyObject, string> property;
    }


    public class when_a_meta_is_retrieved_whilst_already_being_updated
    {
        It should_throw;
    }
}