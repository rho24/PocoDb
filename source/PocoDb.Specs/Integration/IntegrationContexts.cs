using System;
using developwithpassion.specifications.fakeiteasy;
using Machine.Specifications;

namespace PocoDb.Specs.Integration
{
    [Subject(typeof (IPocoDbServer))]
    public abstract class with_a_new_PocoDbServer : Observes<IPocoDbServer>
    {}

    public abstract class with_a_populated_PocoDbServer : with_a_new_PocoDbServer
    {
        Establish c = () => sut_setup.run(sut => {
            using (var session = sut.BeginWritableSession()) {
                session.Add(new DummyObject());
                session.SaveChanges();
            }
        });
    }
}