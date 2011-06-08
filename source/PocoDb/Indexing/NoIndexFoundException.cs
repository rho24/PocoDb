using System;

namespace PocoDb.Indexing
{
    public class NoIndexFoundException : Exception
    {
        public NoIndexFoundException(string message) : base(message) {}
    }
}