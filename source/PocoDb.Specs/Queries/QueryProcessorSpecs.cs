using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.Extensions;
using PocoDb.Indexing;
using PocoDb.Meta;
using PocoDb.Queries;

namespace PocoDb.Specs.Queries
{
    public class when_queried_for_IEnumerable : with_a_new_QueryProcessor
    {
        Establish c = () => {
            query = fake.an<IQuery>();
            expression = QueryExpressions.DummyObjectIEnumerable;

            A.CallTo(() => query.Expression).Returns(expression);

            index = fake.an<IIndex>();
            ids = fake.an<IEnumerable<IPocoId>>();
            metas = fake.an<IEnumerable<IPocoMeta>>();

            A.CallTo(() => indexManager.RetrieveIndex(expression)).Returns(IndexMatch.ExactMatch(index));
            A.CallTo(() => index.GetIds()).Returns(ids);
            A.CallTo(() => metaStore.Get(ids)).Returns(metas);
        };

        Because of = () => result = sut.ProcessEnumerable(query);

        It should_retrieve_the_index = () => A.CallTo(() => indexManager.RetrieveIndex(expression)).MustHaveHappened();
        It should_get_ids_from_index = () => A.CallTo(() => index.GetIds()).MustHaveHappened();
        It should_retrieve_metas_from_MetaStore = () => A.CallTo(() => metaStore.Get(ids)).MustHaveHappened();
        It should_return_the_metas = () => result.Metas.ShouldEqual(metas);
        It should_return_an_EnumerablePocoQueryResult = () => result.ShouldBeOfType<EnumerableQueryResult>();
        It should_return_the_ids = () => ((EnumerableQueryResult) result).ElementIds.ShouldEqual(ids);

        static IQuery query;
        static Expression expression;
        static IIndex index;
        static IEnumerable<IPocoId> ids;
        static IEnumerable<IPocoMeta> metas;
        static IQueryResult result;
    }

    public class when_queried_for_Single : with_a_new_QueryProcessor
    {
        Establish c = () => {
            query = fake.an<IQuery>();
            expression = QueryExpressions.DummyObjectSingle;

            A.CallTo(() => query.Expression).Returns(expression);

            index = fake.an<IIndex>();
            id = fake.an<IPocoId>();
            ids = new[] {id};
            meta = fake.an<IPocoMeta>();
            metas = new[] {meta};

            A.CallTo(() => indexManager.RetrieveIndex(expression.GetInnerQuery())).Returns(IndexMatch.ExactMatch(index));
            A.CallTo(() => index.GetIds()).Returns(ids);
            A.CallTo(() => metaStore.Get(id)).Returns(meta);
        };

        Because of = () => result = sut.ProcessSingle(query);

        It should_get_ids_from_index = () => A.CallTo(() => index.GetIds()).MustHaveHappened();
        It should_retrieve_metas_from_MetaStore = () => A.CallTo(() => metaStore.Get(id)).MustHaveHappened();
        It should_return_the_metas = () => result.Metas.ShouldEqual(metas);
        It should_return_an_SingleQueryResult = () => result.ShouldBeOfType<SingleQueryResult>();
        It should_return_the_id = () => ((SingleQueryResult) result).ElementId.ShouldEqual(id);

        static IQuery query;
        static Expression expression;
        static IIndex index;
        static IPocoId id;
        static IEnumerable<IPocoId> ids;
        static IEnumerable<IPocoMeta> metas;
        static IPocoMeta meta;
        static IQueryResult result;
    }

    public class when_queried_for_First : with_a_new_QueryProcessor
    {
        Establish c = () => {
            query = fake.an<IQuery>();
            expression = QueryExpressions.DummyObjectFirst;

            A.CallTo(() => query.Expression).Returns(expression);

            index = fake.an<IIndex>();
            id = fake.an<IPocoId>();
            ids = new[] {id};
            meta = fake.an<IPocoMeta>();
            metas = new[] {meta};

            A.CallTo(() => indexManager.RetrieveIndex(expression.GetInnerQuery())).Returns(IndexMatch.ExactMatch(index));
            A.CallTo(() => index.GetIds()).Returns(ids);
            A.CallTo(() => metaStore.Get(id)).Returns(meta);
        };

        Because of = () => result = sut.ProcessElement(query);

        It should_get_ids_from_index = () => A.CallTo(() => index.GetIds()).MustHaveHappened();
        It should_retrieve_metas_from_MetaStore = () => A.CallTo(() => metaStore.Get(id)).MustHaveHappened();
        It should_return_the_metas = () => result.Metas.ShouldEqual(metas);
        It should_return_an_ElementQueryResult = () => result.ShouldBeOfType<ElementQueryResult>();
        It should_return_the_id = () => ((ElementQueryResult) result).ElementId.ShouldEqual(id);

        static IQuery query;
        static Expression expression;
        static IIndex index;
        static IPocoId id;
        static IEnumerable<IPocoId> ids;
        static IEnumerable<IPocoMeta> metas;
        static IPocoMeta meta;
        static IQueryResult result;
    }

    public class when_queried_for_FirstOrDefault
    {}
}