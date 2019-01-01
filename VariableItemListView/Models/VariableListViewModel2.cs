using VariableItemListView.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Support;
using System.Collections.ObjectModel;

namespace VariableItemListView.Models
{
    public class VariableListViewModel2 : Bindable
    {
        public VariableListViewModel2()
        {
            Items = new ObservableCollection<VariableType>();
        }

        public ObservableCollection<VariableType> Items {
            get { return Get<ObservableCollection<VariableType>>(); }
            set { Set(value); }
        }

    }
}
