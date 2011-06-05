using System.Linq;
using PocoDb.ChangeTracking;

namespace PocoDb.Session
{
    public interface IWritablePocoSession : IPocoSession
    {
        ITrackedChanges Changes { get; }

        IQueryable<T> GetWritable<T>();
        void Add<T>(T poco);
        void SaveChanges();
    }
}