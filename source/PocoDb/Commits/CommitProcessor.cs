using System;
using PocoDb.Persistence;

namespace PocoDb.Commits
{
    public class CommitProcessor : ICommitProcessor
    {
        public IMetaStore MetaStore { get; private set; }

        public CommitProcessor(IMetaStore metaStore) {
            MetaStore = metaStore;
        }

        public void Apply(ICommit commit) {
            foreach (var addObject in commit.AddedPocos) {
                MetaStore.AddNew(addObject.Meta);
            }

            foreach (var propertySet in commit.SetProperties) {
                var meta = MetaStore.GetWritable(propertySet.PocoId);
                meta.Properties[propertySet.Property] = propertySet.Value;

                MetaStore.Update(meta);
            }

            foreach (var addToCollection in commit.CollectionAdditions) {
                var meta = MetaStore.GetWritable(addToCollection.CollectionId);
                meta.Collection.Add(addToCollection.Value);

                MetaStore.Update(meta);
            }

            foreach (var removeFromCollection in commit.CollectionRemovals) {
                var meta = MetaStore.GetWritable(removeFromCollection.CollectionId);
                meta.Collection.Remove(removeFromCollection.Value);

                MetaStore.Update(meta);
            }
        }
    }
}