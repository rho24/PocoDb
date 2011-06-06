using System;
using PocoDb.Meta;
using PocoDb.Session;

namespace PocoDb.Pocos
{
    public interface ICollectionProxyBuilder
    {
        void Initialise(IInternalPocoSession session);
        object BuildProxy(IPocoMeta meta);
    }
}