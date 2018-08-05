using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceProviders;
using Shared.Support;
using VariableItemListView.Models;

namespace OpenAlljoynExplorer.Models
{
    public class MethodModel : Bindable
    {
        public MethodModel()
        {
            VariableListViewModel = new VariableListViewModel();
        }

        public VariableListViewModel VariableListViewModel
        {
            get { return Get<VariableListViewModel>(); }
            set { Set(value); }
        }

        public IMethod Method
        {
            get { return Get<IMethod>(); }
            set { Set(value); }
        }

        public AllJoynStatus MethodStatus
        {
            get { return Get<AllJoynStatus>(); }
            set { Set(value); }
        }

        public string MethodResult
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

    }
}
