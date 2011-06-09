using System;
using System.Collections.Generic;
using Castle.DynamicProxy;
using PocoDb.Extensions;
using PocoDb.Meta;
using PocoDb.Session;

namespace PocoDb.Pocos
{
    public class PocoProxyPropertyGetterInterceptor<T> : IInterceptor
    {
        public IPocoMeta Meta { get; private set; }
        public ICanGetPocos PocoGetter { get; private set; }
        public List<IProperty> InitialisedProperties { get; private set; }

        public PocoProxyPropertyGetterInterceptor(IPocoMeta meta, ICanGetPocos pocoGetter) {
            Meta = meta;
            PocoGetter = pocoGetter;

            InitialisedProperties = new List<IProperty>();
        }

        public void Intercept(IInvocation invocation) {
            if (invocation.Method.IsPropertyGetter())
                GenericHelper.InvokeGeneric(() => Intercept<object>(invocation), invocation.Method.ReturnType);

            invocation.Proceed();
        }

        void Intercept<P>(IInvocation invocation) {
            var property = new Property<T, P>(invocation.Method);

            if (!InitialisedProperties.Contains(property)) {
                var value = Meta.Properties[property];
                if (value is IPocoId)
                    value = PocoGetter.GetPoco((IPocoId) value);

                InitialisedProperties.Add(property);
                property.Set(invocation.InvocationTarget, value);
            }
        }
    }
}