﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DeviceProviders;
using VariableItemListView.Models;

namespace OpenAlljoynExplorer.Support
{

    public class PropertyReader
    {
        private const string ComObjectTypeString = "System.__ComObject";
        private HashSet<int> KnownObjects = new HashSet<int>();

        /// <summary>
        /// Map from property name to type of property.
        /// This is essential for System.__ComObjects because their type cannot be determined
        /// automatically.
        /// </summary>
        public Dictionary<string, Type> PropertyMap { get; internal set; }
        public VariableListViewModel Out { get; internal set; }
        public bool SkipComObjects { get; set; } = true;

        /// <summary>
        /// Read all properties of <paramref name="propertyObject"/> recursively and
        /// adds names and values of read properties to <see cref="Out"/>.
        /// </summary>
        /// <param name="propertyObject"></param>
        /// <param name="propertyName"></param>
        internal void Read(object propertyObject, string propertyName)
        {
            if (Out == null)
                throw new ArgumentNullException($"{nameof(Out)} must be set!");
            Type propertyType = null;
            if (PropertyMap != null && propertyName != null)
                propertyType = PropertyMap[propertyName];
            GetProps(propertyParent: "", propertyObject: propertyObject, propertyType: propertyType);
        }

        bool IsSimple(object obj)
        {
            return obj == null || obj.GetType() == typeof(string);
        }

        private void GetProps(string propertyParent, object propertyObject, Type propertyType = null)
        {
            // Filter simple object, e.g. we are not interested in the properties of a string.
            if (IsSimple(propertyObject)) return;

            // Avoid recursive loops
            var hash = propertyObject.GetHashCode();
            if (KnownObjects.Contains(hash)) return;
            KnownObjects.Add(hash);

            // If given, load available properties from type, else try reading from obj itself (will not work for ComObjects!)
            if (propertyType == null)
            {
                propertyType = propertyObject.GetType();
                if (!SkipComObjects && propertyType.ToString().Equals(ComObjectTypeString, StringComparison.Ordinal))
                    throw new ArgumentException();
            }

            PropertyInfo[] props = propertyType.GetProperties();

            // Handle IList<> - each element is handled individually.
            var info = propertyType.GetTypeInfo();
            if (info.IsGenericType)
            {
                if (info.GetGenericTypeDefinition() == typeof(IList<>))
                {
                    // get the type of the list elements, ie. T for IList<T>
                    var listItemType = info.GenericTypeArguments[0];

                    // IList implements ICollection which provides Count property. Get it!
                    var collection = typeof(ICollection<>).MakeGenericType(listItemType);
                    var countProperty = collection.GetProperty("Count");
                    var countValue = (int)countProperty?.GetValue(propertyObject);

                    // for each element in IList
                    for (int i = 0; i < countValue; i++)
                    {
                        var listItem = info.GetDeclaredMethod("get_Item").Invoke(propertyObject, new object[] { i });
                        GetProps(propertyParent + "[" + i + "]", listItem, listItemType);
                    }
                }
            }
            else if (propertyType.IsArray)
            {
                // not tested. Will not work if obj is ComObject because casting to Array will return null.
                if (propertyType.ToString().Equals(ComObjectTypeString, StringComparison.Ordinal))
                    throw new NotSupportedException();

                Array array = (Array)propertyObject;
                var elementType = propertyType.GetElementType();
                int count = 0;
                foreach (object o in array)
                {
                    GetProps(propertyParent + "[" + count++ + "]", o, elementType);
                }
            }
            else
            {
                foreach (var prop in props)
                {
                    var propertyName = prop.Name;
                    object propertyValue = prop.GetValue(propertyObject);

                    if (SkipComObjects && propertyValue != null && propertyValue.ToString() != ComObjectTypeString) {
                        AddProperty(propertyParent, propertyName, propertyValue);
                    }

                    // get child type, or null
                    PropertyMap.TryGetValue(propertyName, out Type childType);
                    GetProps(propertyParent + "." + propertyName, propertyValue, childType);
                }
            }
        }

        private void AddProperty(string propertyParent, string propertyName, object propertyValue)
        {
            System.Diagnostics.Debug.WriteLine($" {propertyParent} {propertyName} -> {propertyValue}");
            VariableType item = new VariableType
            {
                Name = propertyName,
                Value = propertyValue
            };
            Out.Items.Add(item);
        }
    }
}
