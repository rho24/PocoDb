using System;
using System.Linq.Expressions;

namespace PocoDb
{
    public interface IPocoQuery
    {
        Expression Expression { get; }
    }
}