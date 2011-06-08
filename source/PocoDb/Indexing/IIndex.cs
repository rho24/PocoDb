using System;
using System.Collections.Generic;
using PocoDb.Meta;

namespace PocoDb.Indexing
{
    public interface IIndex
    {
        IEnumerable<IPocoId> GetIds();
    }
}