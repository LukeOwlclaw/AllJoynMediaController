using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Shared.Converters
{
    // MarkupExtension for converter does not work for UWP.
    //public class VisibleWhenZeroConverter : ConverterMarkupExtension<VisibleWhenZeroConverter>
    public class VisibleWhenZeroConverter : IValueConverter
    {
        public object Convert(object v, Type t, object p, string l) =>
        Equals(0d, System.Convert.ToDouble(v)) ? Visibility.Visible : Visibility.Collapsed;

        public object ConvertBack(object v, Type t, object p, string l) => null;
    }
}
