using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.Meta;

namespace PocoDb.Specs.Meta
{
    public class when_building_a_meta : with_a_new_PocoMetaBuilder
    {
        Establish c = () => {
            poco = new object();
            id = fake.an<IPocoId>();

            A.CallTo(() => pocoIdBuilder.New()).Returns(id);
            A.CallTo(() => session.TrackedIds).Returns(new Dictionary<object, IPocoId>());
        };

        Because of = () => metas = sut.Build(poco);

        It should_return_a_meta = () => metas.Count().ShouldEqual(1);
        It should_track_the_new_id = () => session.TrackedIds[poco].ShouldEqual(id);

        static object poco;
        static IPocoId id;
        static IEnumerable<IPocoMeta> metas;
    }
}