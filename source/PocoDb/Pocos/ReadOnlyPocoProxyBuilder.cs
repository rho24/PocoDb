using System;
using Castle.DynamicProxy;
using PocoDb.Extensions;
using PocoDb.Meta;

namespace PocoDb.Pocos
{
    public class ReadOnlyPocoProxyBuilder : IPocoProxyBuilder
    {
        public ICanGetPocos PocoGetter { get; private set; }
        public ProxyGenerator Generator { get; private set; }
        public ProxyGenerationOptions ProxyOptions { get; private set; }

        public ReadOnlyPocoProxyBuilder() {
            Generator = new ProxyGenerator();
            ProxyOptions = new ProxyGenerationOptions(new PropertyHook());
        }

        public void Initialise(ICanGetPocos pocoGetter) {
            PocoGetter = pocoGetter;
        }

        public object BuildProxy(IPocoMeta meta) {
            var propertyGetterInterceptor = GenericHelper.InvokeGeneric(
                () => new PocoProxyPropertyGetterInterceptor<object>(meta, PocoGetter), meta.Type) as IInterceptor;

            return Generator.CreateClassProxy(meta.Type, ProxyOptions, propertyGetterInterceptor);
        }
    }
}