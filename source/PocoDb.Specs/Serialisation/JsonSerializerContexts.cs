using System;
using developwithpassion.specifications.fakeiteasy;
using Machine.Specifications;
using PocoDb.Serialisation;

namespace PocoDb.Specs.Serialisation
{
    [Subject(typeof (JsonSerializer))]
    public class with_a_new_JsonSerializer : Observes<JsonSerializer>
    {
        Establish c = () => { };
    }
}