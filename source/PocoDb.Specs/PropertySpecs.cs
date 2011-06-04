using Machine.Specifications;
using PocoDb.Meta;

namespace PocoDb.Specs
{
    [Subject(typeof (IProperty), "Equality")]
    public class when_a_property_is_compared_to_itself
    {
        Establish c = () => { prop = new Property<DummyObject, string>(o => o.FirstName); };

        It should_be_equal = () => prop.ShouldEqual(prop);
        It should_have_the_same_hash_code = () => prop.GetHashCode().ShouldEqual(prop.GetHashCode());

        static IProperty prop;
    }

    [Subject(typeof (IProperty), "Equality")]
    public class when_a_property_is_compared_to_a_copy
    {
        Establish c = () => {
            prop1 = new Property<DummyObject, string>(o => o.FirstName);
            prop2 = new Property<DummyObject, string>(o => o.FirstName);
        };

        It should_be_equal = () => prop1.ShouldEqual(prop2);
        It should_have_the_same_hash_code = () => prop1.GetHashCode().ShouldEqual(prop2.GetHashCode());

        static IProperty prop1;
        static IProperty prop2;
    }

    [Subject(typeof (IProperty), "Equality")]
    public class when_a_property_is_compared_to_a_different_property
    {
        Establish c = () => {
            prop1 = new Property<DummyObject, string>(o => o.FirstName);
            prop2 = new Property<DummyObject, string>(o => o.LastName);
        };

        It should_be_not_be_equal = () => prop1.ShouldNotEqual(prop2);

        static IProperty prop1;
        static IProperty prop2;
    }
}