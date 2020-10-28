using System;
using System.Collections;
using System.Collections.Generic;

namespace Antiduh.ClassLib
{
    public partial class Table<TVal> : IList<TVal>
    {
        private readonly List<TVal> list;

        private readonly List<IIndexUpdate> tableIndexes;

        public Table()
        {
            this.list = new List<TVal>();

            this.tableIndexes = new List<IIndexUpdate>();
        }

        public TVal this[int index]
        {
            get
            {
                return this.list[index];
            }
            set
            {
                var oldItem = this.list[index];

                foreach( var tableIndex in this.tableIndexes )
                {
                    tableIndex.Remove( oldItem );
                }

                this.list[index] = value;

                foreach( var tableIndex in this.tableIndexes )
                {
                    tableIndex.Update( value );
                }
            }
        }

        public int Count => this.list.Count;

        public bool IsReadOnly => false;

        public void Add( TVal item )
        {
            this.list.Add( item );

            foreach( var tableIndex in this.tableIndexes )
            {
                tableIndex.Update( item );
            }
        }

        public ITableIndex<TKey, TVal> CreateIndex<TKey>( Func<TVal, TKey> selector )
        {
            var index = new TableIndex<TKey>( selector );

            this.tableIndexes.Add( index );

            return index;
        }

        public void Clear()
        {
            this.list.Clear();

            foreach( var tableIndex in this.tableIndexes )
            {
                tableIndex.Clear();
            }
        }

        public bool Contains( TVal item )
        {
            return this.list.Contains( item );
        }

        public void CopyTo( TVal[] array, int arrayIndex )
        {
            this.list.CopyTo( array, arrayIndex );
        }

        public IEnumerator<TVal> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        public int IndexOf( TVal item )
        {
            return this.list.IndexOf( item );
        }

        public void Insert( int index, TVal item )
        {
            this.list.Insert( index, item );

            foreach( var tableIndex in this.tableIndexes )
            {
                tableIndex.Update( item );
            }
        }

        public bool Remove( TVal item )
        {
            foreach( var tableIndex in this.tableIndexes )
            {
                tableIndex.Remove( item );
            }

            return this.list.Remove( item );
        }

        public void RemoveAt( int index )
        {
            TVal item = this.list[index];
            foreach( var tableIndex in this.tableIndexes )
            {
                tableIndex.Remove( item );
            }

            this.list.RemoveAt( index );
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        private interface IIndexUpdate
        {
            void Update( TVal record );

            void Remove( TVal record );

            void Clear();
        }

    }

    public interface ITableIndex<TKey, TVal> : IReadOnlyDictionary<TKey, IList<TVal>>
    {
    }
}
