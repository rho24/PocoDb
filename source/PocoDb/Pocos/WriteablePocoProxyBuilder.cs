using System;
using System.Collections.Generic;
using Castle.DynamicProxy;
using PocoDb.ChangeTracking;
using PocoDb.Extensions;
using PocoDb.Meta;

namespace PocoDb.Pocos
{
    public class WriteablePocoProxyBuilder : IPocoProxyBuilder
    {
        public ICanGetPocos PocoGetter { get; private set; }
        public IChangeTracker ChangeTracker { get; private set; }
        public ProxyGenerator Generator { get; private set; }
        public ProxyGenerationOptions ProxyOptions { get; private set; }

        public WriteablePocoProxyBuilder() {
            Generator = new ProxyGenerator();
            ProxyOptions = new ProxyGenerationOptions(new PropertyHook());
        }

        public void Initialise(ICanGetPocos pocoGetter, IChangeTracker changeTracker) {
            PocoGetter = pocoGetter;
            ChangeTracker = changeTracker;
        }

        public object BuildProxy(IPocoMeta meta) {
            var propertyInterceptor = GenericHelper.InvokeGeneric(
                () => new WritablePropertyInterceptor<object>(meta, PocoGetter, ChangeTracker), meta.Type) as
                                      IInterceptor;

            return Generator.CreateClassProxy(meta.Type, ProxyOptions, propertyInterceptor);
        }

        public class WritablePropertyInterceptor<T> : IInterceptor
        {
            public IPocoMeta Meta { get; private set; }
            public ICanGetPocos PocoGetter { get; private set; }
            public IChangeTracker ChangeTracker { get; private set; }
            protected List<IProperty> CurrentlyInitialisingProperties { get; private set; }
            public List<IProperty> InitialisedProperties { get; private set; }

            public WritablePropertyInterceptor(IPocoMeta meta, ICanGetPocos pocoGetter, IChangeTracker changeTracker) {
                Meta = meta;
                PocoGetter = pocoGetter;
                ChangeTracker = changeTracker;

                CurrentlyInitialisingProperties = new List<IProperty>();
                InitialisedProperties = new List<IProperty>();
            }

            public void Intercept(IInvocation invocation) {
                if (invocation.Method.IsPropertyGetter())
                    GenericHelper.InvokeGeneric(() => InterceptGetter<object>(invocation),
                                                invocation.Method.GetPropertyInfo().PropertyType);

                if (invocation.Method.IsPropertySetter())
                    GenericHelper.InvokeGeneric(() => InterceptSetter<object>(invocation),
                                                invocation.Method.GetPropertyInfo().PropertyType);

                invocation.Proceed();
            }

            void InterceptGetter<P>(IInvocation invocation) {
                var property = new Property<T, P>(invocation.Method);

                if (!InitialisedProperties.Contains(property)) {
                    var value = Meta.Properties[property];
                    if (value is IPocoId)
                        value = PocoGetter.GetPoco((IPocoId) value);

                    CurrentlyInitialisingProperties.Add(property);
                    property.Set(invocation.InvocationTarget, value);
                    CurrentlyInitialisingProperties.Remove(property);
                    InitialisedProperties.Add(property);
                }
            }

            void InterceptSetter<P>(IInvocation invocation) {
                var property = new Property<T, P>(invocation.Method);

                if (CurrentlyInitialisingProperties.Contains(property))
                    return;

                var poco = invocation.InvocationTarget;
                var newValue = invocation.Arguments[0];
                var currentValue = property.Get(poco);

                if (newValue != currentValue)
                    ChangeTracker.TrackPropertySet(poco, property, newValue);
            }
        }
    }
}