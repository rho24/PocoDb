using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace PocoDb.Meta
{
    public class Property<T,P> : IProperty
    {
        private Expression<Func<T,P>> Exp { get; set; }
        //public Type ParentType { get; private set; }
        //public string PropertyName { get; private set; }

        public Property(Expression<Func<T,P>> property) {
            Exp = property;
        }

        public override bool Equals(object obj) {
            var other = obj as Property<T,P>;

            if(other == null)
                return base.Equals(obj);
            
            return Exp.Type.Equals(other.Exp.Type) &&
                (Exp.Body as MemberExpression).Member.Name.Equals((other.Exp.Body as MemberExpression).Member.Name);
        }

        public override int GetHashCode() {
            return Exp.Type.GetHashCode() ^ (Exp.Body as MemberExpression).Member.Name.GetHashCode();
        }
    }
}