using System;
using PocoDb.Meta;

namespace PocoDb.Pocos
{
    public interface IPocoProxyBuilder
    {
        object BuildProxy(IPocoMeta meta);
    }
}