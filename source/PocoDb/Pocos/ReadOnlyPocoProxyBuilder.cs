﻿using System;
using Castle.DynamicProxy;
using PocoDb.Extensions;
using PocoDb.Meta;
using PocoDb.Session;

namespace PocoDb.Pocos
{
    public class ReadOnlyPocoProxyBuilder : IPocoProxyBuilder
    {
        public IInternalPocoSession Session { get; private set; }
        public ProxyGenerator Generator { get; private set; }
        public ProxyGenerationOptions ProxyOptions { get; private set; }

        public ReadOnlyPocoProxyBuilder() {
            Generator = new ProxyGenerator();
            ProxyOptions = new ProxyGenerationOptions(new PropertyHook());
        }

        public void Initialise(IInternalPocoSession session) {
            Session = session;
        }

        public object BuildProxy(IPocoMeta meta) {
            var propertyGetterInterceptor = GenericHelper.InvokeGeneric(
                () => new PocoProxyPropertyGetterInterceptor<object>(meta, Session), meta.Type) as IInterceptor;

            return Generator.CreateClassProxy(meta.Type, ProxyOptions, propertyGetterInterceptor);
        }
    }
}