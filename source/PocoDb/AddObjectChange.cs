using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocoDb
{
    public class AddObjectChange
    {
        public object Object { get; private set; }

        public AddObjectChange(object o) {
            Object = o;
        }
    }
}