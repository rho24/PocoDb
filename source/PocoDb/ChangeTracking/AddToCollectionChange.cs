﻿using System;
using System.Collections;

namespace PocoDb.ChangeTracking
{
    public class AddToCollectionChange
    {
        public ICollection Collection { get; private set; }
        public object Value { get; private set; }

        public AddToCollectionChange(ICollection collection, object value) {
            Collection = collection;
            Value = value;
        }
    }
}