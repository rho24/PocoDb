using System;
using System.Linq.Expressions;

namespace PocoDb
{
    public interface IQuery
    {
        Expression Expression { get; }
    }
}