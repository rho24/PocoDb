using System;

namespace PocoDb.Commits
{
    public class CommitId : ICommitId
    {
        public Guid Id { get; private set; }
        public DateTime Created { get; set; }

        public CommitId(Guid id, DateTime created) {
            Id = id;
            Created = created;
        }

        public int CompareTo(ICommitId other) {
            var otherId = other as CommitId;

            if (otherId == null)
                return 0;

            var createdCompare = Created.CompareTo(otherId.Created);

            return createdCompare != 0 ? createdCompare : Id.CompareTo(otherId.Id);
        }

        public override bool Equals(object other) {
            var otherId = other as CommitId;

            if (otherId == null)
                return false;

            return Id == otherId.Id;
        }

        public override int GetHashCode() {
            return Id.GetHashCode();
        }
    }
}