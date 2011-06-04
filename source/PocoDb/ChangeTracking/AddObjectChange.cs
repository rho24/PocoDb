using System;

namespace PocoDb.ChangeTracking
{
    public class AddObjectChange
    {
        public object Poco { get; private set; }

        public AddObjectChange(object poco) {
            Poco = poco;
        }
    }
}