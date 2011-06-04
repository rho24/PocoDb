using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.ChangeTracking;
using PocoDb.Commits;
using PocoDb.Meta;

namespace PocoDb.Specs
{
    public class when_a_new_object_is_being_tracked : with_a_new_CommitBuilder
    {
        Establish c = () => {
            poco = new DummyObject();

            trackedChanges.TrackAddedObject(poco);
        };

        It should_contain_an_added_PocoMeta = () => commit.AddedMetas.Count().ShouldEqual(1);
        It should_create_a_PocoMeta = () => A.CallTo(() => pocoMetaBuilder.Build(poco)).MustHaveHappened();

        static DummyObject poco;
    }

    public class when_a_property_set_is_being_tracked : with_a_new_CommitBuilder
    {
        Establish c = () => {
            poco = new DummyObject();
            property = new Property<DummyObject, string>(o => o.FirstName);

            trackedChanges.TrackPropertySet(poco, property, "Changed");

            pocoId = A.Fake<IPocoId>();

            A.CallTo(() => pocoMetaBuilder.Resolve(poco)).Returns(pocoId);
        };

        It should_contain_a_set_property = () => commit.PropertySets.Count().ShouldEqual(1);
        It should_reference_the_PocoId = () => commit.PropertySets.First().ParentId.ShouldEqual(pocoId);
        It should_reference_the_Property = () => commit.PropertySets.First().Property.ShouldEqual(property);
        It should_reference_the_value = () => commit.PropertySets.First().Value.ShouldEqual("Changed");
        It should_resolve_the_PocoMeta = () => A.CallTo(() => pocoMetaBuilder.Resolve(poco)).MustHaveHappened();

        static DummyObject poco;
        static IPocoId pocoId;
        static IProperty property;
    }
}