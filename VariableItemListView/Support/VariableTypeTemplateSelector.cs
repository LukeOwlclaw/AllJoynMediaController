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
        private static Dictionary<Type, DataTemplate> gTypeDict = null;

        public VariableTypeTemplateSelector()
        {

        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var variableTypeObject = item as VariableType;
            if (variableTypeObject == null)
                throw new ArgumentException($"{nameof(VariableTypeTemplateSelector)} only accepts items of type {nameof(VariableType)}");

            if (gTypeDict == null)
            {
                gTypeDict = new Dictionary<Type, DataTemplate>
                {
                    {typeof(int),IntItemTemplate},
                    {typeof(UInt16),IntItemTemplate},
                    {typeof(string),StringItemTemplate},
                };
            }

            DataTemplate template = null;
            if (variableTypeObject.Value != null)
                gTypeDict.TryGetValue(variableTypeObject.Value.GetType(), out template);

            if (template == null)
            {
                if (ThrowOnUnsupportedType)
                    throw new NotSupportedException($"Unsupported value type: {variableTypeObject.Value.GetType()}");
                else
                    return StringItemTemplate;
            }
            return template;
        }

        /// <summary>
        /// THrows exception if unsupported value type is found. Otherwise StringItemTemplate is used.
        /// </summary>
        public bool ThrowOnUnsupportedType { get; set; }

        public DataTemplate StringItemTemplate { get; set; }
        public DataTemplate IntItemTemplate { get; set; }
    }
}
