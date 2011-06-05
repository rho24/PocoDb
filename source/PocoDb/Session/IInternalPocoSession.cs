using System.Collections.Generic;
using PocoDb.Meta;
using PocoDb.Server;

namespace PocoDb.Session
{
    public interface IInternalPocoSession
    {
        IPocoDbServer Server { get; }
        ICollection<IPocoMeta> Metas { get; }
        object GetPoco(IPocoId id);
    }
}