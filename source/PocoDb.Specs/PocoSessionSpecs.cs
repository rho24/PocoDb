using System;
using System.Linq;
using Machine.Specifications;
using PocoDb.Linq;

namespace PocoDb.Specs
{
    public class when_getting_by_type : with_a_new_PocoSession
    {
        Establish c = () => { };

        Because of = () => query = sut.Get<DummyObject>();

        It should_return_a_PocoQueryable = () => query.ShouldBeOfType<PocoQueryable<DummyObject>>();

        static IQueryable<DummyObject> query;
    }
}