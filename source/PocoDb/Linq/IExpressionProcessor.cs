using System;
using System.Linq.Expressions;

namespace PocoDb.Linq
{
    public interface IExpressionProcessor
    {
        Expression Process(Expression expression);
    }
}