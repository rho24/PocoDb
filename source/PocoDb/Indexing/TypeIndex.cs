using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using PocoDb.Extensions;
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

        public bool IsExactMatch(Expression expression) {
            if (!expression.IsPocoQuery())
                throw new ArgumentException("expression is not a PocoQuery");

            return expression.Type.GetGenericArguments()[0] == typeof (T);
        }
    }
}