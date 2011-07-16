using System;
using Machine.Specifications;
using PocoDb.Linq;
using developwithpassion.specifications.fakeiteasy;

namespace PocoDb.Specs.Linq
{
    [Subject(typeof (ExpressionProcessor))]
    public class with_a_new_ExpressionProcessor : Observes<ExpressionProcessor>
    {
        Establish c = () => { };
    }
}