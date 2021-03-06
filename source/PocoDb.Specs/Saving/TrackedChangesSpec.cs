﻿using System;
using System.Collections;
using System.Linq;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.ChangeTracking;
using PocoDb.Meta;

namespace PocoDb.Specs.Saving
{
    [Subject(typeof (ITrackedChanges), "Adding an object")]
    public class when_an_object_is_added : with_a_new_TrackedChanges
    {
        Establish c = () => { poco = new object(); };

        Because of = () => sut.TrackAddedObject(poco);

        It should_contain_an_AddObjectChange = () => sut.Changes.AddedPocos.Count().ShouldEqual(1);
        It should_reference_the_added_object = () => sut.Changes.AddedPocos.First().Poco.ShouldEqual(poco);

        static object poco;
    }

    [Subject(typeof (ITrackedChanges), "Adding an object")]
    public class when_an_object_is_added_again : with_an_object_added
    {
        Because of = () => sut.TrackAddedObject(poco);

        It should_only_track_the_object_once = () => sut.Changes.AddedPocos.Count().ShouldEqual(1);
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
            poco = new object();
            property = A.Fake<IProperty>();
            value = new object();
        };

        Because of = () => sut.TrackPropertySet(poco, property, value);

        It should_contain_a_PropertySetChange = () => sut.Changes.SetProperties.Count().ShouldEqual(1);
        It should_reference_the_parent_object = () => sut.Changes.SetProperties.First().Poco.ShouldEqual(poco);
        It should_reference_the_property = () => sut.Changes.SetProperties.First().Property.ShouldEqual(property);
        It should_reference_the_child_object = () => sut.Changes.SetProperties.First().Value.ShouldEqual(value);

        static object poco;
        static IProperty property;
        static object value;
    }

    [Subject(typeof (ITrackedChanges), "Setting a property")]
    public class when_an_property_is_set_again : with_a_property_set
    {
        Establish c = () => newValue = new object();

        Because of = () => sut.TrackPropertySet(poco, property, newValue);

        It should_only_track_one_property_set = () => sut.Changes.SetProperties.Count().ShouldEqual(1);
        It should_reference_the_new_value = () => sut.Changes.SetProperties.First().Value.ShouldEqual(newValue);

        static object newValue;
    }

    [Subject(typeof (ITrackedChanges), "Setting a property")]
    public class when_a_property_is_set_with_a_null_parent : with_a_new_TrackedChanges
    {
        Establish c = () => {
            poco = null;
            property = A.Fake<IProperty>();
            value = new object();
        };

        Because of = () => spec.catch_exception(() => sut.TrackPropertySet(poco, property, value));

        It should_throw_an_argument_null_exception = () => spec.exception_thrown.ShouldBeOfType<ArgumentNullException>();

        static object poco;
        static IProperty property;
        static object value;
    }

    [Subject(typeof (ITrackedChanges), "Setting a property")]
    public class when_a_property_is_set_with_a_null_property : with_a_new_TrackedChanges
    {
        Establish c = () => {
            poco = new object();
            property = null;
            value = new object();
        };

        Because of = () => spec.catch_exception(() => sut.TrackPropertySet(poco, property, value));

        It should_throw_an_argument_null_exception = () => spec.exception_thrown.ShouldBeOfType<ArgumentNullException>();

        static object poco;
        static IProperty property;
        static object value;
    }

    [Subject(typeof (ITrackedChanges), "Setting a property")]
    public class when_a_property_is_set_with_a_null_value : with_a_new_TrackedChanges
    {
        Establish c = () => {
            poco = new object();
            property = A.Fake<IProperty>();
            value = null;
        };

        Because of = () => sut.TrackPropertySet(poco, property, value);

        It should_contain_a_PropertySetChange = () => sut.Changes.SetProperties.Count().ShouldEqual(1);
        It should_reference_the_parent_object = () => sut.Changes.SetProperties.First().Poco.ShouldEqual(poco);
        It should_reference_the_property = () => sut.Changes.SetProperties.First().Property.ShouldEqual(property);
        It should_reference_the_child_object = () => sut.Changes.SetProperties.First().Value.ShouldEqual(value);

        static object poco;
        static IProperty property;
        static object value;
    }

    [Subject(typeof (ITrackedChanges), "Adding to a collection")]
    public class when_a_collection_is_added_to : with_a_new_TrackedChanges
    {
        Establish c = () => {
            collection = A.Fake<ICollection>();
            value = new object();
        };

        Because of = () => sut.TrackAddToCollection(collection, value);

        It should_contain_a_AddToCollectionChange = () => sut.Changes.CollectionAdditions.Count().ShouldEqual(1);

        It should_reference_the_collection =
            () => sut.Changes.CollectionAdditions.First().Collection.ShouldEqual(collection);

        It should_reference_the_added_object = () => sut.Changes.CollectionAdditions.First().Value.ShouldEqual(value);

        static ICollection collection;
        static object value;
    }

    [Subject(typeof (ITrackedChanges), "Adding to a collection")]
    public class when_a_collection_is_added_to_with_a_null_collection : with_a_new_TrackedChanges
    {
        Establish c = () => {
            collection = null;
            value = new object();
        };

        Because of = () => spec.catch_exception(() => sut.TrackAddToCollection(collection, value));

        It should_throw_an_argument_null_exception = () => spec.exception_thrown.ShouldBeOfType<ArgumentNullException>();

        static ICollection collection;
        static object value;
    }

    [Subject(typeof (ITrackedChanges), "Removing from a collection")]
    public class when_a_collection_has_an_object_removed : with_a_new_TrackedChanges
    {
        Establish c = () => {
            collection = A.Fake<ICollection>();
            value = new object();
        };

        Because of = () => sut.TrackRemoveFromCollection(collection, value);

        It should_contain_a_RemoveFromCollectionChange = () => sut.Changes.CollectionRemovals.Count().ShouldEqual(1);

        It should_reference_the_collection =
            () => sut.Changes.CollectionRemovals.First().Collection.ShouldEqual(collection);

        It should_reference_the_removed_object = () => sut.Changes.CollectionRemovals.First().Value.ShouldEqual(value);

        static ICollection collection;
        static object value;
    }

    [Subject(typeof (ITrackedChanges), "Removing from a collection")]
    public class when_a_collection_has_an_object_removed_with_a_null_collection : with_a_new_TrackedChanges
    {
        Establish c = () => {
            collection = null;
            value = new object();
        };

        Because of = () => spec.catch_exception(() => sut.TrackRemoveFromCollection(collection, value));

        It should_throw_an_argument_null_exception = () => spec.exception_thrown.ShouldBeOfType<ArgumentNullException>();

        static ICollection collection;
        static object value;
    }
}