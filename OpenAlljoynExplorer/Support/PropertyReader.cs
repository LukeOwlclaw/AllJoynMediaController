using System;
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
        private Dictionary<int, string> KnownObjects = new Dictionary<int, string>();

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
            ReadRecursively(propertyPath: new string[] { propertyName }, propertyObject: propertyObject, propertyType: propertyType);
        }

        bool IsSimple(object obj)
        {
            return obj == null || obj.GetType() == typeof(string);
        }

        /// <summary>
        /// Reads <paramref name="propertyObject"/> and all its properties recursively.
        /// </summary>
        /// <param name="propertyPath"></param>
        /// <param name="propertyObject"></param>
        /// <param name="propertyType"></param>
        private void ReadRecursively(IEnumerable<string> propertyPath, object propertyObject, Type propertyType = null)
        {
            // If given, load available properties from type, else try reading from obj itself (will not work for ComObjects!)
                if (propertyType == null)
            {
                propertyType = propertyObject.GetType();
                if (!SkipComObjects && propertyType.ToString().Equals(ComObjectTypeString, StringComparison.Ordinal))
                    throw new ArgumentException();
            }

            PropertyInfo[] props = propertyType.GetProperties();

            
            if (props.Length == 0 || IsSimple(propertyObject))
            {
                // For simple objects: E.g. int(does not have properties) and simple type like string(has only non - relevant properties)
                // Include current simple object itself
                AddProperty(propertyPath, propertyObject);
                // No hashing, not getting child properties
                return;
            }

            // Avoid recursive loops
            var hash = propertyObject.GetHashCode();
            VariableType item = new VariableType
            {
                PropertyPath = propertyPath,
            };
            if (KnownObjects.ContainsKey(hash))
            {
                item.Value = "Cycle to " + KnownObjects[hash];
                AddProperty(item.PropertyPath, item.Value);
                return;
            }
            KnownObjects.Add(hash, item.PropertyPathString ?? "this object");

            // Include current object itself
            AddProperty(propertyPath, propertyObject);
            

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
                        ReadRecursively(AddToLastItemAndReturnNewList(propertyPath, "[" + i + "]"), listItem, listItemType);
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
                    ReadRecursively(AddToLastItemAndReturnNewList(propertyPath, "[" + count++ + "]"), o, elementType);
                }
            }
            else
            {
                foreach (var prop in props)
                {
                    var propertyName = prop.Name;
                    object propertyValue = prop.GetValue(propertyObject);

                    var newPropertyPath = propertyPath.Concat(new[] { propertyName });

                    // get child type, or null
                    PropertyMap.TryGetValue(propertyName, out Type childType);
                    ReadRecursively(newPropertyPath, propertyValue, childType);
                }
                
            }
        }

        private IEnumerable<string> AddToLastItemAndReturnNewList(IEnumerable<string> propertyPath, string v)
        {
            return propertyPath.Take(propertyPath.Count() - 1).Concat(new[] { propertyPath.LastOrDefault() + v });
        }

        public int PropertyCounter { get; private set; }

        private void AddProperty(IEnumerable<string> propertyPath, object propertyValue)
        {
            VariableType item = new VariableType
            {
                PropertyPath = propertyPath,
                Value = propertyValue,
                Index = PropertyCounter++
            };
            //System.Diagnostics.Debug.WriteLine($" {item.PropertyPathString} -> {propertyValue}");
            Out.Items.Add(item);            
        }
    }
}
