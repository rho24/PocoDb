using System;
using PocoDb.Extensions;

namespace PocoDb.Indexing
{
    public class NoIndexFoundException : Exception
    {
        public NoIndexFoundException(Type type) : base("No index found for '{0}'".Fmt(type.FullName)) {}
    }
}