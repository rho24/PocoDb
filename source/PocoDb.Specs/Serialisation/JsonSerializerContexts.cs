using System;
using Machine.Specifications;
using PocoDb.Serialisation;
using developwithpassion.specifications.fakeiteasy;

namespace PocoDb.Specs.Serialisation
{
    [Subject(typeof (JsonSerializer))]
    public class with_a_new_JsonSerializer : Observes<JsonSerializer>
    {
        Establish c = () => { };
    }
}