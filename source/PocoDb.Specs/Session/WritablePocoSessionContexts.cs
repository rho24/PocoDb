using System;
using Machine.Specifications;
using PocoDb.Server;
using PocoDb.Session;
using developwithpassion.specifications.fakeiteasy;

namespace PocoDb.Specs.Session
{
    [Subject(typeof (IWritablePocoSession))]
    public class with_a_new_WritablePocoSession : Observes<WritablePocoSession>
    {
        Establish c = () => { server = depends.on<IPocoDbServer>(); };

        protected static IPocoDbServer server;
    }
}