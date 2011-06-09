using System;
using System.Linq;
using PocoDb.Pocos;

namespace PocoDb
{
    public interface IPocoSession : IDisposable
    {
        IQueryable<T> Get<T>();
    }
}