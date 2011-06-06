using System;
using PocoDb.Meta;
using PocoDb.Session;

namespace PocoDb.Pocos
{
    public interface IPocoFactory
    {
        void Initialise(IInternalPocoSession session);
        object Build(IPocoMeta meta);
    }
}