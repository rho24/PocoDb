using System.Linq.Expressions;

namespace PocoDb.Linq
{
    public interface IPocoQueryableExecutor
    {
        T Execute<T>(Expression expression);
    }
}