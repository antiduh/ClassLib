using System;
using System.Collections;
using System.Collections.Generic;

namespace Antiduh.ClassLib
{
    /// <summary>
    /// Implements a list-like collection that also provides the ability to define one more indexes
    /// on the records and then access all known records through those indexes.
    /// </summary>
    /// <typeparam name="TVal">The type of the record to store.</typeparam>
    public partial class Table<TVal> : IList<TVal>
    {
        private readonly List<TVal> list;

        private readonly List<IIndexUpdate> tableIndexes;

        /// <summary>
        /// Initializes a new instance of the <see cref="Table{TVal}"/> class that has the default
        /// initial capacity.
        /// </summary>
        public Table()
            : this( 0 )
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Table{TVal}"/> class with the specified
        /// initial capacity.
        /// </summary>
        /// <param name="capacity">
        /// The number of elements the new <see cref="Table{TVal}"/> can store.
        /// </param>
        public Table( int capacity )
        {
            this.list = new List<TVal>( capacity );
            this.tableIndexes = new List<IIndexUpdate>();
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The index of the element to access.</param>
        /// <returns>The element at the specified index.</returns>
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

        /// <summary>
        /// Gets the number of elements stored in the table.
        /// </summary>
        public int Count => this.list.Count;

        /// <summary>
        /// Returns false.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Adds the element to the table.
        /// </summary>
        /// <param name="item">The element to add.</param>
        public void Add( TVal item )
        {
            this.list.Add( item );

            foreach( var tableIndex in this.tableIndexes )
            {
                tableIndex.Update( item );
            }
        }

        /// <summary>
        /// Declares a new index on the table and returns a read-only view of the index.
        /// </summary>
        /// <typeparam name="TKey">The type of the primary key to index elements with.</typeparam>
        /// <param name="selector">A function that returns the primary key given an element.</param>
        /// <returns>The index.</returns>
        public ITableIndex<TKey, TVal> CreateIndex<TKey>( Func<TVal, TKey> selector )
        {
            var index = new TableIndex<TKey>( selector );

            this.tableIndexes.Add( index );

            IIndexUpdate update = index;
            foreach( var value in this.list )
            {
                update.Update( value );
            }

            return index;
        }

        /// <summary>
        /// Removes all elements from the table.
        /// </summary>
        public void Clear()
        {
            this.list.Clear();

            foreach( var tableIndex in this.tableIndexes )
            {
                tableIndex.Clear();
            }
        }

        /// <summary>
        /// Returns whether the table contains the given element.
        /// </summary>
        /// <param name="item">The element to search for.</param>
        /// <returns>True if the table contains the element, false otherwise.</returns>
        public bool Contains( TVal item )
        {
            return this.list.Contains( item );
        }

        /// <summary>
        /// Copies the elements stored in the table to the given array.
        /// </summary>
        /// <param name="array">The array to copy to.</param>
        /// <param name="arrayIndex">The first index in the array to write to.</param>
        public void CopyTo( TVal[] array, int arrayIndex )
        {
            this.list.CopyTo( array, arrayIndex );
        }

        /// <summary>
        /// Returns an enumerator that iterates through all values stored in the table.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TVal> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        /// <summary>
        /// Searches the table for the element and returns the index of the first occurance, or -1
        /// if not found.
        /// </summary>
        /// <param name="item">The item to search for.</param>
        /// <returns>The index of the found item; otherwise, -1.</returns>
        public int IndexOf( TVal item )
        {
            return this.list.IndexOf( item );
        }

        /// <summary>
        /// Inserts the element into the table at the specified index.
        /// </summary>
        /// <param name="index">The index at which the element should be stored.</param>
        /// <param name="item">The element to store.</param>
        public void Insert( int index, TVal item )
        {
            this.list.Insert( index, item );

            foreach( var tableIndex in this.tableIndexes )
            {
                tableIndex.Update( item );
            }
        }

        /// <summary>
        /// Removes the first occurance of the element in the table.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>True if the element was removed from the list; false otherwise.</returns>
        public bool Remove( TVal item )
        {
            foreach( var tableIndex in this.tableIndexes )
            {
                tableIndex.Remove( item );
            }

            return this.list.Remove( item );
        }

        /// <summary>
        /// Removes the element at the given index from the table.
        /// </summary>
        /// <param name="index">The index of the element to remove.</param>
        public void RemoveAt( int index )
        {
            TVal item = this.list[index];
            foreach( var tableIndex in this.tableIndexes )
            {
                tableIndex.Remove( item );
            }

            this.list.RemoveAt( index );
        }

        /// <summary>
        /// Returns an enumerator that iterates through the table.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        /// <summary>
        /// Specifies the interface for updating index objects.
        /// </summary>
        private interface IIndexUpdate
        {
            /// <summary>
            /// Adds the record to the index.
            /// </summary>
            /// <param name="record"></param>
            void Update( TVal record );

            /// <summary>
            /// Removes the record from the index.
            /// </summary>
            /// <param name="record"></param>
            void Remove( TVal record );

            /// <summary>
            /// Removes all items from the index.
            /// </summary>
            void Clear();
        }
    }

    /// <summary>
    /// Specifies the interface for indexes returned by the <see cref="Table{TVal}"/> class.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys stored by the index.</typeparam>
    /// <typeparam name="TVal">The type of the values stored by the index.</typeparam>
    public interface ITableIndex<TKey, TVal> : IReadOnlyDictionary<TKey, IList<TVal>>
    {
    }
}