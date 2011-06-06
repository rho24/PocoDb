﻿using System;
using System.Linq.Expressions;
using System.Reflection;
using PocoDb.Extensions;

namespace PocoDb.Meta
{
    public class Property<T, P> : IProperty
    {
        public PropertyInfo Info { get; private set; }

        public Property(Expression<Func<T, P>> expression) {
            var memberAccess = expression.Body as MemberExpression;
            if (memberAccess == null)
                throw new ArgumentException("expression is not a property");

            Info = memberAccess.Member as PropertyInfo;

            if (Info == null)
                throw new ArgumentException("expression is not a property");
        }

        public Property(MethodInfo method) {
            if (!method.IsProperty())
                throw new ArgumentException("method is not a property");

            Info = method.GetPropertyInfo();

            if (Info == null)
                throw new ArgumentException("method is not a property");

            if (Info.PropertyType != typeof (P))
                throw new ArgumentException("Property is not correct type");
        }

        public void Set(object poco, object value) {
            Info.SetValue(poco, value, null);
        }

        public object Get(object poco) {
            return Info.GetValue(poco, null);
        }

        public override bool Equals(object obj) {
            var other = obj as Property<T, P>;

            if (other == null)
                return false;

            return Info.Equals(other.Info);
        }

        public override int GetHashCode() {
            return Info.GetHashCode();
        }
    }
}