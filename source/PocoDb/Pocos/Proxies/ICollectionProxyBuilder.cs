using System;
using PocoDb.Meta;

namespace PocoDb.Pocos.Proxies
{
    public interface ICollectionProxyBuilder
    {
        object BuildProxy(IPocoMeta meta);
    }
}