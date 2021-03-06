﻿using System;
using System.Collections.Generic;
using System.Linq;
using PocoDb.Linq;
using PocoDb.Meta;
using PocoDb.Pocos;
using PocoDb.Server;

namespace PocoDb.Session
{
    public class PocoSession : IPocoSession, IInternalPocoSession
    {
        public IPocoDbServer Server { get; private set; }
        public IPocoFactory PocoFactory { get; private set; }
        public IExpressionProcessor ExpressionProcessor { get; private set; }
        public IIdsMetasAndProxies IdsMetasAndProxies { get; private set; }


        public PocoSession(IPocoDbServer server, IPocoFactory pocoFactory, IExpressionProcessor expressionProcessor) {
            Server = server;
            PocoFactory = pocoFactory;
            ExpressionProcessor = expressionProcessor;

            IdsMetasAndProxies = new IdsMetasAndProxies();
        }

        //IPocoSession

        public IQueryable<T> Get<T>() {
            return new PocoQueryable<T>(new PocoQueryProvider(new PocoQueryableExecutor(this, ExpressionProcessor)));
        }

        //IInternalPocoSession
        public object GetPoco(IPocoId id) {
            if (IdsMetasAndProxies.Metas.ContainsKey(id))
                return PocoFactory.Build(IdsMetasAndProxies.Metas[id], IdsMetasAndProxies);
            else {
                var meta = Server.GetMeta(id);

                if (meta == null)
                    throw new ArgumentException("id is not recognised");

                IdsMetasAndProxies.Metas.Add(meta.Id, meta);
                return PocoFactory.Build(meta, IdsMetasAndProxies);
            }
        }

        public IEnumerable<object> GetPocos(IEnumerable<IPocoId> ids) {
            foreach (var id in ids) {
                if (IdsMetasAndProxies.Metas.ContainsKey(id))
                    yield return PocoFactory.Build(IdsMetasAndProxies.Metas[id], IdsMetasAndProxies);
                else {
                    var meta = Server.GetMeta(id);

                    if (meta == null)
                        throw new ArgumentException("id is not recognised");

                    IdsMetasAndProxies.Metas.Add(meta.Id, meta);
                    yield return PocoFactory.Build(meta, IdsMetasAndProxies);
                }
            }
        }

        public void Dispose() {}
    }
}