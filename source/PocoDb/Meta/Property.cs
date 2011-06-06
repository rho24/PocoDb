using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoDb.Meta
{
    public class Property<T, P> : IProperty
    {
        Expression<Func<T, P>> Expression { get; set; }

        public Property(Expression<Func<T, P>> expression) {
            Expression = expression;
        }

        public void Set(object poco, object value) {
            var prop = ((Expression.Body as MemberExpression).Member) as PropertyInfo;

            prop.SetValue(poco, value, null);
        }

        public override bool Equals(object obj) {
            var other = obj as Property<T, P>;

            if (other == null)
                return base.Equals(obj);

            return Expression.Type.Equals(other.Expression.Type) &&
                   (Expression.Body as MemberExpression).Member.Name.Equals(
                       (other.Expression.Body as MemberExpression).Member.Name);
        }

        public override int GetHashCode() {
            return Expression.Type.GetHashCode() ^ (Expression.Body as MemberExpression).Member.Name.GetHashCode();
        }
    }
}