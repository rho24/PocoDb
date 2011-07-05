using System;
using Machine.Specifications;
using PocoDb.Session;
using developwithpassion.specifications.fakeiteasy;

namespace PocoDb.Specs.Session
{
    [Subject(typeof (IPocoSession))]
    public class with_a_new_PocoSession : Observes<PocoSession>
    {
        Establish c = () => { };
    }
}