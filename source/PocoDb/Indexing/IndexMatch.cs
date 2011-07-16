using System;

namespace PocoDb.Indexing
{
    public class IndexMatch
    {
        public IIndex Index { get; private set; }
        public bool IsExact { get; private set; }
        public bool IsPartial { get; private set; }
        public int PartialDepth { get; private set; }

        IndexMatch(IIndex index, bool isExact, bool isPartial, int partialDepth) {
            Index = index;
            IsExact = isExact;
            IsPartial = isPartial;
            PartialDepth = partialDepth;
        }

        public static IndexMatch ExactMatch(IIndex index) {
            return new IndexMatch(index, true, false, 0);
        }

        public static IndexMatch PartialMatch(IIndex index, int depth) {
            return new IndexMatch(index, false, true, depth);
        }

        public static IndexMatch NoMatch(IIndex index) {
            return new IndexMatch(index, false, false, 0);
        }

        public static IndexMatch NoMatch() {
            return new IndexMatch(null, false, false, 0);
        }
    }
}