using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.Indexing;
using PocoDb.Linq;
using PocoDb.Meta;
using PocoDb.Queries;

namespace PocoDb.Specs.Queries
{
    public class when_a_queried_for_IEnumerable : with_a_new_QueryProcessor
    {
        Establish c = () => {
            query = fake.an<IPocoQuery>();
            expression = Expression.Constant(new PocoQueryable<DummyObject>(null));

            A.CallTo(() => query.Expression).Returns(expression);

            index = fake.an<IIndex>();
            ids = fake.an<IEnumerable<IPocoId>>();
            metas = fake.an<IEnumerable<IPocoMeta>>();

            A.CallTo(() => indexManager.RetrieveIndex(expression)).Returns(IndexMatch.ExactMatch(index));
            A.CallTo(() => index.GetIds()).Returns(ids);
            A.CallTo(() => metaStore.Get(ids)).Returns(metas);
        };

        Because of = () => result = sut.Process(query);

        It should_retrieve_the_index = () => A.CallTo(() => indexManager.RetrieveIndex(expression)).MustHaveHappened();
        It should_get_ids_from_index = () => A.CallTo(() => index.GetIds()).MustHaveHappened();
        It should_retrieve_metas_from_MetaStore = () => A.CallTo(() => metaStore.Get(ids)).MustHaveHappened();
        It should_return_the_ids = () => result.Ids.ShouldEqual(ids);
        It should_return_the_metas = () => result.Metas.ShouldEqual(metas);

        static IPocoQuery query;
        static Expression expression;
        static IIndex index;
        static IEnumerable<IPocoId> ids;
        static IEnumerable<IPocoMeta> metas;
        static IPocoQueryResult result;
    }

    public class when_queried_for_First : with_a_new_QueryProcessor
    {
        Establish c = () => {
            query = fake.an<IPocoQuery>();
            queryExpression = Expression.Constant(new PocoQueryable<DummyObject>(null));

            expression =
                Expression.Call(
                    typeof (Queryable).GetMethods().Where(m => m.Name.StartsWith("First")).First().MakeGenericMethod(
                        typeof (DummyObject)), queryExpression);

            A.CallTo(() => query.Expression).Returns(expression);

            index = fake.an<IIndex>();
            id = fake.an<IPocoId>();
            ids = new[] {id};
            meta = fake.an<IPocoMeta>();
            metas = new[] {meta};

            A.CallTo(() => indexManager.RetrieveIndex(queryExpression)).Returns(IndexMatch.ExactMatch(index));
            A.CallTo(() => index.GetIds()).Returns(ids);
            A.CallTo(() => metaStore.Get(id)).Returns(meta);
        };

        Because of = () => result = sut.Process(query);

        It should_retrieve_the_index =
            () => A.CallTo(() => indexManager.RetrieveIndex(queryExpression)).MustHaveHappened();

        It should_get_ids_from_index = () => A.CallTo(() => index.GetIds()).MustHaveHappened();
        It should_retrieve_metas_from_MetaStore = () => A.CallTo(() => metaStore.Get(id)).MustHaveHappened();
        It should_return_the_ids = () => result.Ids.ShouldContainOnly(id);
        It should_return_the_meta = () => result.Metas.ShouldContainOnly(meta);

        static IPocoQuery query;
        static ConstantExpression queryExpression;
        static Expression expression;
        static IIndex index;
        static IPocoId id;
        static IEnumerable<IPocoId> ids;
        static IEnumerable<IPocoMeta> metas;
        static IPocoMeta meta;
        static IPocoQueryResult result;
    }
}