using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.Queries;

namespace PocoDb.Specs
{
    public class when_calling_GetEnumerator : with_a_new_PocoQueryable<DummyObject>
    {
        Establish c = () => { };

        Because of = () => (sut as IEnumerable<DummyObject>).GetEnumerator();

        It should_call_Query_on_the_server = () => A.CallTo(() => server.Query(A<PocoQuery>.Ignored)).MustHaveHappened();
    }

    public class when_calling_non_generic_GetEnumerator : with_a_new_PocoQueryable<DummyObject>
    {
        Establish c =
            () =>
            A.CallTo(() => server.Query(A<PocoQuery>.Ignored)).Invokes(i => pocoQuery = i.Arguments[0] as PocoQuery);

        Because of = () => (sut as IEnumerable).GetEnumerator();

        It should_call_Query_on_the_server = () => A.CallTo(() => server.Query(A<PocoQuery>.Ignored)).MustHaveHappened();
        It should_include_the_expression_in_the_query;

        static PocoQuery pocoQuery;
        static Expression expectedExpression;
    }
}