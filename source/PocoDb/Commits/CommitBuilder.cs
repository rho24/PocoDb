using System;
using PocoDb.ChangeTracking;
using PocoDb.Extensions;
using PocoDb.Meta;
using PocoDb.Session;

namespace PocoDb.Commits
{
    public class CommitBuilder : ICommitBuilder
    {
        public IInternalWritablePocoSession Session { get; private set; }
        protected ICommitIdGenerator IdGenerator { get; private set; }
        public IPocoMetaBuilder PocoMetaBuilder { get; private set; }

        public CommitBuilder(ICommitIdGenerator idGenerator, IPocoMetaBuilder pocoMetaBuilder) {
            IdGenerator = idGenerator;
            PocoMetaBuilder = pocoMetaBuilder;
        }

        public void Initialise(IInternalWritablePocoSession session) {
            Session = session;
        }

        public ICommit Build(ITrackedChanges trackedChanges) {
            var id = IdGenerator.New();

            var commit = new Commit(id);

            foreach (var addObjectChange in trackedChanges.AddedPocos) {
                RecordAddObject(addObjectChange.Poco, commit);
            }

            foreach (var propertySetChange in trackedChanges.SetProperties) {
                RecordPropertySet(propertySetChange, commit);
            }

            foreach (var addToCollectionChange in trackedChanges.CollectionAdditions) {
                RecordAddToCollection(addToCollectionChange, commit);
            }

            foreach (var removeFromCollectionChange in trackedChanges.CollectionRemovals) {
                RecordRemoveFromCollection(removeFromCollectionChange, commit);
            }

            return commit;
        }

        void RecordAddObject(object poco, Commit commit) {
            var metas = PocoMetaBuilder.Build(poco);

            foreach (var meta in metas) {
                commit.AddedPocos.Add(new AddedPoco(meta));
            }
        }

        void RecordPropertySet(TrackedSetProperty trackedSetProperty, Commit commit) {
            var pocoId = ResolveId(trackedSetProperty.Poco, commit);

            if (trackedSetProperty.Value.IsPocoType()) {
                var valueId = ResolveId(trackedSetProperty.Value, commit);
                commit.SetProperties.Add(new SetProperty(pocoId, trackedSetProperty.Property, valueId));
            }
            else
                commit.SetProperties.Add(new SetProperty(pocoId, trackedSetProperty.Property, trackedSetProperty.Value));
        }

        void RecordAddToCollection(TrackedCollectionAddition trackedCollectionAddition, Commit commit) {
            var collectionId = ResolveId(trackedCollectionAddition.Collection, commit);

            if (trackedCollectionAddition.Value.IsPocoType()) {
                var valueId = ResolveId(trackedCollectionAddition.Value, commit);
                commit.CollectionAdditions.Add(new CollectionAddition(collectionId, valueId));
                //TODO: Don't like value being used for IMetaId.
            }
            else
                commit.CollectionAdditions.Add(new CollectionAddition(collectionId,
                                                                      trackedCollectionAddition.Value));
        }

        void RecordRemoveFromCollection(TrackedCollectionRemoval trackedCollectionRemoval, Commit commit) {
            var collectionId = ResolveId(trackedCollectionRemoval.Collection, commit);

            if (trackedCollectionRemoval.Value.IsPocoType()) {
                var valueId = ResolveId(trackedCollectionRemoval.Value, commit);
                commit.CollectionRemovals.Add(new CollectionRemoval(collectionId, valueId));
                //TODO: Don't like value being used for IMetaId.
            }
            else
                commit.CollectionRemovals.Add(new CollectionRemoval(collectionId, trackedCollectionRemoval.Value));
        }

        IPocoId ResolveId(object poco, Commit commit) {
            if (Session.TrackedIds.ContainsKey(poco))
                return Session.TrackedIds[poco];

            RecordAddObject(poco, commit);

            return Session.TrackedIds[poco];
        }
    }
}