using System;
using System.Linq.Expressions;

namespace PocoDb.Meta
{
    public interface IPocoMetaBuilder
    {
        IPocoMeta Build(object poco);
        IPocoId Resolve(object poco);
    }
}