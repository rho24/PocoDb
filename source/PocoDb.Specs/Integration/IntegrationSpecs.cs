using System.Linq;
using Machine.Specifications;

namespace PocoDb.Specs.Integration
{
    public class when_an_object_is_added : with_a_new_PocoDbServer
    {
        Because of = () => {
            using (var session = sut.BeginWritableSession()) {
                session.Add(new DummyObject());
                session.SaveChanges();
            }
        };

        It should_contain_object = () => sut.BeginSession().Get<DummyObject>().FirstOrDefault().ShouldNotBeNull();
    }

    public class when_a_property_is_changed : with_a_populated_PocoDbServer
    {
        Because of = () => {
            using (var session = sut.BeginWritableSession()) {
                var poco = session.GetWritable<DummyObject>().First();
                poco.FirstName = "Changed";
                session.SaveChanges();
            }
        };

        It should_contain_contain_change = () => sut.BeginSession().Get<DummyObject>().First().FirstName.ShouldEqual("Changed");

        static IWritablePocoSession session;
        static DummyObject poco;
    }
}