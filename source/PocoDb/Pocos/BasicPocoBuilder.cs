using System;
using System.Collections.Generic;
using System.Reflection;
using Castle.DynamicProxy;
using PocoDb.Extensions;
using PocoDb.Meta;
using PocoDb.Session;

namespace PocoDb.Pocos
{
    public class BasicPocoBuilder : IPocoBuilder
    {
        public IInternalPocoSession Session { get; private set; }
        protected ProxyGenerator Generator { get; private set; }
        public ProxyGenerationOptions ProxyOptions { get; private set; }

        public BasicPocoBuilder() {
            Generator = new ProxyGenerator();
            ProxyOptions = new ProxyGenerationOptions(new PropertyGetHook());
        }

        public void Initialise(IInternalPocoSession session) {
            Session = session;
        }

        public object Build(IPocoMeta meta) {
            ProxyOptions.AddMixinInstance(new PocoProxy(meta));

            var pocoPropertyInterceptor =
                LambdaExtensions.InvokeGeneric(() => new PocoPropertyInterceptor<object>(meta, Session), meta.Type) as
                IInterceptor;

            var proxy = Generator.CreateClassProxy(meta.Type, ProxyOptions, pocoPropertyInterceptor);

            return proxy;
        }

        class PocoProxy
        {
            public IPocoMeta Meta { get; private set; }

            public PocoProxy(IPocoMeta meta) {
                Meta = meta;
            }
        }

        class PocoPropertyInterceptor<T> : IInterceptor
        {
            public IPocoMeta Meta { get; private set; }
            public IInternalPocoSession Session { get; private set; }
            public List<IProperty> InitialisedProperties { get; private set; }

            public PocoPropertyInterceptor(IPocoMeta meta, IInternalPocoSession session) {
                Meta = meta;
                Session = session;

                InitialisedProperties = new List<IProperty>();
            }

            public void Intercept(IInvocation invocation) {
                LambdaExtensions.InvokeGeneric(() => Intercept<object>(invocation), invocation.Method.ReturnType);
            }

            void Intercept<P>(IInvocation invocation) {
                var property = new Property<T, P>(invocation.Method);

                if (!InitialisedProperties.Contains(property)) {
                    var value = Meta.Properties[property];
                    if (value is IPocoId)
                        value = Session.GetPoco((IPocoId) value);

                    property.Set(invocation.InvocationTarget, value);
                    InitialisedProperties.Add(property);
                }

                invocation.Proceed();
            }
        }

        class PropertyGetHook : IProxyGenerationHook
        {
            public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo) {
                return methodInfo.Name.StartsWith("get_", StringComparison.Ordinal);
            }

            public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo) {}

            public void MethodsInspected() {}
        }
    }
}