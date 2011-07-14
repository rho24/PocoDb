using System;
using System.Collections.Generic;
using PocoDb.Meta;

namespace PocoDb.Commits
{
    public class PocoUpdates
    {
        public IPocoId PocoId { get; private set; }
        public ICollection<SetProperty> SetProperties { get; private set; }

        public PocoUpdates(IPocoId pocoId) {
            PocoId = pocoId;
            SetProperties = new List<SetProperty>();
        }
    }
}