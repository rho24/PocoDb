using System;
using PocoDb.ChangeTracking;

namespace PocoDb.Session
{
    public interface IInternalWriteablePocoSession : IInternalPocoSession
    {
        IChangeTracker ChangeTracker { get; }
    }
}