using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using PocoDb.Extensions;

namespace PocoDb.Indexing
{
    public class IndexManager : IIndexManager
    {
        public IDictionary<Type, IIndex> TypeIndexes { get; private set; }

        public IndexManager() {
            TypeIndexes = new Dictionary<Type, IIndex>();
        }

        public IIndex RetrieveIndex(Expression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            if (expression is ConstantExpression) {
                var constantExpression = (ConstantExpression) expression;

                if (constantExpression.Type.GetGenericTypeDefinition() != typeof (PocoDb.Linq.PocoQueryable<>))
                    throw new ArgumentException("expression is not based on PocoQueryable<>");

                var innerType = constantExpression.Type.GetGenericArguments()[0];

                if (!TypeIndexes.ContainsKey(innerType))
                    throw new NoIndexFoundException("Could not find TypeIndex for '{0}'".Fmt(innerType));

                return TypeIndexes[innerType];
            }

            throw new NotImplementedException();
        }
    }
}