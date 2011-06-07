using System;
using developwithpassion.specifications.fakeiteasy;
using Machine.Specifications;

namespace PocoDb.Specs
{
    [Subject(typeof (PocoDbClient))]
    public class with_a_new_PocoDbClient : Observes<PocoDbClient>
    {
        Establish c = () => { };
    }
}