using System;
using System.Collections.Generic;

namespace PocoDb.Commits
{
    public class Commit : ICommit
    {
        public List<AddObject> AddObjects { get; private set; }
        public List<PropertySet> PropertySets { get; private set; }
        public List<AddToCollection> AddToCollections { get; private set; }
        public List<RemoveFromCollection> RemoveFromCollections { get; private set; }

        IEnumerable<AddObject> ICommit.AddObjects { get { return AddObjects; } }
        IEnumerable<PropertySet> ICommit.PropertySets { get { return PropertySets; } }
        IEnumerable<AddToCollection> ICommit.AddToCollections { get { return AddToCollections; } }
        IEnumerable<RemoveFromCollection> ICommit.RemoveFromCollections { get { return RemoveFromCollections; } }

        public Commit() {
            AddObjects = new List<AddObject>();
            PropertySets = new List<PropertySet>();
            AddToCollections = new List<AddToCollection>();
            RemoveFromCollections = new List<RemoveFromCollection>();
        }
    }
}