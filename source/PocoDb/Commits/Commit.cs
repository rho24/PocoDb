using System;
using System.Collections.Generic;
using PocoDb.Meta;

namespace PocoDb.Commits
{
    public class Commit : ICommit
    {
        public List<IPocoMeta> AddedMetas { get; private set; }
        public List<PropertySet> PropertySets { get; private set; }

        IEnumerable<IPocoMeta> ICommit.AddedMetas { get { return AddedMetas; } }
        IEnumerable<PropertySet> ICommit.PropertySets { get { return PropertySets; } }

        public Commit() {
            AddedMetas = new List<IPocoMeta>();
            PropertySets = new List<PropertySet>();
        }
    }
}