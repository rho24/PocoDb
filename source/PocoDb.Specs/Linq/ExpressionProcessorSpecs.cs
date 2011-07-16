using System;
using System.Linq.Expressions;
using Machine.Specifications;
using PocoDb.Extensions;

namespace PocoDb.Specs.Linq
{
    public class when_an_Enumerable_query_is_processed : with_a_new_ExpressionProcessor
    {
        Establish c = () => { expression = QueryExpressions.DummyObjectIEnumerable; };

        Because of = () => result = sut.Process(expression);

        It should_return_the_original = () => result.ShouldEqual(expression);

        static Expression expression;
        static Expression result;
    }

    public class when_a_Single_query_with_constant_is_processed : with_a_new_ExpressionProcessor
    {
        Establish c = () => { expression = QueryExpressions.DummyObjectSingle; };

        Because of = () => result = sut.Process(expression);

        It should_return_the_original = () => result.ShouldEqual(expression);

        static Expression expression;
        static Expression result;
    }

    public class when_a_Where_query_with_a_literal_constant_is_processed : with_a_new_ExpressionProcessor
    {
        Establish c = () => { expression = QueryExpressions.DummyObjectWhere; };

        Because of = () => result = sut.Process(expression);

        It should_return_the_original = () => result.ShouldEqual(expression);

        static Expression expression;
        static Expression result;
    }

    public class when_a_Where_query_with_a_field_constant_is_processed : with_a_new_ExpressionProcessor
    {
        Establish c = () => { expression = QueryExpressions.DummyObjectWhereWithFieldConstant; };

        Because of = () => result = sut.Process(expression);

        It should_replace_with_value = () =>
                                       result.As<MethodCallExpression>()
                                           .Arguments[1].As<UnaryExpression>()
                                           .Operand.As<LambdaExpression>()
                                           .Body.As<BinaryExpression>()
                                           .Right.As<ConstantExpression>()
                                           .Value.ShouldEqual("value");

        static Expression expression;
        static Expression result;
    }

    public class when_a_Where_query_with_a_property_constant_is_processed : with_a_new_ExpressionProcessor
    {
        Establish c = () => { expression = QueryExpressions.DummyObjectWhereWithPropertyConstant; };

        Because of = () => result = sut.Process(expression);

        It should_replace_with_value = () =>
                                       result.As<MethodCallExpression>()
                                           .Arguments[1].As<UnaryExpression>()
                                           .Operand.As<LambdaExpression>()
                                           .Body.As<BinaryExpression>()
                                           .Right.As<ConstantExpression>()
                                           .Value.ShouldEqual("value");

        static Expression expression;
        static Expression result;
    }
}