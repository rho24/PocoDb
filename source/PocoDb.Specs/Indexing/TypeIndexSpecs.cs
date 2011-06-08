using System;
using System.Collections.Generic;
using Machine.Specifications;
using PocoDb.Indexing;
using PocoDb.Meta;

namespace PocoDb.Specs.Indexing
{
    public class when_GetIds_is_called : with_a_populated_TypeIndex
    {
        Because of = () => ids = sut.GetIds();

        It should_return_its_ids = () => ids.ShouldContainOnly(id);

        static IEnumerable<IPocoId> ids;
    }

    public class when_matching_against_a_plain_query : with_a_new_TypeIndex
    {
        Because of = () => result = sut.GetMatch(QueryExpressions.DummyObjectIEnumerable);

        It should_be_an_exact_match = () => result.IsExact.ShouldBeTrue();
        It should_reference_the_index = () => result.Index.ShouldEqual(sut);

        static IndexMatch result;
    }

    public class when_matching_against_a_where_query : with_a_new_TypeIndex
    {
        Because of = () => result = sut.GetMatch(QueryExpressions.DummyObjectWhere);

        It should_be_a_partial_match = () => result.IsPartial.ShouldBeTrue();
        It should_have_a_PartialDepth_of_one = () => result.PartialDepth.ShouldEqual(1);

        static IndexMatch result;
    }

    public class when_matching_against_a_different_type : with_a_new_TypeIndex
    {
        Because of = () => result = sut.GetMatch(QueryExpressions.ChildObjectIEnumerable);

        It should_be_a_no_match = () => (result.IsExact || result.IsPartial).ShouldBeFalse();

        static IndexMatch result;
    }
}