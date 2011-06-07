using System;
using System.Collections.Generic;
using PocoDb.ChangeTracking;
using PocoDb.Meta;
using PocoDb.Server;

namespace PocoDb.Session
{
    public interface IInternalPocoSession
    {
        IPocoDbServer Server { get; }
        IDictionary<IPocoId, IPocoMeta> Metas { get; }
        IDictionary<IPocoId, object> TrackedPocos { get; }
        object GetPoco(IPocoId id);
    }

    public interface IInternalWriteablePocoSession : IInternalPocoSession
    {
        ITrackedChanges Changes { get; }
    }
}