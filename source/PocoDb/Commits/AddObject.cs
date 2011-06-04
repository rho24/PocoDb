using System;
using PocoDb.Meta;

namespace PocoDb.Commits
{
    public class AddObject
    {
        public IPocoMeta Meta { get; private set; }

        public AddObject(IPocoMeta meta) {
            Meta = meta;
        }
    }
}