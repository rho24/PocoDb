using System;
using System.Collections.Generic;
using PocoDb.Session;

namespace PocoDb.Meta
{
    public class PocoMetaBuilder : IPocoMetaBuilder
    {
        public IInternalWritablePocoSession Session { get; private set; }
        public IPocoIdBuilder IdBuilder { get; private set; }

        public PocoMetaBuilder(IPocoIdBuilder idBuilder) {
            IdBuilder = idBuilder;
        }

        public void Initialise(IInternalWritablePocoSession session) {
            Session = session;
        }

        public IEnumerable<IPocoMeta> Build(object poco) {
            var id = IdBuilder.New();

            Session.TrackedIds.Add(poco, id);

            var newMetas = new List<IPocoMeta> {null};

            return newMetas;
        }
    }
}