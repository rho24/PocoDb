using System;
using developwithpassion.specifications.fakeiteasy;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.Linq;
using PocoDb.Server;

namespace PocoDb.Specs.Linq
{
    [Subject(typeof (PocoQueryable<>))]
    public class with_a_new_PocoQueryable<T> : Observes<PocoQueryable<T>>
    {
        Establish c = () => {
            session = fake.an<IInternalPocoSession>();
            server = fake.an<IPocoDbServer>();
            A.CallTo(() => session.Server).Returns(server);
            sut_factory.create_using(
                () => new PocoQueryable<T>(new PocoQueryProvider(new PocoQueryableExecutor(session))));
        };

        protected static IInternalPocoSession session;
        protected static IPocoDbServer server;
    }
}