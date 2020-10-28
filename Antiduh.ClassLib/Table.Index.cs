using System;
using System.Collections;
using System.Collections.Generic;

namespace Antiduh.ClassLib
{
    public partial class Table<TVal> : IList<TVal>
    {
        private class TableIndex<TKey> : ITableIndex<TKey, TVal>, IIndexUpdate
        {
            private readonly Func<TVal, TKey> keySelector;
            private readonly Dictionary<TKey, IList<TVal>> map;

            public IEnumerable<TKey> Keys => this.map.Keys;

            public IEnumerable<IList<TVal>> Values => this.map.Values;

            public int Count => throw new NotImplementedException();

            public TableIndex( Func<TVal, TKey> selector )
            {
                this.keySelector = selector;

                this.map = new Dictionary<TKey, IList<TVal>>();

            }

            public IList<TVal> this[TKey key] => this.map[key];

            public bool ContainsKey( TKey key )
            {
                return this.map.ContainsKey( key );
            }

            public bool TryGetValue( TKey key, out IList<TVal> value )
            {
                return this.map.TryGetValue( key, out value );
            }

            public IEnumerator<KeyValuePair<TKey, IList<TVal>>> GetEnumerator()
            {
                return this.map.GetEnumerator();
            }

            void IIndexUpdate.Update( TVal newRecord )
            {
                TKey key = this.keySelector( newRecord );

                IList<TVal> records;

                if( this.map.TryGetValue( key, out records ) == false )
                {
                    records = new List<TVal>();
                    this.map[key] = records;
                }

                records.Add( newRecord );
            }

            void IIndexUpdate.Remove( TVal record )
            {
                TKey key = this.keySelector( record );

                this.map.Remove( key );
            }

            void IIndexUpdate.Clear()
            {
                this.map.Clear();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ( (IEnumerable)this.map ).GetEnumerator();
            }
        }
    }

}
