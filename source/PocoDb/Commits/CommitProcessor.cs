using System;
using PocoDb.Meta;

namespace PocoDb.Commits
{
    public class CommitProcessor : ICommitProcessor
    {
        public IMetaStore MetaStore { get; private set; }

        public CommitProcessor(IMetaStore metaStore) {
            MetaStore = metaStore;
        }

        public void Apply(ICommit commit) {
            foreach (var addObject in commit.AddObjects) {
                MetaStore.Save(addObject.Meta);
            }

            foreach (var propertySet in commit.PropertySets) {
                var meta = MetaStore.Get(propertySet.PocoId);
                meta.Properties[propertySet.Property] = propertySet.Value;

                MetaStore.Save(meta);
            }

            foreach (var addToCollection in commit.AddToCollections) {
                var meta = MetaStore.Get(addToCollection.CollectionId);
                meta.Collection.Add(addToCollection.Value);

                MetaStore.Save(meta);
            }

            foreach (var removeFromCollection in commit.RemoveFromCollections) {
                var meta = MetaStore.Get(removeFromCollection.CollectionId);
                meta.Collection.Remove(removeFromCollection.Value);

                MetaStore.Save(meta);
            }
        }
    }
}