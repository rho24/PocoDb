using System;
using PocoDb.ChangeTracking;
using PocoDb.Extensions;
using PocoDb.Meta;

namespace PocoDb.Commits
{
    public class CommitBuilder : ICommitBuilder
    {
        public IPocoMetaBuilder PocoMetaBuilder { get; private set; }

        public CommitBuilder(IPocoMetaBuilder pocoMetaBuilder) {
            PocoMetaBuilder = pocoMetaBuilder;
        }

        public ICommit Build(ITrackedChanges changes) {
            var commit = new Commit();

            foreach (var addObjectChange in changes.AddObjectChanges) {
                AddNewPoco(addObjectChange.Object, commit);
            }

            foreach (var propertySetChange in changes.PropertySetChanges) {
                AddPropertySet(propertySetChange, commit);
            }

            foreach (var addToCollectionChange in changes.AddToCollectionChanges) {
                AddAddToCollection(addToCollectionChange, commit);
            }

            foreach (var removeFromCollectionChange in changes.RemoveFromCollectionChanges) {
                AddRemoveFromCollection(removeFromCollectionChange, commit);
            }

            return commit;
        }

        IPocoId AddNewPoco(object poco, Commit commit) {
            var meta = PocoMetaBuilder.Build(poco);
            commit.AddedMetas.Add(meta);

            return meta.Id;
        }

        void AddPropertySet(PropertySetChange propertySetChange, Commit commit) {
            var id = ResolveId(propertySetChange.Object, commit);

            if (propertySetChange.Value.IsPocoType()) {
                var valueId = ResolveId(propertySetChange.Value, commit);
                commit.PropertySets.Add(new PropertySet(id, propertySetChange.Property, valueId));
            }
            else
                commit.PropertySets.Add(new PropertySet(id, propertySetChange.Property, propertySetChange.Value));
        }

        void AddAddToCollection(AddToCollectionChange addToCollectionChange, Commit commit) {
            var id = ResolveId(addToCollectionChange.Collection, commit);

            if (addToCollectionChange.Value.IsPocoType()) {
                var valueId = ResolveId(addToCollectionChange.Value, commit);
                commit.AddToCollections.Add(new AddToCollection(addToCollectionChange.Collection, valueId));
            }
            else
                commit.AddToCollections.Add(new AddToCollection(addToCollectionChange.Collection,
                                                                addToCollectionChange.Value));
        }

        void AddRemoveFromCollection(RemoveFromCollectionChange removeFromCollectionChange, Commit commit) {
            var id = ResolveId(removeFromCollectionChange.Collection, commit);

            if (removeFromCollectionChange.Value.IsPocoType()) {
                var valueId = ResolveId(removeFromCollectionChange.Value, commit);
                commit.RemoveFromCollections.Add(new RemoveFromCollection(removeFromCollectionChange.Collection, valueId));
            }
            else
                commit.RemoveFromCollections.Add(new RemoveFromCollection(removeFromCollectionChange.Collection,
                                                                          removeFromCollectionChange.Value));
        }

        IPocoId ResolveId(object poco, Commit commit) {
            var id = PocoMetaBuilder.Resolve(poco);

            return id ?? AddNewPoco(poco, commit);
        }
    }
}