using System;

namespace PocoDb.Specs
{
    public class DummyObject
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual ChildObject Child { get; set; }
    }
}