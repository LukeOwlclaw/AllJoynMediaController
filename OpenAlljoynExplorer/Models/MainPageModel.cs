using OpenAlljoynExplorer.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VariableItemListView.Models;
using VariableItemListView.Support;

namespace OpenAlljoynExplorer.Models
{
    public class MainPageModel : VariableItemListView.Support.Bindable
    {
        public MainPageModel()
        {
            VariableListViewModel = new VariableListViewModel();
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
