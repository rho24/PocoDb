using System;
using developwithpassion.specifications.fakeiteasy;
using Machine.Specifications;

namespace PocoDb.Specs.Integration
{
    [Subject(typeof (PocoDbClient))]
    public abstract class with_a_new_PocoDbClient : Observes<PocoDbClient>
    {}

    public abstract class with_a_populated_poco_PocoDbClient : with_a_new_PocoDbClient
    {
        Establish c = () => sut_setup.run(sut => {
            using (var session = sut.BeginWritableSession()) {
                session.Add(new DummyObject());
                session.SaveChanges();
            }
        });
    }

    public abstract class with_a_populated_poco_and_child_PocoDbClient : with_a_new_PocoDbClient
    {
        Establish c = () => sut_setup.run(sut => {
            using (var session = sut.BeginWritableSession()) {
                session.Add(new DummyObject() {Child = new ChildObject()});
                session.SaveChanges();
            }
        });
    }
}