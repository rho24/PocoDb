using System;
using PocoDb.ChangeTracking;
using PocoDb.Extensions;
using PocoDb.Meta;

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

        public ICommit Build(ITrackedChanges changes) {
            var id = IdGenerator.New();

            var commit = new Commit(id);

            foreach (var addObjectChange in changes.AddObjectChanges) {
                RecordAddObject(addObjectChange.Poco, commit);
            }

            foreach (var propertySetChange in changes.PropertySetChanges) {
                RecordPropertySet(propertySetChange, commit);
            }

            foreach (var addToCollectionChange in changes.AddToCollectionChanges) {
                RecordAddToCollection(addToCollectionChange, commit);
            }

            foreach (var removeFromCollectionChange in changes.RemoveFromCollectionChanges) {
                RecordRemoveFromCollection(removeFromCollectionChange, commit);
            }

            return commit;
        }

        IPocoId RecordAddObject(object poco, Commit commit) {
            var meta = PocoMetaBuilder.Build(poco);
            commit.AddObjects.Add(new AddObject(meta));

            return meta.Id;
        }

        void RecordPropertySet(PropertySetChange propertySetChange, Commit commit) {
            var pocoId = ResolveId(propertySetChange.Poco, commit);

            if (propertySetChange.Value.IsPocoType()) {
                var valueId = ResolveId(propertySetChange.Value, commit);
                commit.PropertySets.Add(new PropertySet(pocoId, propertySetChange.Property, valueId));
            }
            else
                commit.PropertySets.Add(new PropertySet(pocoId, propertySetChange.Property, propertySetChange.Value));
        }

        void RecordAddToCollection(AddToCollectionChange addToCollectionChange, Commit commit) {
            var collectionId = ResolveId(addToCollectionChange.Collection, commit);

            if (addToCollectionChange.Value.IsPocoType()) {
                var valueId = ResolveId(addToCollectionChange.Value, commit);
                commit.AddToCollections.Add(new AddToCollection(collectionId, valueId));
                    //TODO: Don't like value being used for IMetaId.
            }
            else
                commit.AddToCollections.Add(new AddToCollection(collectionId,
                                                                addToCollectionChange.Value));
        }

        void RecordRemoveFromCollection(RemoveFromCollectionChange removeFromCollectionChange, Commit commit) {
            var collectionId = ResolveId(removeFromCollectionChange.Collection, commit);

            if (removeFromCollectionChange.Value.IsPocoType()) {
                var valueId = ResolveId(removeFromCollectionChange.Value, commit);
                commit.RemoveFromCollections.Add(new RemoveFromCollection(collectionId, valueId));
                    //TODO: Don't like value being used for IMetaId.
            }
            else
                commit.RemoveFromCollections.Add(new RemoveFromCollection(collectionId, removeFromCollectionChange.Value));
        }

        IPocoId ResolveId(object poco, Commit commit) {
            var id = PocoMetaBuilder.Resolve(poco);

            return id ?? RecordAddObject(poco, commit);
        }
    }
}