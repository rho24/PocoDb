using System;
using System.Linq.Expressions;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.Indexing;
using PocoDb.Linq;
using PocoDb.Meta;

namespace PocoDb.Specs.Indexing
{
    public class when_asked_to_retrieve_an_index_for_a_type : with_a_new_IndexManager_containing_a_TypeIndex
    {
        Establish c =
            () => { expression = Expression.Constant(new PocoQueryable<DummyObject>(fake.an<PocoQueryProvider>())); };


        Because of = () => indexMatch = sut.RetrieveIndex(expression);

        It should_return_an_exact_match = () => indexMatch.IsExact.ShouldBeTrue();
        It should_return_a_TypeIndex = () => indexMatch.Index.ShouldBeOfType<TypeIndex<DummyObject>>();

        static Expression expression;
        static IndexMatch indexMatch;
    }


    public class when_a_notified_of_a_new_poco : with_a_new_IndexManager_containing_a_TypeIndex
    {
        Establish c = () => {
            meta = fake.an<IPocoMeta>();
            id = fake.an<IPocoId>();
            A.CallTo(() => meta.Id).Returns(id);
            A.CallTo(() => meta.Type).Returns(typeof (DummyObject));
        };

        Because of = () => sut.NotifyMetaChange(meta);

        It should_notify_each_index = () => typeIndex.GetIds().ShouldContain(id);

        static IPocoMeta meta;
        static IPocoId id;
    }

    public class when_a_notified_of_a_new_poco_when_no_TypeIndex_exists : with_a_new_IndexManager
    {
        Establish c = () => {
            meta = fake.an<IPocoMeta>();
            id = fake.an<IPocoId>();
            A.CallTo(() => meta.Id).Returns(id);
            A.CallTo(() => meta.Type).Returns(typeof (DummyObject));
        };

        Because of = () => sut.NotifyMetaChange(meta);

        It should_create_a_TypeIndex = () => sut.TypeIndexes.ContainsKey(typeof (DummyObject)).ShouldBeTrue();
        It should_notify_the_index = () => sut.TypeIndexes[typeof (DummyObject)].GetIds().ShouldContain(id);

        static IPocoMeta meta;
        static IPocoId id;
    }
}