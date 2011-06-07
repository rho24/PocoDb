using System;
using PocoDb.Meta;

namespace PocoDb.Pocos
{
    public interface ICollectionProxyBuilder
    {
        object BuildProxy(IPocoMeta meta);
    }
}