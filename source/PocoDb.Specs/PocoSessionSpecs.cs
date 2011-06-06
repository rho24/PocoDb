using System;
using System.Linq;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.Linq;
using PocoDb.Meta;
using PocoDb.Pocos;

namespace PocoDb.Specs
{
    public class when_querying_by_type : with_a_new_PocoSession
    {
        Establish c = () => { };

        Because of = () => query = sut.Get<DummyObject>();

        It should_return_a_PocoQueryable = () => query.ShouldBeOfType<PocoQueryable<DummyObject>>();

        static IQueryable<DummyObject> query;
    }

    public class when_getting_an_object : with_a_new_PocoSession
    {
        Establish c = () => {
            pocoBuilder = depends.on<IPocoBuilder>();

            id = fake.an<IPocoId>();
            meta = fake.an<IPocoMeta>();

            sut_setup.run(sut => sut.Metas.Add(id, meta));
        };

        Because of = () => sut.GetPoco(id);

        It should_call_BuildPoco = () => A.CallTo(() => pocoBuilder.Build(A<IPocoMeta>.Ignored)).MustHaveHappened();
        It should_call_Use_the_relevant_meta = () => A.CallTo(() => pocoBuilder.Build(meta)).MustHaveHappened();

        static IPocoBuilder pocoBuilder;
        static IPocoId id;
        static IPocoMeta meta;
    }
}