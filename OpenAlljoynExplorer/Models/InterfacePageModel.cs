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
    public class InterfacePageModel : Bindable
    {
        public InterfacePageModel()
        {
        }

        public IService Service { get; internal set; }
        public IInterface Interface { get; internal set; }
    }
}
