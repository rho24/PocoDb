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


        Because of = () => indexMatch = sut.RetrieveIndex(expression);

        It should_return_an_exact_match = () => indexMatch.IsExact.ShouldBeTrue();
        It should_return_a_TypeIndex = () => indexMatch.Index.ShouldBeOfType<TypeIndex<DummyObject>>();

        static Expression expression;
        static IndexMatch indexMatch;
    }
}