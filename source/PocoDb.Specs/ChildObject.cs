using System;
using System.Collections.Generic;

namespace PocoDb.Specs
{
    public class ChildObject
    {
        public virtual int Counter { get; set; }
        public virtual ICollection<string> Collection { get; set; }
    }
}