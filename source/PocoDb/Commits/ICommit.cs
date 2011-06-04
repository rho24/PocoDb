using System;
using System.Collections.Generic;

namespace PocoDb.Commits
{
    public interface ICommit
    {
        IEnumerable<AddObject> AddObjects { get; }
        IEnumerable<PropertySet> PropertySets { get; }
        IEnumerable<AddToCollection> AddToCollections { get; }
        IEnumerable<RemoveFromCollection> RemoveFromCollections { get; }
    }
}