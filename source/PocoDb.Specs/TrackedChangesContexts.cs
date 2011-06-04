using developwithpassion.specifications.fakeiteasy;
using Machine.Specifications;
using PocoDb.ChangeTracking;
using FakeItEasy;
using PocoDb.Meta;

namespace PocoDb.Specs
{
    [Subject(typeof (TrackedChanges))]
    public abstract class with_a_new_TrackedChanges : Observes<ITrackedChanges>
    {
        Establish c = () => sut_factory.create_using(() => new TrackedChanges());
    }

    [Subject(typeof (TrackedChanges))]
    public abstract class with_an_object_added : with_a_new_TrackedChanges
    {
        Establish c = () => {
            obj = new object();

            sut_setup.run(sut => sut.TrackAddedObject(obj));
        };

        protected static object obj;
    }

    [Subject(typeof (TrackedChanges))]
    public abstract class with_a_property_set : with_a_new_TrackedChanges
    {
        Establish c = () => {
            obj = new object();
            prop = A.Fake<IProperty>();
            val = new object();

            sut_setup.run(sut => sut.TrackPropertySet(obj, prop, val));
        };

        protected static object obj;
        protected static IProperty prop;
        protected static object val;
    }
}