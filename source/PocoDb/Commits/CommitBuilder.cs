using System;
using PocoDb.ChangeTracking;
using PocoDb.Extensions;
using PocoDb.Meta;
using PocoDb.Pocos;

namespace PocoDb.Commits
{
    public class CommitBuilder : ICommitBuilder
    {
        protected ICommitIdGenerator IdGenerator { get; private set; }
        public IPocoMetaBuilder PocoMetaBuilder { get; private set; }

        public CommitBuilder(ICommitIdGenerator idGenerator, IPocoMetaBuilder pocoMetaBuilder) {
            IdGenerator = idGenerator;
            PocoMetaBuilder = pocoMetaBuilder;
        }

        public ICommit Build(ITrackedChanges trackedChanges, IIdsMetasAndProxies idsMetasAndProxies) {
            var id = IdGenerator.New();

            var commit = new Commit(id);

            foreach (var addObjectChange in trackedChanges.AddedPocos) {
                RecordAddObject(addObjectChange.Poco, commit, idsMetasAndProxies);
            }

            foreach (var propertySetChange in trackedChanges.SetProperties) {
                RecordPropertySet(propertySetChange, commit, idsMetasAndProxies);
            }

            foreach (var addToCollectionChange in trackedChanges.CollectionAdditions) {
                RecordAddToCollection(addToCollectionChange, commit, idsMetasAndProxies);
            }

            foreach (var removeFromCollectionChange in trackedChanges.CollectionRemovals) {
                RecordRemoveFromCollection(removeFromCollectionChange, commit, idsMetasAndProxies);
            }

            return commit;
        }

        void RecordAddObject(object poco, Commit commit, IIdsMetasAndProxies idsMetasAndProxies) {
            var metas = PocoMetaBuilder.Build(poco, idsMetasAndProxies);

            foreach (var meta in metas) {
                commit.AddedPocos.Add(new AddedPoco(meta));
            }
        }

        void RecordPropertySet(TrackedSetProperty trackedSetProperty, Commit commit,
                               IIdsMetasAndProxies idsMetasAndProxies) {
            var pocoId = ResolveId(trackedSetProperty.Poco, commit, idsMetasAndProxies);

            if (trackedSetProperty.Value.IsPocoType()) {
                var valueId = ResolveId(trackedSetProperty.Value, commit, idsMetasAndProxies);
                commit.UpdatedPocos.Add(Tuple.Create(pocoId, new SetProperty(trackedSetProperty.Property, valueId)));
            }
            else
                commit.UpdatedPocos.Add(Tuple.Create(pocoId,
                                                     new SetProperty(trackedSetProperty.Property,
                                                                     trackedSetProperty.Value)));
        }

        void RecordAddToCollection(TrackedCollectionAddition trackedCollectionAddition, Commit commit,
                                   IIdsMetasAndProxies idsMetasAndProxies) {
            var collectionId = ResolveId(trackedCollectionAddition.Collection, commit, idsMetasAndProxies);

            if (trackedCollectionAddition.Value.IsPocoType()) {
                var valueId = ResolveId(trackedCollectionAddition.Value, commit, idsMetasAndProxies);
                commit.CollectionAdditions.Add(new CollectionAddition(collectionId, valueId));
                //TODO: Don't like value being used for IMetaId.
            }
            else
                commit.CollectionAdditions.Add(new CollectionAddition(collectionId,
                                                                      trackedCollectionAddition.Value));
        }

        void RecordRemoveFromCollection(TrackedCollectionRemoval trackedCollectionRemoval, Commit commit,
                                        IIdsMetasAndProxies idsMetasAndProxies) {
            var collectionId = ResolveId(trackedCollectionRemoval.Collection, commit, idsMetasAndProxies);

            if (trackedCollectionRemoval.Value.IsPocoType()) {
                var valueId = ResolveId(trackedCollectionRemoval.Value, commit, idsMetasAndProxies);
                commit.CollectionRemovals.Add(new CollectionRemoval(collectionId, valueId));
                //TODO: Don't like value being used for IMetaId.
            }
            else
                commit.CollectionRemovals.Add(new CollectionRemoval(collectionId, trackedCollectionRemoval.Value));
        }

        IPocoId ResolveId(object poco, Commit commit, IIdsMetasAndProxies idsMetasAndProxies) {
            if (idsMetasAndProxies.Ids.ContainsKey(poco))
                return idsMetasAndProxies.Ids[poco];

            RecordAddObject(poco, commit, idsMetasAndProxies);

            return idsMetasAndProxies.Ids[poco];
        }
    }
}