using System;
using PocoDb.Server;

namespace PocoDb.Commits
{
    public class CommitProcessor : ICommitProcessor
    {
        public IPocoDbServer Server { get; private set; }

        public void Initialise(IPocoDbServer server) {
            Server = server;
        }

        public void Apply(ICommit commit) {
            foreach (var addObject in commit.AddedPocos) {
                Server.MetaStore.AddNew(addObject.Meta);
                Server.IndexManager.NotifyMetaChange(addObject.Meta);
            }

            foreach (var propertySet in commit.UpdatedPocos) {
                var meta = Server.MetaStore.GetWritable(propertySet.Key);
                foreach (var setProperty in propertySet) {
                    meta.Properties[setProperty.Property] = setProperty.Value;
                }

                Server.MetaStore.Update(meta);
            }

            foreach (var addToCollection in commit.CollectionAdditions) {
                var meta = Server.MetaStore.GetWritable(addToCollection.CollectionId);
                meta.Collection.Add(addToCollection.Value);

                Server.MetaStore.Update(meta);
            }

            foreach (var removeFromCollection in commit.CollectionRemovals) {
                var meta = Server.MetaStore.GetWritable(removeFromCollection.CollectionId);
                meta.Collection.Remove(removeFromCollection.Value);

                Server.MetaStore.Update(meta);
            }
        }
    }
}