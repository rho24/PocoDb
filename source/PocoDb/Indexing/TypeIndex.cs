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

        public IndexMatch GetMatch(Expression expression) {
            if (expression.GetQueryPocoType() != typeof (T))
                return IndexMatch.NoMatch(this);

            if (expression.IsQueryBase())
                return IndexMatch.ExactMatch(this);

            var depth = 0;
            while (!expression.IsQueryBase()) {
                depth++;
                expression = expression.GetInnerQuery();
            }

            return IndexMatch.PartialMatch(this, depth);
        }

        public void NotifyMetaChange(IPocoMeta meta) {
            if (meta.Type == typeof (T) && !Ids.Contains(meta.Id))
                Ids.Add(meta.Id);
        }
    }
}