using System;
using System.Collections.Generic;

namespace PocoDb.Meta
{
    public interface IPocoMeta
    {
        IPocoId Id { get; }
        IDictionary<IProperty, object> Properties { get; }
        ICollection<object> Collection { get; }
    }
}