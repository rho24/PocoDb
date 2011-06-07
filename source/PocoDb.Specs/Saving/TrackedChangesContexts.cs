using System;
using developwithpassion.specifications.fakeiteasy;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.ChangeTracking;
using PocoDb.Meta;

namespace PocoDb.Specs.Saving
{
    [Subject(typeof (ChangeTracker))]
    public abstract class with_a_new_TrackedChanges : Observes<ChangeTracker>
    {
        Establish c = () => { };
    }

    [Subject(typeof (ChangeTracker))]
    public abstract class with_an_object_added : with_a_new_TrackedChanges
    {
        Establish c = () => {
            poco = new object();

            sut_setup.run(sut => sut.TrackAddedObject(poco));
        };

        protected static object poco;
    }

    [Subject(typeof (ChangeTracker))]
    public abstract class with_a_property_set : with_a_new_TrackedChanges
    {
        Establish c = () => {
            poco = new object();
            property = A.Fake<IProperty>();
            value = new object();

            sut_setup.run(sut => sut.TrackPropertySet(poco, property, value));
        };

        protected static object poco;
        protected static IProperty property;
        protected static object value;
    }
}