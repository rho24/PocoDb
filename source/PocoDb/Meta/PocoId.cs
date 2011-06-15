using System;

namespace PocoDb.Meta
{
    public class PocoId : IPocoId
    {
        public Guid Id { get; private set; }

        public PocoId(Guid id) {
            Id = id;
        }

        public override bool Equals(object obj) {
            var otherId = obj as PocoId;

            if (otherId == null)
                return false;

            return Id == otherId.Id;
        }

        public override int GetHashCode() {
            return Id.GetHashCode();
        }
    }
}