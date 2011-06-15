using System;

namespace PocoDb.Meta
{
    public class PocoIdBuilder : IPocoIdBuilder
    {
        public IPocoId New() {
            var id = Guid.NewGuid();
            return new PocoId(id);
        }
    }
}