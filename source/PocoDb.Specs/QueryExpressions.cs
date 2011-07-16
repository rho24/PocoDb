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
        public static Expression DummyObjectWhereWithFieldConstant { get; private set; }
        public static Expression DummyObjectWhereWithPropertyConstant { get; private set; }
        public static Expression DummyObjectWhereWithMethodConstant { get; private set; }
        public static Expression ChildObjectIEnumerable { get; private set; }

        static QueryExpressions() {
            DummyObjectQueryable = new PocoQueryable<DummyObject>(new PocoQueryProvider(null));

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

            DummyObjectWhere = DummyObjectQueryable.Where(d => d.FirstName == "value").Expression;

            var field = "value";
            DummyObjectWhereWithFieldConstant = DummyObjectQueryable.Where(d => d.FirstName == field).Expression;

            var prop = new DummyObject() {FirstName = "value"};
            DummyObjectWhereWithPropertyConstant =
                DummyObjectQueryable.Where(d => d.FirstName == prop.FirstName).Expression;

            DummyObjectWhereWithMethodConstant = DummyObjectQueryable.Where(d => d.FirstName == GetValue()).Expression;
        }

        static string GetValue() {
            return "value";
        }
    }
}