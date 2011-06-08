using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.Indexing;
using PocoDb.Meta;
using PocoDb.Queries;

namespace PocoDb.Specs.Queries
{
    public class when_a_queried_for_all_of_type : with_a_new_QueryProcessor
    {
        Establish c = () => {
            query = fake.an<IPocoQuery>();
            expression = fake.an<Expression>();

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
}