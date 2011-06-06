using System;

namespace PocoDb.Pocos
{
    internal class ProxyException : Exception
    {
        public ProxyException(string message)
            : base(message) {}
    }
}