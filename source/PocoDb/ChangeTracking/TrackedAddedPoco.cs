using System;

namespace PocoDb.ChangeTracking
{
    public class TrackedAddedPoco : ITrackedChange
    {
        public object Poco { get; private set; }

        public TrackedAddedPoco(object poco) {
            Poco = poco;
        }
    }
}