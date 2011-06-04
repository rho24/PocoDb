using System;
using developwithpassion.specifications.fakeiteasy;
using Machine.Specifications;
using PocoDb.Server;

namespace PocoDb.Specs
{
    [Subject(typeof (IPocoDbServer))]
    public class with_a_new_PocoDbServer : Observes<PocoDbServer>
    {
        Establish c = () => { };
    }
}