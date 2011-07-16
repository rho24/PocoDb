using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using PocoDb.Extensions;
using PocoDb.Queries;
using PocoDb.Session;

namespace PocoDb.Linq
{
    public class PocoQueryableExecutor : IPocoQueryableExecutor
    {
        public IInternalPocoSession Session { get; private set; }
        protected IExpressionProcessor ExpressionProcessor { get; private set; }

        public PocoQueryableExecutor(IInternalPocoSession session, IExpressionProcessor expressionProcessor) {
            Session = session;
            ExpressionProcessor = expressionProcessor;
        }

        public T Execute<T>(Expression expression) {
            expression = ExpressionProcessor.Process(expression);

            var returnType = typeof (T);
            if (expression.IsSingleQuery())
                return (T) GenericHelper.InvokeGeneric(() => ExecuteSingle<object>(expression), returnType);

            if (expression.IsElementQuery())
                return (T) GenericHelper.InvokeGeneric(() => ExecuteElement<object>(expression), returnType);

            if (returnType.IsEnumerable())
                return (T) GenericHelper.InvokeGeneric(() => ExecuteEnumerable<object>(expression),
                                                       returnType.EnumerableInnerType());

            throw new NotSupportedException("Unknown expression type");
        }

        T ExecuteSingle<T>(Expression expression) {
            var result = Session.Server.QuerySingle(new Query(expression));

            if (result.ElementId == null) {
                if (expression.IsOrDefaultQuery())
                    return default(T);

                throw new InvalidOperationException("Source has no elements");
            }

            if (result.HasMany)
                throw new InvalidOperationException("Source has more than one element");

            ImportMetas(result);
            return (T) Session.GetPoco(result.ElementId);
        }

        T ExecuteElement<T>(Expression expression) {
            var result = Session.Server.QueryElement(new Query(expression));

            if (result.ElementId == null) {
                if (expression.IsOrDefaultQuery())
                    return default(T);

                throw new InvalidOperationException("Source has no elements");
            }

            ImportMetas(result);
            return (T) Session.GetPoco(result.ElementId);
        }

        IEnumerable<T> ExecuteEnumerable<T>(Expression expression) {
            var result = Session.Server.QueryEnumerable(new Query(expression));

            ImportMetas(result);
            return result.ElementIds.Select(id => (T) Session.GetPoco(id));
        }

        void ImportMetas(IQueryResult result) {
            foreach (var meta in result.Metas) {
                Session.IdsMetasAndProxies.Metas.Add(meta.Id, meta);
            }
        }
    }
}