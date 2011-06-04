﻿using System;
using PocoDb.Meta;

namespace PocoDb.Commits
{
    public class PropertySet
    {
        public IPocoId PocoId { get; private set; }
        public IProperty Property { get; private set; }
        public object Value { get; private set; }

        public PropertySet(IPocoId pocoId, IProperty property, object value) {
            PocoId = pocoId;
            Property = property;
            Value = value;
        }
    }
}