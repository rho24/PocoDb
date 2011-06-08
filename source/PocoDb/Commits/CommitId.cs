﻿using System;

namespace PocoDb.Commits
{
    public class CommitId : ICommitId
    {
        public Guid Id { get; private set; }

        public CommitId() {
            Id = Guid.NewGuid();
        }

        public override bool Equals(object obj) {
            var otherId = obj as CommitId;

            if (otherId == null)
                return false;

            return Id == otherId.Id;
        }

        public override int GetHashCode() {
            return Id.GetHashCode();
        }
    }
}