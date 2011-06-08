using System;
using PocoDb.Extensions;

namespace PocoDb.Linq
{
    public class IncorrectQueryResultType : Exception
    {
        public IncorrectQueryResultType(Type expected, Type was)
            : base("Expected '{0}', was '{1}'".Fmt(expected.Name, was.Name)) {}
    }
}