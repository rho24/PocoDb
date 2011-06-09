using System;
using PocoDb.Pocos;
using PocoDb.Server;

namespace PocoDb.Session
{
    public interface IInternalPocoSession : ICanGetPocos
    {
        IPocoDbServer Server { get; }
        IIdsMetasAndProxies IdsMetasAndProxies { get; }
    }
}