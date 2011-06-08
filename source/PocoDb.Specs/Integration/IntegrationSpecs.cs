using System;
using System.Linq;
using Machine.Specifications;

namespace PocoDb.Specs.Integration
{
    public class when_querying_an_IEnumerable
    {}

    public class when_querying_with_first
    {}

    public class when_querying_count
    {}

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

    public class when_a_property_is_changed : with_a_populated_poco_PocoDbClient
    {
        Because of = () => {
            using (var session = sut.BeginWritableSession()) {
                var poco = session.GetWritable<DummyObject>().First();
                poco.FirstName = "Changed";
                session.SaveChanges();
            }
        };

        It should_have_saved_change =
            () => sut.BeginSession().Get<DummyObject>().FirstOrDefault().FirstName.ShouldEqual("Changed");
    }

    public class when_a_poco_poco_property_is_set : with_a_populated_poco_PocoDbClient
    {
        Because of = () => {
            using (var session = sut.BeginWritableSession()) {
                var poco = session.GetWritable<DummyObject>().First();
                poco.Child = new ChildObject();
                poco.Child.Counter = 5;
                session.SaveChanges();
            }
        };

        It should_have_saved_child =
            () => sut.BeginSession().Get<DummyObject>().FirstOrDefault().Child.ShouldNotBeNull();

        It should_have_saved_childs_values =
            () => sut.BeginSession().Get<DummyObject>().FirstOrDefault().Child.Counter.ShouldEqual(5);

        It should_have_make_the_child_queryable =
            () => sut.BeginSession().Get<ChildObject>().FirstOrDefault().ShouldNotBeNull();
    }

    public class when_a_child_property_is_changed : with_a_populated_poco_and_child_PocoDbClient
    {
        Because of = () => {
            using (var session = sut.BeginWritableSession()) {
                var poco = session.GetWritable<DummyObject>().First();
                poco.Child.Counter = 5;
                session.SaveChanges();
            }
        };

        It should_have_saved_change =
            () => sut.BeginSession().Get<ChildObject>().FirstOrDefault().Counter.ShouldEqual(5);
    }
}