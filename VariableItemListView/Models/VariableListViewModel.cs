using VariableItemListView.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableItemListView.Models
{
    public class VariableListViewModel : Bindable
    {
        public VariableListViewModel()
        {
            Items = new ObservableCollectionThreadSafe<VariableType>();
            bool designTime = Windows.ApplicationModel.DesignMode.DesignModeEnabled;
            if (designTime)
            {
                var i = new VariableType(1);
                Items.Add(i);
                i = new VariableType(2);
                Items.Add(i);
            }
        }

        public ObservableCollectionThreadSafe<VariableType> Items {
            get { return Get<ObservableCollectionThreadSafe<VariableType>>(); }
            set { Set(value); }
        }

    }
}
