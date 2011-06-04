using System;
using developwithpassion.specifications.fakeiteasy;
using Machine.Specifications;

namespace PocoDb
{
}

namespace PocoDb.Specs
{
    [Subject(typeof (IWritablePocoSession))]
    public class with_a_new_WritablePocoSession : Observes<WritablePocoSession>
    {
        Establish c = () => { server = depends.on<IInternalServer>(); };

        protected static IInternalServer server;
    }
}