using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace VariableItemListView.Converters
{
    /// <summary>
    /// Allows converters to be instantiated as used in XAML code directly via:
    /// {Binding Converter={converters:TsFolderToCheckableTreeViewModel}}
    /// 
    /// Otherwise a resource needs to be defined first.
    /// 
    /// Inspired by http://www.broculos.net/2014/04/wpf-how-to-use-converters-without.html
    /// </summary>
    public abstract class ConverterMarkupExtension<T> : MarkupExtension, IValueConverter
        where T : class, new()
    {
        private static T gConverter = null;

        public ConverterMarkupExtension()
        {
        }

        protected override object ProvideValue()
        {
            return gConverter ?? (gConverter = new T());
        }

        public abstract object Convert(object value, Type targetType, object parameter, string language);
        public abstract object ConvertBack(object value, Type targetType, object parameter, string language);
    }
}
