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
                MetaStore.Add(addObject.Meta);
            }
        }
    }
}