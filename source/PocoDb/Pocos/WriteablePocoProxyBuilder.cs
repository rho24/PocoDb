using System;
using System.Collections.Generic;
using Castle.DynamicProxy;
using PocoDb.Extensions;
using PocoDb.Meta;
using PocoDb.Session;

namespace PocoDb.Pocos
{
    public class WriteablePocoProxyBuilder : IPocoProxyBuilder
    {
        public IInternalPocoSession Session { get; private set; }
        public ProxyGenerator Generator { get; private set; }
        public ProxyGenerationOptions ProxyOptions { get; private set; }

        public WriteablePocoProxyBuilder() {
            Generator = new ProxyGenerator();
            ProxyOptions = new ProxyGenerationOptions(new PropertyHook());
        }

        public void Initialise(IInternalPocoSession session) {
            Session = session;
        }

        public object BuildProxy(IPocoMeta meta) {
            var propertyGetterInterceptor = GenericHelper.InvokeGeneric(
                () => new PocoProxyPropertyGetterInterceptor<object>(meta, Session), meta.Type) as IInterceptor;

            var propertySetterInterceptor = GenericHelper.InvokeGeneric(
                () => new WritablePropertyInterceptor<object>(meta, Session), meta.Type) as IInterceptor;

            return Generator.CreateClassProxy(meta.Type, ProxyOptions, propertyGetterInterceptor,
                                              propertySetterInterceptor);
        }

        public class WritablePropertyInterceptor<T> : IInterceptor
        {
            public IPocoMeta Meta { get; private set; }
            public IInternalPocoSession Session { get; private set; }
            public List<IProperty> InitialisedProperties { get; private set; }

            public WritablePropertyInterceptor(IPocoMeta meta, IInternalPocoSession session) {
                Meta = meta;
                Session = session;

                InitialisedProperties = new List<IProperty>();
            }

            public void Intercept(IInvocation invocation) {
                if (invocation.Method.IsPropertySetter())
                    GenericHelper.InvokeGeneric(() => Intercept<object>(invocation),
                                                invocation.Method.GetPropertyInfo().PropertyType);

                invocation.Proceed();
            }

            void Intercept<P>(IInvocation invocation) {
                var property = new Property<T, P>(invocation.Method);
                var poco = invocation.InvocationTarget;
                var currentValue = property.Get(poco);
                var newValue = invocation.Arguments[0];

                if (currentValue != newValue)
                    Session.Changes.TrackPropertySet(poco, property, newValue);
            }
        }
    }
}