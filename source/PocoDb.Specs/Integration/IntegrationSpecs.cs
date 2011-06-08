using System;
using System.Linq;
using Machine.Specifications;

namespace PocoDb.Specs.Integration
{
    public class when_an_object_is_added : with_a_new_PocoDbClient
    {
        Because of = () => {
            using (var session = sut.BeginWritableSession()) {
                session.Add(new DummyObject());
                session.SaveChanges();
            }
        };

        It should_contain_object = () => sut.BeginSession().Get<DummyObject>().ToArray().Count().ShouldEqual(1);
    }

    public class when_a_property_is_changed : with_a_populated_PocoDbClient
    {
        Because of = () => {
            using (var session = sut.BeginWritableSession()) {
                var poco = session.GetWritable<DummyObject>().First();
                poco.FirstName = "Changed";
                session.SaveChanges();
            }
        };

        It should_contain_contain_change =
            () => sut.BeginSession().Get<DummyObject>().FirstOrDefault().FirstName.ShouldEqual("Changed");

        static IWritablePocoSession session;
        static DummyObject poco;
    }
}