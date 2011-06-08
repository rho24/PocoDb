using System;
using PocoDb.Meta;
using PocoDb.Pocos;
using PocoDb.Server;

namespace PocoDb.Session
{
    public interface IInternalPocoSession
    {
        IPocoDbServer Server { get; }
        IIdsMetasAndProxies IdsMetasAndProxies { get; }
        object GetPoco(IPocoId id);
    }
}