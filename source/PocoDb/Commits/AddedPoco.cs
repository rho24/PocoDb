using System;
using PocoDb.Meta;

namespace PocoDb.Commits
{
    public class AddedPoco
    {
        public IPocoMeta Meta { get; private set; }

        public AddedPoco(IPocoMeta meta) {
            Meta = meta;
        }
    }
}