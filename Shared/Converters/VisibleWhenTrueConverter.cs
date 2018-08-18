using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Shared.Converters
{
    public class VisibleWhenTrueConverter : IValueConverter
    {
        public object Convert(object v, Type t, object p, string l)
        {
            return Equals(true, System.Convert.ToBoolean(v)) ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object v, Type t, object p, string l) => null;
    }
}
