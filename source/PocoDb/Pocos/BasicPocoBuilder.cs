using System;
using System.Linq;
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
            ProxyOptions = new ProxyGenerationOptions(new PocoPropertyHook());
        }

        public void Initialise(IInternalPocoSession session) {
            Session = session;
        }

        public object Build(IPocoMeta meta) {
            ProxyOptions.AddMixinInstance(new PocoProxy(meta));

            var pocoPropertyInterceptor = new PocoPropertyInterceptor(meta, Session);

            var proxy = Generator.CreateClassProxy(meta.Type, ProxyOptions, pocoPropertyInterceptor);

            foreach (var propertyAndValue in meta.Properties.Where(p => !p.Value.IsPocoType())) {
                propertyAndValue.Key.Set(proxy, propertyAndValue.Value);
            }

            return proxy;
        }

        class PocoProxy
        {
            public IPocoMeta Meta { get; private set; }

            public PocoProxy(IPocoMeta meta) {
                Meta = meta;
            }
        }

        class PocoPropertyInterceptor : IInterceptor
        {
            public PocoPropertyInterceptor(IPocoMeta meta, IInternalPocoSession session) {}

            public void Intercept(IInvocation invocation) {}
        }

        class PocoPropertyHook : IProxyGenerationHook
        {
            public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo) {
                return methodInfo.Name.StartsWith("get_", StringComparison.Ordinal) &&
                       methodInfo.ReturnType.IsPocoType();
            }

            public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo) {}

            public void MethodsInspected() {}
        }
    }
}