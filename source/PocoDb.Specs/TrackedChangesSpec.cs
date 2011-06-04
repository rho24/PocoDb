using System.Collections.Generic;
using System.Linq;
using developwithpassion.specifications.fakeiteasy;
using Machine.Specifications;
using FakeItEasy;
using PocoDb.Specs;
using PocoDb;

namespace PocoDb.Specs
{
    [Subject(typeof (TrackedChanges))]
    public abstract class with_a_new_TrackedChanges : Observes<ITrackedChanges>
    {
        Establish c = () => sut_factory.create_using(() => new TrackedChanges());
    }

    public class when_an_object_is_added : with_a_new_TrackedChanges
    {
        Establish c = () => { obj = new object(); };

        Because of = () => sut.TrackAddedObject(obj);

        It should_contain_an_AddObjectChange = () => sut.AddObjectChanges.Count().ShouldEqual(1);
        It should_reference_the_added_object = () => sut.AddObjectChanges.First().Object.ShouldEqual(obj);

        static object obj;
    }

    public class when_a_property_is_set : with_a_new_TrackedChanges
    {
        Establish c = () => {
            obj = new object();
            prop = new Property();
            val = new object();
        };

        Because of = () => sut.TrackPropertySet(obj, prop, val);

        It should_contain_a_PropertySetChange = () => sut.PropertySetChanges.Count().ShouldEqual(1);
        It should_reference_the_parent_object = () => sut.PropertySetChanges.First().Object.ShouldEqual(obj);
        It should_reference_the_property = () => sut.PropertySetChanges.First().Property.ShouldEqual(prop);
        It should_reference_the_child_object = () => sut.PropertySetChanges.First().Value.ShouldEqual(val);

        static object obj;
        static Property prop;
        static object val;
    }

    public class when_a_collection_is_added_to : with_a_new_TrackedChanges
    {
        Establish c = () => {
            collection = A.Fake<ICollection<object>>();
            obj = new object();
        };

        Because of = () => sut.TrackAddToCollection(collection, obj);

        It should_contain_a_AddToCollectionChange = () => sut.AddToCollectionChanges.Count().ShouldEqual(1);
        It should_reference_the_collection = () => sut.AddToCollectionChanges.First().Collection.ShouldEqual(collection);
        It should_reference_the_added_object = () => sut.AddToCollectionChanges.First().Object.ShouldEqual(obj);

        static ICollection<object> collection;
        static object obj;
    }

    public class when_a_collection_has_an_object_removed : with_a_new_TrackedChanges
    {
        Establish c = () => {
            collection = A.Fake<ICollection<object>>();
            obj = new object();
        };

        Because of = () => sut.TrackRemoveFromCollection(collection, obj);

        It should_contain_a_RemoveFromCollectionChange = () => sut.RemoveFromCollectionChanges.Count().ShouldEqual(1);

        It should_reference_the_collection =
            () => sut.RemoveFromCollectionChanges.First().Collection.ShouldEqual(collection);

        It should_reference_the_removed_object = () => sut.RemoveFromCollectionChanges.First().Object.ShouldEqual(obj);

        static ICollection<object> collection;
        static object obj;
    }
}