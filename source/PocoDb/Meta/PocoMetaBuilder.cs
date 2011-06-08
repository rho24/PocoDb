using System;
using System.Collections.Generic;
using PocoDb.Extensions;
using PocoDb.Pocos;

namespace PocoDb.Meta
{
    public class PocoMetaBuilder : IPocoMetaBuilder
    {
        public IPocoIdBuilder IdBuilder { get; private set; }

        public PocoMetaBuilder(IPocoIdBuilder idBuilder) {
            IdBuilder = idBuilder;
        }

        public IEnumerable<IPocoMeta> Build(object poco, IIdsMetasAndProxies idsMetasAndProxies) {
            if (poco.ImplementsCollectionType())
                return
                    (IEnumerable<IPocoMeta>)
                    GenericHelper.InvokeGeneric(() => BuildCollectionMeta<object>(poco, idsMetasAndProxies),
                                                poco.GetCollectionInnerType());

            return BuildPocoMeta(poco, idsMetasAndProxies);
        }

        IEnumerable<IPocoMeta> BuildPocoMeta(object poco, IIdsMetasAndProxies idsMetasAndProxies) {
            var newMetas = new List<IPocoMeta>();

            var id = IdBuilder.New();
            var meta = new PocoMeta(id, poco.GetType());

            newMetas.Add(meta);
            idsMetasAndProxies.Ids.Add(poco, id);

            foreach (var property in meta.Type.PublicVirtualProperties()) {
                var value = property.Get(poco);

                if (value.IsPocoType()) {
                    if (!idsMetasAndProxies.Ids.ContainsKey(value))
                        newMetas.AddRange(Build(value, idsMetasAndProxies));

                    var childId = idsMetasAndProxies.Ids[value];
                    meta.Properties.Add(property, childId);
                }
                else
                    meta.Properties.Add(property, value);
            }

            return newMetas;
        }

        IEnumerable<IPocoMeta> BuildCollectionMeta<T>(object poco, IIdsMetasAndProxies idsMetasAndProxies) {
            var collection = poco as ICollection<T>;
            if (collection == null)
                throw new ArgumentException("poco doesn't implement ICollection<>");

            var newMetas = new List<IPocoMeta>();

            var id = IdBuilder.New();
            var meta = new PocoMeta(id, typeof (ICollection<T>));

            newMetas.Add(meta);
            idsMetasAndProxies.Ids.Add(collection, id);

            foreach (var value in collection) {
                if (value.IsPocoType()) {
                    if (!idsMetasAndProxies.Ids.ContainsKey(value))
                        newMetas.AddRange(Build(value, idsMetasAndProxies));

                    var childId = idsMetasAndProxies.Ids[value];
                    meta.Collection.Add(childId);
                }
                else
                    meta.Collection.Add(value);
            }

            return newMetas;
        }
    }
}