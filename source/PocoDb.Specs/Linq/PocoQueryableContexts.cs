using System;
using Machine.Specifications;
using PocoDb.Linq;
using developwithpassion.specifications.fakeiteasy;

namespace PocoDb.Specs.Linq
{
    [Subject(typeof (PocoQueryable<>))]
    public class with_a_new_PocoQueryable<T> : Observes<PocoQueryable<T>>
    {
        Establish c = () => {
            executor = fake.an<IPocoQueryableExecutor>();

            sut_factory.create_using(() => new PocoQueryable<T>(new PocoQueryProvider(executor)));
        };

        protected static IPocoQueryableExecutor executor;
    }
}