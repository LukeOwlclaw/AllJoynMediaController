using VariableItemListView.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace VariableItemListView.Support
{
    public class VariableTypeTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var file = item as VariableType;
            if (file.v == 1)
                return StringItemTemplate;
            else
                return IntItemTemplate;
        }

        public DataTemplate StringItemTemplate { get; set; }
        public DataTemplate IntItemTemplate { get; set; }
    }
}
