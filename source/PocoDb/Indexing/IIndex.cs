using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using PocoDb.Meta;

namespace PocoDb.Indexing
{
    public interface IIndex
    {
        IEnumerable<IPocoId> GetIds();

        bool IsExactMatch(Expression expression);
    }
}