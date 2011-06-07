using System;

namespace PocoDb.Meta
{
    public class PocoIdBuilder : IPocoIdBuilder
    {
        public IPocoId New() {
            return new PocoId();
        }
    }

    public class PocoId : IPocoId
    {
        public Guid Id { get; private set; }

        public PocoId() {
            Id = Guid.NewGuid();
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