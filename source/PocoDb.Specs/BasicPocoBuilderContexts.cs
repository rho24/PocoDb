using System;
using developwithpassion.specifications.fakeiteasy;
using Machine.Specifications;
using PocoDb.Pocos;

namespace PocoDb.Specs
{
    [Subject(typeof (BasicPocoBuilder))]
    public class with_a_new_BasicPocoBuilder : Observes<BasicPocoBuilder>
    {
        Establish c = () => { };
    }
}