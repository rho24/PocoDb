using System;
using System.Collections.Generic;
using PocoDb.Extensions;
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
            var newMetas = new List<IPocoMeta>();

            var id = IdBuilder.New();
            Session.TrackedIds.Add(poco, id);

            var meta = new PocoMeta(id, poco.GetType());

            foreach (var property in meta.Type.PublicVirtualProperties()) {
                var value = property.Get(poco);

                if (value.IsPocoType())
                    throw new NotImplementedException();

                meta.Properties.Add(property, value);
            }

            newMetas.Add(meta);

            return newMetas;
        }
    }

    public class PocoMeta : IPocoMeta
    {
        public IPocoId Id { get; private set; }
        public IDictionary<IProperty, object> Properties { get; private set; }
        public ICollection<object> Collection { get; private set; }
        public Type Type { get; private set; }

        public PocoMeta(IPocoId id, Type type) {
            Id = id;
            Type = type;
            Properties = new Dictionary<IProperty, object>();
            Collection = new List<object>();
        }
    }
}