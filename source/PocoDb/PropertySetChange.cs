using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocoDb
{
    public class PropertySetChange
    {
        public object Object { get; private set; }
        public Property Property { get; private set; }
        public object value { get; set; }
    }
}
