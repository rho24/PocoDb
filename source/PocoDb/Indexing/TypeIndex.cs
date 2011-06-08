using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using PocoDb.Meta;

namespace PocoDb.Indexing
{
    public class TypeIndex<T> : IIndex
    {
        public List<IPocoId> Ids { get; private set; }

        public TypeIndex() {
            Ids = new List<IPocoId>();
        }

        public IEnumerable<IPocoId> GetIds() {
            return Ids;
        }

        public IndexMatch GetMatch(Expression expression) {
            return expression.Type.GetGenericArguments()[0] == typeof (T)
                       ? IndexMatch.ExactMatch(this)
                       : IndexMatch.NoMatch(this);
        }
    }
}