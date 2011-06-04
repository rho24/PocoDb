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
                var childId = ResolveId(propertySetChange.Value, commit);
                commit.PropertySets.Add(new PropertySet(id, propertySetChange.Property, childId));
            }
            else
                commit.PropertySets.Add(new PropertySet(id, propertySetChange.Property, propertySetChange.Value));
        }

        IPocoId ResolveId(object poco, Commit commit) {
            var id = PocoMetaBuilder.Resolve(poco);

            return id ?? AddNewPoco(poco, commit);
        }
    }
}