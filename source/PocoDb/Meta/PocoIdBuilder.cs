using System;

namespace PocoDb.Meta
{
    public class PocoIdBuilder : IPocoIdBuilder
    {
        public IPocoId New() {
            return new PocoId();
        }
    }
}