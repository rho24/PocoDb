using System;
using System.Linq.Expressions;
using Machine.Specifications;
using PocoDb.Indexing;
using PocoDb.Linq;

namespace PocoDb.Specs.Indexing
{
    public class when_asked_to_retrieve_an_index_for_a_type : with_a_new_IndexManager
    {
        Establish c =
            () => { expression = Expression.Constant(new PocoQueryable<DummyObject>(fake.an<PocoQueryProvider>())); };


        Because of = () => index = sut.RetrieveIndex(expression);

        It should_return_a_TypeIndex = () => index.ShouldBeOfType<TypeIndex>();

        static Expression expression;
        static IIndex index;
    }
}