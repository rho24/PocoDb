using System.Collections.Generic;
using PocoDb.Meta;

namespace PocoDb.Commits
{
    public interface ICommit
    {
        IEnumerable<IPocoMeta> AddedMetas { get; }
        IEnumerable<PropertySet> PropertySets { get; }
        IEnumerable<AddToCollection> AddToCollections { get; }
        IEnumerable<RemoveFromCollection> RemoveFromCollections { get; }
    }
}