using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocoDb
{
    public class AddToCollectionChange
    {
        public object Collection { get; private set; }
        public object Object { get; private set; }

        public AddToCollectionChange(object collection, object o) {
            Collection = collection;
            Object = o;
        }
    }
}