using System;
using developwithpassion.specifications.fakeiteasy;
using Machine.Specifications;
using PocoDb.Session;

namespace PocoDb.Specs
{
    [Subject(typeof (IPocoSession))]
    public class with_a_new_PocoSession : Observes<PocoSession>
    {
        Establish c = () => { };
    }
}