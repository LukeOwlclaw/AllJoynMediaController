using VariableItemListView.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Support;

namespace VariableItemListView.Models
{
    public class VariableListViewModel : Bindable
    {
        public VariableListViewModel()
        {
            Items = new ObservableCollectionThreadSafe<VariableType>();
        }

        public ObservableCollectionThreadSafe<VariableType> Items {
            get { return Get<ObservableCollectionThreadSafe<VariableType>>(); }
            set { Set(value); }
        }

    }
}
