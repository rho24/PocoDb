using System;
using System.Linq.Expressions;

namespace PocoDb.Queries
{
    public interface IQuery
    {
        Expression Expression { get; }
    }
}