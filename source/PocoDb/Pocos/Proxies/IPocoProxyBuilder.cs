using PocoDb.Meta;

namespace PocoDb.Pocos.Proxies
{
    public interface IPocoProxyBuilder
    {
        object BuildProxy(IPocoMeta meta);
    }
}