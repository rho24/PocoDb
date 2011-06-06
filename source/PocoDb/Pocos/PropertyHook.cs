using System;
using System.Reflection;
using Castle.DynamicProxy;
using PocoDb.Extensions;

namespace PocoDb.Pocos
{
    public class PropertyHook : IProxyGenerationHook
    {
        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo) {
            return methodInfo.IsProperty();
        }

        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo) {}

        public void MethodsInspected() {}
    }
}