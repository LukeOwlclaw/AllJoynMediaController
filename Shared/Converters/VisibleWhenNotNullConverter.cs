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
    public class VisibleWhenNotNullConverter : IValueConverter
    {
        public object Convert(object v, Type t, object p, string l) =>
        v != null ? Visibility.Visible : Visibility.Collapsed;

        public object ConvertBack(object v, Type t, object p, string l) => null;
    }
}
