using System;
using System.Collections.Generic;
using PocoDb.Meta;

namespace PocoDb.Commits
{
    public class Commit : ICommit
    {
        public List<IPocoMeta> AddedMetas { get; private set; }
        public List<PropertySet> PropertySets { get; private set; }
        public List<AddToCollection> AddToCollections { get; private set; }
        public List<RemoveFromCollection> RemoveFromCollections { get; private set; }

        IEnumerable<IPocoMeta> ICommit.AddedMetas { get { return AddedMetas; } }
        IEnumerable<PropertySet> ICommit.PropertySets { get { return PropertySets; } }
        IEnumerable<AddToCollection> ICommit.AddToCollections { get { return AddToCollections; } }
        IEnumerable<RemoveFromCollection> ICommit.RemoveFromCollections { get { return RemoveFromCollections; } }

        public Commit() {
            AddedMetas = new List<IPocoMeta>();
            PropertySets = new List<PropertySet>();
            AddToCollections = new List<AddToCollection>();
            RemoveFromCollections = new List<RemoveFromCollection>();
        }
    }
}