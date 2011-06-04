using System;
using System.Linq;

namespace PocoDb
{
    public interface IPocoSession:IDisposable
    {
        IQueryable<T> Get<T>();
    }
}