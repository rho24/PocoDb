using System;
using Machine.Specifications;
using developwithpassion.specifications.fakeiteasy;

namespace PocoDb.Specs
{
    [Subject(typeof (PocoDbClient))]
    public class with_a_new_PocoDbClient : Observes<PocoDbClient>
    {
        Establish c = () => { };
    }
}