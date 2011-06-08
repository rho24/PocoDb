using System;
using System.Linq;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.Linq;
using PocoDb.Meta;
using PocoDb.Pocos;

namespace PocoDb.Specs.Session
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
            pocoFactory = depends.on<IPocoFactory>();

            id = fake.an<IPocoId>();
            meta = fake.an<IPocoMeta>();

            sut_setup.run(sut => sut.IdsMetasAndProxies.Metas.Add(id, meta));
        };

        Because of = () => sut.GetPoco(id);

        It should_call_BuildPoco =
            () => A.CallTo(() => pocoFactory.Build(A<IPocoMeta>.Ignored, sut.IdsMetasAndProxies)).MustHaveHappened();

        It should_call_Use_the_relevant_meta =
            () => A.CallTo(() => pocoFactory.Build(meta, sut.IdsMetasAndProxies)).MustHaveHappened();

        static IPocoFactory pocoFactory;
        static IPocoId id;
        static IPocoMeta meta;
    }
}