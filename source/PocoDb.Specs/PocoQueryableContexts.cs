using System;
using developwithpassion.specifications.fakeiteasy;
using Machine.Specifications;
using PocoDb.Linq;
using PocoDb.Server;

namespace PocoDb.Specs
{
    [Subject(typeof (PocoQueryable<>))]
    public class with_a_new_PocoQueryable<T> : Observes<PocoQueryable<T>>
    {
        Establish c = () => {
            server = fake.an<IPocoDbServer>();
            sut_factory.create_using(() => new PocoQueryable<T>(server));
        };

        protected static IPocoDbServer server;
    }
}