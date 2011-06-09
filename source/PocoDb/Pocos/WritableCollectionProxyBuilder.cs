using System;
using System.Collections;
using System.Collections.Generic;
using PocoDb.ChangeTracking;
using PocoDb.Extensions;
using PocoDb.Meta;

namespace PocoDb.Pocos
{
    public class WritableCollectionProxyBuilder : ICollectionProxyBuilder
    {
        public ICanGetPocos PocoGetter { get; private set; }
        public IChangeTracker ChangeTracker { get; private set; }

        public void Initialise(ICanGetPocos pocoGetter, IChangeTracker changeTracker) {
            PocoGetter = pocoGetter;
            ChangeTracker = changeTracker;
        }

        public object BuildProxy(IPocoMeta meta) {
            if (!meta.Type.IsCollectionType())
                throw new ArgumentException("meta is not an Collection");

            var innerType = meta.Type.GetGenericArguments()[0];

            return
                GenericHelper.InvokeGeneric(() => new WritableCollectionProxy<object>(meta, PocoGetter, ChangeTracker),
                                            innerType);
        }

        class WritableCollectionProxy<T> : ICollection<T>
        {
            public IPocoMeta Meta { get; private set; }
            public ICanGetPocos PocoGetter { get; private set; }
            public IChangeTracker ChangeTracker { get; private set; }
            public ICollection<T> InnerCollection { get; private set; }

            public WritableCollectionProxy(IPocoMeta meta, ICanGetPocos pocoGetter, IChangeTracker changeTracker) {
                Meta = meta;
                PocoGetter = pocoGetter;
                ChangeTracker = changeTracker;

                Initialise();
            }

            void Initialise() {
                InnerCollection = new List<T>();
                foreach (var o in Meta.Collection) {
                    var value = o is IPocoId ? PocoGetter.GetPoco((IPocoId) o) : o;

                    if (value is T)
                        InnerCollection.Add((T) value);
                    else
                        throw new InvalidOperationException("value is not a '{0}'".Fmt(typeof (T).Name));
                }
            }

            public bool IsReadOnly { get { return false; } }

            public void Add(T item) {
                ChangeTracker.TrackAddToCollection(this, item);
                InnerCollection.Add(item);
            }

            public void Clear() {
                foreach (var value in InnerCollection) {
                    ChangeTracker.TrackRemoveFromCollection(this, value);
                }

                InnerCollection.Clear();
            }

            public bool Contains(T item) {
                return InnerCollection.Contains(item);
            }

            public void CopyTo(T[] array, int arrayIndex) {
                InnerCollection.CopyTo(array, arrayIndex);
            }

            public bool Remove(T item) {
                ChangeTracker.TrackRemoveFromCollection(this, item);
                return InnerCollection.Remove(item);
            }

            public int Count { get { return InnerCollection.Count; } }

            public IEnumerator<T> GetEnumerator() {
                return InnerCollection.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }
        }
    }
}