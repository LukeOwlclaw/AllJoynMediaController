using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VariableItemListView.Support
{
    public class Bindable : INotifyPropertyChanged
    {
        private Dictionary<string, object> mProperties = new Dictionary<string, object>();

        /// <summary>
        /// Gets the value of a property
        /// </summary>
        /// <typeparam name="T">Type of property</typeparam>
        /// <param name="name">Name of property for which to get value</param>
        /// <param name="defaultValue">Value to return when none is found</param>
        /// <returns>Found value of property or <paramref name="defaultValue"/></returns>
        public T Get<T>([CallerMemberName] string name = null, T defaultValue = default(T))
        {
            Debug.Assert(name != null, "name != null");
#if DEBUG
            // If user choose a value for defaultValueForNull for a non-nullable type
            // which is not default(T), it will not work as expected
            if (default(T) != null && !default(T).Equals(defaultValue))
                Debug.Assert(defaultValue is System.Nullable, "Default value only works as expected for nullable types! Check bindable " + name);
#endif
            object value = null;
            if (mProperties.TryGetValue(name, out value))
            {
                return value == null ? defaultValue : (T)value;
            }

            return defaultValue;
        }

        /// <summary>
        /// Sets the value of a property
        /// </summary>
        /// <typeparam name="T">Type of property</typeparam>
        /// <param name="value">New value for property</param>
        /// <param name="name">Name of property for which to set value</param>
        /// <remarks>Use this overload when implicitly naming the property</remarks>
        public void Set<T>(T value, [CallerMemberName] string name = null)
        {
            Debug.Assert(name != null, "name != null");
            if (Equals(value, Get<T>(name)))
            {
                return;
            }

            mProperties[name] = value;
            OnPropertyChanged(name);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

#if DEBUG
        public System.Delegate[] GetInvocationList()
        {
            return PropertyChanged?.GetInvocationList();
        }
#endif
    }
}
