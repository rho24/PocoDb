using System;
using developwithpassion.specifications.fakeiteasy;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.Extensions;

namespace PocoDb.Specs
{
    [Subject(typeof (LambdaExtensions))]
    public class when_InvokeGeneric_is_called_with_a_non_method : Observes
    {
        Because of = () => spec.catch_exception(() => LambdaExtensions.InvokeGeneric(() => "", typeof (object)));

        It should_throw_an_ArgumentException = () => spec.exception_thrown.ShouldBeOfType<ArgumentException>();
    }

    [Subject(typeof (LambdaExtensions))]
    public class when_InvokeGeneric_is_called_with_a_non_generic : Observes
    {
        Because of =
            () =>
            spec.catch_exception(() => LambdaExtensions.InvokeGeneric(() => string.IsNullOrEmpty(""), typeof (object)));

        It should_throw_an_ArgumentException = () => spec.exception_thrown.ShouldBeOfType<ArgumentException>();
    }

    [Subject(typeof (LambdaExtensions))]
    public class when_InvokeGeneric_is_called_with_a_generic_method_with_a_return_value : Observes
    {
        Establish c = () => { testClass = A.Fake<IHasGenericMethod>(); };

        Because of = () => result = LambdaExtensions.InvokeGeneric(() => testClass.Test<object>(), typeof (DummyObject));

        It should_execute_the_method = () => A.CallTo(testClass).MustHaveHappened();
        It should_return_the_value = () => result.ShouldBeOfType<DummyObject>();

        static IHasGenericMethod testClass;
        static object result;
    }

    [Subject(typeof (LambdaExtensions))]
    public class when_InvokeGeneric_is_called_with_a_generic_method_with_an_input_value : Observes
    {
        Establish c = () => {
            testClass = A.Fake<IHasGenericMethod>();
            input = A.Fake<DummyObject>();
        };

        Because of = () => LambdaExtensions.InvokeGeneric(() => testClass.Test2<object>(input), typeof (DummyObject));

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