using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using FakeItEasy;
using PocoDb.ChangeTracking;
using System;

namespace PocoDb.Specs
{
    [Subject(typeof (ITrackedChanges), "Adding an object")]
    public class when_an_object_is_added : with_a_new_TrackedChanges
    {
        Establish c = () => { obj = new object(); };

        Because of = () => sut.TrackAddedObject(obj);

        It should_contain_an_AddObjectChange = () => sut.AddObjectChanges.Count().ShouldEqual(1);
        It should_reference_the_added_object = () => sut.AddObjectChanges.First().Object.ShouldEqual(obj);

        static object obj;
    }

    [Subject(typeof (ITrackedChanges), "Adding an object")]
    public class when_an_object_is_added_again : with_an_object_added
    {
        Because of = () => sut.TrackAddedObject(obj);

        It should_only_track_the_object_once = () => sut.AddObjectChanges.Count().ShouldEqual(1);
    }

    [Subject(typeof (ITrackedChanges), "Adding an object")]
    public class when_the_added_object_is_null : with_a_new_TrackedChanges
    {
        Because of = () => spec.catch_exception(() => sut.TrackAddedObject(null));

        It should_throw_an_argument_null_exception = () => spec.exception_thrown.ShouldBeOfType<ArgumentNullException>();
    }

    [Subject(typeof (ITrackedChanges), "Setting a property")]
    public class when_a_property_is_set : with_a_new_TrackedChanges
    {
        Establish c = () => {
            obj = new object();
            prop = A.Fake<IProperty>();
            val = new object();
        };

        Because of = () => sut.TrackPropertySet(obj, prop, val);

        It should_contain_a_PropertySetChange = () => sut.PropertySetChanges.Count().ShouldEqual(1);
        It should_reference_the_parent_object = () => sut.PropertySetChanges.First().Object.ShouldEqual(obj);
        It should_reference_the_property = () => sut.PropertySetChanges.First().Property.ShouldEqual(prop);
        It should_reference_the_child_object = () => sut.PropertySetChanges.First().Value.ShouldEqual(val);

        static object obj;
        static IProperty prop;
        static object val;
    }

    [Subject(typeof (ITrackedChanges), "Setting a property")]
    public class when_an_property_is_set_again : with_a_property_set
    {
        Establish c = () => newVal = new object();

        Because of = () => sut.TrackPropertySet(obj, prop, newVal);

        It should_only_track_one_property_set = () => sut.PropertySetChanges.Count().ShouldEqual(1);
        It should_reference_the_new_value = () => sut.PropertySetChanges.First().Value.ShouldEqual(newVal);

        static object newVal;
    }

    [Subject(typeof (ITrackedChanges), "Setting a property")]
    public class when_a_property_is_set_with_a_null_parent : with_a_new_TrackedChanges
    {
        Establish c = () => {
            obj = null;
            prop = A.Fake<IProperty>();
            val = new object();
        };

        Because of = () => spec.catch_exception(() => sut.TrackPropertySet(obj, prop, val));

        It should_throw_an_argument_null_exception = () => spec.exception_thrown.ShouldBeOfType<ArgumentNullException>();

        static object obj;
        static IProperty prop;
        static object val;
    }

    [Subject(typeof (ITrackedChanges), "Setting a property")]
    public class when_a_property_is_set_with_a_null_property : with_a_new_TrackedChanges
    {
        Establish c = () => {
            obj = new object();
            prop = null;
            val = new object();
        };

        Because of = () => spec.catch_exception(() => sut.TrackPropertySet(obj, prop, val));

        It should_throw_an_argument_null_exception = () => spec.exception_thrown.ShouldBeOfType<ArgumentNullException>();

        static object obj;
        static IProperty prop;
        static object val;
    }

    [Subject(typeof (ITrackedChanges), "Setting a property")]
    public class when_a_property_is_set_with_a_null_value : with_a_new_TrackedChanges
    {
        Establish c = () => {
            obj = new object();
            prop = A.Fake<IProperty>();
            val = null;
        };

        Because of = () => sut.TrackPropertySet(obj, prop, val);

        It should_contain_a_PropertySetChange = () => sut.PropertySetChanges.Count().ShouldEqual(1);
        It should_reference_the_parent_object = () => sut.PropertySetChanges.First().Object.ShouldEqual(obj);
        It should_reference_the_property = () => sut.PropertySetChanges.First().Property.ShouldEqual(prop);
        It should_reference_the_child_object = () => sut.PropertySetChanges.First().Value.ShouldEqual(val);

        static object obj;
        static IProperty prop;
        static object val;
    }

    [Subject(typeof (ITrackedChanges), "Adding to a collection")]
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

    [Subject(typeof (ITrackedChanges), "Adding to a collection")]
    public class when_a_collection_is_added_to_with_a_null_collection : with_a_new_TrackedChanges
    {
        Establish c = () => {
            collection = null;
            obj = new object();
        };

        Because of = () => spec.catch_exception(() => sut.TrackAddToCollection(collection, obj));

        It should_throw_an_argument_null_exception = () =>
                                                     spec.exception_thrown.ShouldBeOfType<ArgumentNullException>();

        static ICollection<object> collection;
        static object obj;
    }

    [Subject(typeof (ITrackedChanges), "Removing from a collection")]
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

    [Subject(typeof (ITrackedChanges), "Removing from a collection")]
    public class when_a_collection_has_an_object_removed_with_a_null_collection : with_a_new_TrackedChanges
    {
        Establish c = () => {
            collection = null;
            obj = new object();
        };

        Because of = () => spec.catch_exception(() => sut.TrackRemoveFromCollection(collection, obj));

        It should_throw_an_argument_null_exception = () => spec.exception_thrown.ShouldBeOfType<ArgumentNullException>();

        static ICollection<object> collection;
        static object obj;
    }
}