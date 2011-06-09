using System;

namespace PocoDb.Pocos.Proxies
{
    internal class ProxyException : Exception
    {
        public ProxyException(string message)
            : base(message) {}
    }
}