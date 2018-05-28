using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Data;

namespace VariableItemListView.Support
{
    public class ObservableCollectionThreadSafe<T> : ObservableCollection<T>
    {
        private object syncLock;
        public ObservableCollectionThreadSafe()
        {
            if (!CoreWindow.GetForCurrentThread().Dispatcher.HasThreadAccess)
            {
                throw new Exception($"{nameof(ObservableCollectionThreadSafe<T>)} must be created on UI thread!");
                //otherwise later Add, Remove, ... operations will still cause:
                //“An exception of type ‘System.NotSupportedException’ occurred in PresentationFramework.dll but was not handled in user code
                //Additional information: This type of CollectionView does not support changes to its SourceCollection from a thread different from the Dispatcher thread.”
            }

            Initialize();
        }

        private void Initialize()
        {
            syncLock = new object();
            // Only for WPF:
            // BindingOperations.EnableCollectionSynchronization(this, syncLock);
           
        }

        public ObservableCollectionThreadSafe(IEnumerable<T> collection) : base(collection)
        {
            Initialize();
        }

        public ObservableCollectionThreadSafe(List<T> list) : base(list)
        {
            Initialize();
        }

        public void AddFront(T item)
        {
            InsertItem(0, item);
        }

        protected override void InsertItem(int index, T item)
        {
            //we are locking on syncLock to allow anyone synchronize with collection on UI thread
            lock (syncLock)
            {
                base.InsertItem(index, item);
            }            
        }        

        protected override void ClearItems()
        {
            //we are locking on syncLock to allow anyone synchronize with collection on UI thread
            lock (syncLock)
            {
                base.ClearItems();
            }
        }

        protected override void MoveItem(int oldIndex, int newIndex)
        {
            //we are locking on syncLock to allow anyone synchronize with collection on UI thread
            lock (syncLock)
            {
                base.MoveItem(oldIndex, newIndex);
            }
        }

        protected override void RemoveItem(int index)
        {
            //we are locking on syncLock to allow anyone synchronize with collection on UI thread
            lock (syncLock)
            {
                base.RemoveItem(index);
            }
        }

        protected override void SetItem(int index, T item)
        {
            //we are locking on syncLock to allow anyone synchronize with collection on UI thread
            lock (syncLock)
            {
                base.SetItem(index, item);
            }
        }
    }
}
