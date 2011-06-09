using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoDb.Linq
{
    public interface IQueryableToEnumerableConverter
    {
        Expression Convert<T>(Expression expression, Expression replace, IEnumerable<T> with);
    }
}