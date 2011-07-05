using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.Extensions;
using developwithpassion.specifications.fakeiteasy;

namespace PocoDb.Specs
{
    [Subject(typeof (GenericHelper))]
    public class when_InvokeGeneric_is_called_with_a_non_method : Observes
    {
        Because of = () => spec.catch_exception(() => GenericHelper.InvokeGeneric(() => "", typeof (object)));

        It should_throw_an_ArgumentException = () => spec.exception_thrown.ShouldBeOfType<ArgumentException>();
    }

    [Subject(typeof (GenericHelper))]
    public class when_InvokeGeneric_is_called_with_a_non_generic_method : Observes
    {
        Because of =
            () =>
            spec.catch_exception(() => GenericHelper.InvokeGeneric(() => string.IsNullOrEmpty(""), typeof (object)));

        It should_throw_an_ArgumentException = () => spec.exception_thrown.ShouldBeOfType<ArgumentException>();
    }

    [Subject(typeof (GenericHelper))]
    public class when_InvokeGeneric_is_called_with_static_method : Observes
    {
        Because of = () => result = GenericHelper.InvokeGeneric(() => Enumerable.Empty<object>(), typeof (string));

        It should_return_the_correct_type = () => result.ShouldBeOfType<IEnumerable<string>>();

        static object result;
    }

    [Subject(typeof (GenericHelper))]
    public class when_InvokeGeneric_is_called_with_a_constructor_method : Observes
    {
        Because of = () => result = GenericHelper.InvokeGeneric(() => new List<object>(), typeof (string));

        It should_return_the_correct_type = () => result.ShouldBeOfType<List<string>>();

        static object result;
    }

    [Subject(typeof (GenericHelper))]
    public class when_InvokeGeneric_is_called_with_a_generic_method_with_a_return_value : Observes
    {
        Establish c = () => { testClass = A.Fake<IHasGenericMethod>(); };

        Because of = () => result = GenericHelper.InvokeGeneric(() => testClass.Test<object>(), typeof (DummyObject));

        It should_execute_the_method = () => A.CallTo(testClass).MustHaveHappened();
        It should_return_the_value = () => result.ShouldBeOfType<DummyObject>();

        static IHasGenericMethod testClass;
        static object result;
    }

    [Subject(typeof (GenericHelper))]
    public class when_InvokeGeneric_is_called_with_a_generic_method_with_an_input_value : Observes
    {
        Establish c = () => {
            testClass = A.Fake<IHasGenericMethod>();
            input = A.Fake<DummyObject>();
        };

        Because of = () => GenericHelper.InvokeGeneric(() => testClass.Test2<object>(input), typeof (DummyObject));

        It should_execute_the_method = () => A.CallTo(testClass).MustHaveHappened();
        It should_recieve_the_input_value = () => A.CallTo(() => testClass.Test2(input)).MustHaveHappened();

        static IHasGenericMethod testClass;
        static DummyObject input;
    }

    public interface IHasGenericMethod
    {
        T Test<T>();

        T Test2<T>(T input);
    }
}