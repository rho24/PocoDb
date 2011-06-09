using System;
using System.Collections;
using System.Collections.Generic;
using PocoDb.Extensions;
using PocoDb.Meta;
using PocoDb.Session;

namespace PocoDb.Pocos
{
    public class ReadOnlyCollectionProxyBuilder : ICollectionProxyBuilder
    {
        public ICanGetPocos PocoGetter { get; private set; }

        public void Initialise(ICanGetPocos pocoGetter) {
            PocoGetter = pocoGetter;
        }

        public object BuildProxy(IPocoMeta meta) {
            if (!meta.Type.IsCollectionType())
                throw new ArgumentException("meta is not an Collection");

            var innerType = meta.Type.GetGenericArguments()[0];

            return GenericHelper.InvokeGeneric(() => new ReadOnlyCollectionProxy<object>(meta, PocoGetter), innerType);
        }

        class ReadOnlyCollectionProxy<T> : ICollection<T>
        {
            public IPocoMeta Meta { get; private set; }
            public ICanGetPocos PocoGetter { get; private set; }
            public ICollection<T> InnerCollection { get; private set; }

            public ReadOnlyCollectionProxy(IPocoMeta meta, ICanGetPocos pocoGetter) {
                Meta = meta;
                PocoGetter = pocoGetter;

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
                InnerCollection.Add(item);
            }

            public void Clear() {
                InnerCollection.Clear();
            }

            public bool Contains(T item) {
                return InnerCollection.Contains(item);
            }

            public void CopyTo(T[] array, int arrayIndex) {
                InnerCollection.CopyTo(array, arrayIndex);
            }

            public bool Remove(T item) {
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