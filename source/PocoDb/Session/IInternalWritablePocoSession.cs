using System;
using PocoDb.ChangeTracking;

namespace PocoDb.Session
{
    public interface IInternalWritablePocoSession : IInternalPocoSession
    {
        IChangeTracker ChangeTracker { get; }
    }
}