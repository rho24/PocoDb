using System;
using System.Linq;
using System.Linq.Expressions;
using PocoDb.Linq;

namespace PocoDb.Specs
{
    public static class QueryExpressions
    {
        public static IQueryable<DummyObject> DummyObjectQueryable { get; private set; }
        public static Expression DummyObjectIEnumerable { get; private set; }
        public static Expression DummyObjectFirst { get; private set; }
        public static Expression DummyObjectFirstOrDefault { get; private set; }
        public static Expression DummyObjectSingle { get; private set; }
        public static Expression DummyObjectWhere { get; private set; }
        public static Expression ChildObjectIEnumerable { get; private set; }

        static QueryExpressions() {
            DummyObjectQueryable = new PocoQueryable<DummyObject>(null);

            DummyObjectIEnumerable = Expression.Constant(DummyObjectQueryable);
            ChildObjectIEnumerable = Expression.Constant(new PocoQueryable<ChildObject>(null));

            var firstMethod = typeof (Queryable).GetMethods().Where(m => m.Name.StartsWith("First")).First();
            var firstOrDefaultMethod =
                typeof (Queryable).GetMethods().Where(m => m.Name.StartsWith("FirstOrDefault")).First();
            var singleMethod = typeof (Queryable).GetMethods().Where(m => m.Name.StartsWith("Single")).First();
            var whereMethod = typeof (Queryable).GetMethods().Where(m => m.Name.StartsWith("Where")).First();

            DummyObjectFirst = Expression.Call(firstMethod.MakeGenericMethod(typeof (DummyObject)),
                                               DummyObjectIEnumerable);

            DummyObjectFirstOrDefault = Expression.Call(firstOrDefaultMethod.MakeGenericMethod(typeof (DummyObject)),
                                                        DummyObjectIEnumerable);

            DummyObjectSingle = Expression.Call(singleMethod.MakeGenericMethod(typeof (DummyObject)),
                                                DummyObjectIEnumerable);

            DummyObjectWhere = Expression.Call(whereMethod.MakeGenericMethod(typeof (DummyObject)),
                                               DummyObjectIEnumerable,
                                               (Expression<Func<DummyObject, bool>>) (d => d.FirstName == "value"));
        }
    }
}