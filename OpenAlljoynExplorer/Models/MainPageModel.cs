using Shared.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VariableItemListView.Models;
using VariableItemListView.Support;

namespace OpenAlljoynExplorer.Models
{
    /// <summary>
    /// Holds connection instances to all available AllJoyn services.
    /// </summary>
    public class AllJoynModel : Bindable
    {
        public AllJoynModel()
        {
            VariableListViewModel = new VariableListViewModel();
            AllJoynServices = new ObservableCollectionThreadSafe<AllJoynService>();            
        }

        public ObservableCollectionThreadSafe<AllJoynService> AllJoynServices
        {
            get { return Get<ObservableCollectionThreadSafe<AllJoynService>>(); }
            set { Set(value); }
        }

        public VariableListViewModel VariableListViewModel
        {
            get { return Get<VariableListViewModel>(); }
            set { Set(value); }
        }

        //public ObservableCollectionThreadSafe<VariableType> Items
        //{
        //    get { return Get<ObservableCollectionThreadSafe<VariableType>>(); }
        //    set { Set(value); }
        //}
        //public VariableType Items
        //{
        //    get { return Get<VariableType>(); }
        //    internal set { Set(value); }
        //}
    }
}
