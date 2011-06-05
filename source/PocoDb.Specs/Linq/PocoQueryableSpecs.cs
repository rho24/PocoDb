using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.Linq;
using PocoDb.Queries;

namespace PocoDb.Specs.Linq
{
    public class when_calling_GetEnumerator : with_a_new_PocoQueryable<DummyObject>
    {
        Because of = () => (sut as IEnumerable<DummyObject>).GetEnumerator();

        It should_call_execute_on_the_executor =
            () => A.CallTo(() => executor.Execute<IEnumerable<DummyObject>>(A<Expression>.Ignored)).MustHaveHappened();
    }

    public class when_calling_non_generic_GetEnumerator : with_a_new_PocoQueryable<DummyObject>
    {
        Because of = () => (sut as IEnumerable).GetEnumerator();

        It should_call_execute_on_the_executor =
            () => A.CallTo(() => executor.Execute<IEnumerable<DummyObject>>(A<Expression>.Ignored)).MustHaveHappened();
    }

    public class when_calling_FirstOrDefault : with_a_new_PocoQueryable<DummyObject>
    {
        Because of = () => sut.FirstOrDefault();

        It should_call_execute_on_the_executor =
            () => A.CallTo(() => executor.Execute<DummyObject>(A<Expression>.Ignored)).MustHaveHappened();
    }
}