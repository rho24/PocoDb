using System;
using Machine.Specifications;
using PocoDb.Server;
using developwithpassion.specifications.fakeiteasy;

namespace PocoDb.Specs.Server
{
    [Subject(typeof (IPocoDbServer))]
    public class with_a_new_PocoDbServer : Observes<PocoDbServer>
    {
        Establish c = () => { };
    }
}