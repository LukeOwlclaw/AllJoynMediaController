using System;
using System.Collections.Generic;
using System.Text;
using DeviceProviders;
using Newtonsoft.Json.Linq;
using OpenAlljoynExplorer.Controllers;
using OpenAlljoynExplorer.Models;
using OpenAlljoynExplorer.Support;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace OpenAlljoynExplorer.Pages
{
    /// <summary>
    /// Page that shows a given AllJoyn Interface (including Properties, Methods, Signals)
    /// (Interface is part of Object which is part of AllJosn Service)
    /// </summary>
    public sealed partial class MethodPage : BackBasePage
    {
        public MethodModel VM { get; set; }

        public ServicePageController Controller { get; set; }

        public MethodPage() : base()
        {
            this.InitializeComponent();
            Loaded += MethodPage_Loaded;
        }

        private void MethodPage_Loaded(object sender, RoutedEventArgs e)
        {
            ReadAll();
            VM.InvocationParametersAsJson = GetInSignatureAsJson();
        }

        private string GetInSignatureAsJson()
        {
            JObject json = new JObject();
            foreach (var inSignature in VM.Method.InSignature)
            {
                json[inSignature.Name] = GetValueTypeAsJson(inSignature.TypeDefinition, null, true);
            }
            return json.ToString();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            BackButton.IsEnabled = this.Frame.CanGoBack;
            VM = (MethodModel)e.Parameter;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            On_BackRequested();
        }

        internal void ReadAll()
        {
            // Define all properties/fields which we want PropertyReader to read
            Dictionary<string, Type> PropMap = new Dictionary<string, Type> {
                {"AboutData", typeof(IAboutData) },
                {"Provider", typeof(IProvider) },
                {"Objects", typeof(IList<IBusObject>) },
                {"ChildObjects", typeof(IList<IBusObject>) },
                {"Interfaces", typeof(IList<IInterface>) },
                {"Methods", typeof(IReadOnlyList<IMethod>) },
                {"Annotations", typeof(IReadOnlyDictionary<string, string>) },
                {"OutSignature", typeof(IList<DeviceProviders.ParameterInfo>) },
                {"InSignature", typeof(IList<DeviceProviders.ParameterInfo>) },
                {"Services", typeof(IList<IService>) }, // Services in Provider
                { nameof(VM.Method), typeof(IMethod)},
                { "test", typeof(IList<ITypeDefinition>)}
            };

            var propertyReader = new PropertyReader
            {
                PropertyMap = PropMap,
                Out = VM.VariableListViewModel
            };

            propertyReader.Read(VM.Method, nameof(VM.Method));
            //propertyReader.Read(AllJoynTypeDefinition.CreateTypeDefintions("a{sv}") , "test");
        }

        /// <summary>
        /// Convert JSON object <paramref name="parameter"/> according to <paramref name="typeDefinition"/> to object which can be sent to
        /// AllJoyn receiver.
        /// </summary>
        /// <param name="typeDefinition">Type definition for given parameter</param>
        /// <param name="parameter">Parameter as JSON object</param>
        /// <returns></returns>
        private object GetValueAsObject(ITypeDefinition typeDefinition, JToken parameter)
        {
            switch (typeDefinition.Type)
            {
                case TypeId.Invalid:
                    return "<invalid>";
                case TypeId.Boolean:
                    return parameter.ToObject<bool>();
                case TypeId.Double:
                    return parameter.ToObject<double>();
                case TypeId.Dictionary:
                    break;
                case TypeId.Signature:
                    break;
                case TypeId.Int32:
                    return parameter.ToObject<Int32>();
                case TypeId.Int16:
                    return parameter.ToObject<Int16>();
                case TypeId.ObjectPath:
                    break;
                case TypeId.Uint16:
                    return parameter.ToObject<UInt16>();
                case TypeId.Struct:
                    break;
                case TypeId.String:
                    return parameter.ToObject<string>();
                case TypeId.Uint64:
                    break;
                case TypeId.Uint32:
                    return parameter.ToObject<UInt32>();
                case TypeId.Variant:
                    break;
                case TypeId.Int64:
                    return parameter.ToObject<Int64>();
                case TypeId.Uint8:
                    return parameter.ToObject<char>();
                case TypeId.ArrayByte:
                    break;
                case TypeId.ArrayByteMask:
                    break;
                case TypeId.BooleanArray:
                    break;
                case TypeId.DoubleArray:
                    break;
                case TypeId.Int32Array:
                    break;
                case TypeId.Int16Array:
                    break;
                case TypeId.Uint16Array:
                    break;
                case TypeId.Uint64Array:
                    break;
                case TypeId.Uint32Array:
                    break;
                case TypeId.VariantArray:
                    break;
                case TypeId.Int64Array:
                    break;
                case TypeId.Uint8Array:
                    break;
                case TypeId.SignatureArray:
                    break;
                case TypeId.ObjectPathArray:
                    break;
                case TypeId.StringArray:
                    break;
                case TypeId.StructArray:
                    break;
                default:
                    break;
            }
            throw new NotImplementedException();
        }

        private JToken GetValueTypeAsJson(ITypeDefinition typeDefinition, object value, bool createTypeTemplate)
        {
            if (createTypeTemplate && value != null)
            {
                throw new ArgumentException("When requesting to create template, value is ignored and must thus be null");
            }
            switch (typeDefinition.Type)
            {
                case TypeId.Invalid:
                    return JToken.FromObject("<invalid>");
                case TypeId.Boolean:
                    return JToken.FromObject(value ?? default(bool));
                case TypeId.Double:
                    return JToken.FromObject(value ?? default(double));
                case TypeId.Dictionary:
                    var dictionary = value as IList<KeyValuePair<object, object>>;
                    if (createTypeTemplate)
                    {
                        // create dictionary with two entries to be used as template
                        dictionary = new List<KeyValuePair<object, object>>
                        {
                            new KeyValuePair<object, object>(null, null),
                            new KeyValuePair<object, object>(null, null)
                        };
                    }
                    if (dictionary == null)
                    {
                        return JToken.FromObject("According to type definition value should be a dictionary but it is not!");
                    }
                    var returnDictionary = new Dictionary<JToken, JToken>(dictionary.Count);
                    foreach (var item in dictionary)
                    {
                        var itemKey = GetValueTypeAsJson(typeDefinition.KeyType, item.Key, createTypeTemplate);
                        var itemValue = GetValueTypeAsJson(typeDefinition.ValueType, item.Value, createTypeTemplate);
                        returnDictionary.Add(itemKey, itemValue);
                    }
                    return JToken.FromObject(returnDictionary);
                case TypeId.Signature:
                    break;
                case TypeId.Int32:
                    return JToken.FromObject(value ?? default(Int32));
                case TypeId.Int16:
                    return JToken.FromObject(value ?? default(Int16));
                case TypeId.ObjectPath:
                    break;
                case TypeId.Uint16:
                    return JToken.FromObject(value ?? default(UInt16));
                case TypeId.Struct:
                    break;
                case TypeId.String:
                    // for template return "string", otherwise default value is null string
                    return JToken.FromObject(createTypeTemplate ? "string" : value ?? "\0");
                case TypeId.Uint64:
                    return JToken.FromObject(value ?? default(UInt64));
                case TypeId.Uint32:
                    return JToken.FromObject(value ?? default(UInt32));
                case TypeId.Variant:
                    var variant = value as AllJoynMessageArgVariant;
                    if (createTypeTemplate)
                    {
                        // Variant can contain anything. Use simple string for template
                        var varianteTemplateType = AllJoynTypeDefinition.CreateTypeDefintions("s")[0];
                        return GetValueTypeAsJson(varianteTemplateType, null, createTypeTemplate);
                    }
                    if (variant == null)
                    {
                        return JToken.FromObject("According to type definition value should be a variant but it is not!");
                    }
                    return GetValueTypeAsJson(variant.TypeDefinition, variant.Value, createTypeTemplate);
                case TypeId.Int64:
                    return JToken.FromObject(value ?? default(Int64));
                case TypeId.Uint8:
                    return JToken.FromObject(value ?? (byte)0);
                case TypeId.ArrayByte:
                    break;
                case TypeId.ArrayByteMask:
                    break;
                case TypeId.BooleanArray:
                    break;
                case TypeId.DoubleArray:
                    break;
                case TypeId.Int32Array:
                    break;
                case TypeId.Int16Array:
                    break;
                case TypeId.Uint16Array:
                    break;
                case TypeId.Uint64Array:
                    break;
                case TypeId.Uint32Array:
                    break;
                case TypeId.VariantArray:
                    break;
                case TypeId.Int64Array:
                    break;
                case TypeId.Uint8Array:
                    break;
                case TypeId.SignatureArray:
                    break;
                case TypeId.ObjectPathArray:
                    break;
                case TypeId.StringArray:
                    break;
                case TypeId.StructArray:
                    //TestObjectType(value);
                    var fields = typeDefinition.Fields as IReadOnlyList<ITypeDefinition>;
                    if (fields == null)
                    {
                        throw new ArgumentNullException("typeDefinition must contains type fields type definition for StructArray");
                    }
                    var structValues = value as IList<object>;
                    if (createTypeTemplate)
                    {
                        var structTemplateItem = new AllJoynMessageArgStructure(typeDefinition);
                        structValues = new List<object>() { structTemplateItem };
                    }
                    if (structValues == null)
                    {
                        return JToken.FromObject("According to type definition value should be a StructArray but it is not!");
                    }
                    var returnList = new List<JToken[]>(structValues.Count);
                    foreach (AllJoynMessageArgStructure structEntry  in structValues)
                    {
                        if (structEntry.Count != fields.Count)
                        {
                            return JToken.FromObject($"Got {structValues.Count} values in struct entry, expected {fields.Count} ");
                        }
                        var entryArray = new JToken[fields.Count];
                        for (int i = 0; i < fields.Count; i++)
                        {
                            var structField = fields[i];
                            var structValue = structEntry[i];
                            entryArray[i] = GetValueTypeAsJson(structField, structValue, createTypeTemplate);
                        }
                        returnList.Add(entryArray);
                    }
                    return JToken.FromObject(returnList);
                default:
                    throw new NotImplementedException();
            }
            throw new NotImplementedException();
        }
            private async void InvokeButton_Click(object sender, RoutedEventArgs e)
        {
            List<object> inArgs = GetInArgumentsAsObjectList();
            var result = await VM.Method.InvokeAsync(inArgs);
            var status = result.Status as AllJoynStatus;
            VM.MethodStatus = status;
            if (result?.Status == null)
            {
                return;
            }

            if (result.Status.IsSuccess)
            {
                if (result.Values.Count != VM.Method.OutSignature.Count)
                {
                    VM.MethodResult = $"Got {result.Values.Count} return values, expected {VM.Method.OutSignature.Count}";
                    return;
                }

                var valueResultItems = new Dictionary<string, JToken>();
                var values = result.Values as IList<object>;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine();
                for (int i = 0; i < VM.Method.OutSignature.Count; i++)
                {
                    ParameterInfo signature = VM.Method.OutSignature[i];
                    var value = result.Values[i];
                    var valueAsJson = GetValueTypeAsJson(signature.TypeDefinition, value, false);
                    valueResultItems.Add(signature.Name, valueAsJson);
                }

                VM.MethodResult = JToken.FromObject(valueResultItems).ToString();
                return;
            }
        }

        /// <summary>
        /// Returns method arguments entered by user as JSON string as list of objects which can be sent to AllJoyn receiver.
        /// </summary>
        /// <returns>List of object (method arguments)</returns>
        private List<object> GetInArgumentsAsObjectList()
        {
            var parameterList = new List<object> { };
            try
            {
                JToken parameters = JObject.Parse(VM.InvocationParametersAsJson);

                foreach (var inSignature in VM.Method.InSignature)
                {
                    JToken parameter = parameters.SelectToken(inSignature.Name);
                    object parameterAsObject = GetValueAsObject(inSignature.TypeDefinition, parameter);
                    parameterList.Add(parameterAsObject);
                }
            }
            catch (Exception ex)
            {
                var dialog = new Windows.UI.Popups.MessageDialog(ex.ToString(), "Error while preparing parameters");
                var asyncNoWait = dialog.ShowAsync();
            }

            return parameterList;

        }

        private void TestObjectType(object j)
        {
            var j00 = j as IList<object>;

            var j01 = j as IDictionary<string, string>;
            var j02 = j as IDictionary<string, object>;
            var j03 = j as IDictionary<string, object[]>;
            var j04 = j as IDictionary<string, object[,]>;
            var j05 = j as IDictionary<object, string>;
            var j06 = j as IDictionary<object, object>;
            var j07 = j as IDictionary<object, object[]>;
            var j08 = j as IDictionary<object, object[,]>;
            var j09 = j as IDictionary<object[], string>;
            var j10 = j as IDictionary<object[], object>;
            var j11 = j as IDictionary<object[], object[]>;
            var j12 = j as IDictionary<object[], object[,]>;
            var j13 = j as IDictionary<object[,], string>;
            var j14 = j as IDictionary<object[,], object>;
            var j15 = j as IDictionary<object[,], object[]>;
            var j16 = j as IDictionary<object[,], object[,]>;
            var j17 = j as IDictionary<object, string>;
            var j18 = j as IDictionary<object[], string>;
            var j19 = j as IDictionary<object[,], string>;

            var k01 = j as IReadOnlyDictionary<string, string>;
            var k02 = j as IReadOnlyDictionary<string, object>;
            var k03 = j as IReadOnlyDictionary<string, object[]>;
            var k04 = j as IReadOnlyDictionary<string, object[,]>;
            var k05 = j as IReadOnlyDictionary<object, string>;
            var k06 = j as IReadOnlyDictionary<object, object>;
            var k07 = j as IReadOnlyDictionary<object, object[]>;
            var k08 = j as IReadOnlyDictionary<object, object[,]>;
            var k09 = j as IReadOnlyDictionary<object[], string>;
            var k10 = j as IReadOnlyDictionary<object[], object>;
            var k11 = j as IReadOnlyDictionary<object[], object[]>;
            var k12 = j as IReadOnlyDictionary<object[], object[,]>;
            var k13 = j as IReadOnlyDictionary<object[,], string>;
            var k14 = j as IReadOnlyDictionary<object[,], object>;
            var k15 = j as IReadOnlyDictionary<object[,], object[]>;
            var k16 = j as IReadOnlyDictionary<object[,], object[,]>;
            var k17 = j as IReadOnlyDictionary<object, string>;
            var k18 = j as IReadOnlyDictionary<object[], string>;
            var k19 = j as IReadOnlyDictionary<object[,], string>;


            var l01 = j as IList<KeyValuePair<string, string>>;
            var l02 = j as IList<KeyValuePair<string, object>>;
            var l03 = j as IList<KeyValuePair<string, object[]>>;
            var l04 = j as IList<KeyValuePair<string, object[,]>>;
            var l05 = j as IList<KeyValuePair<object, string>>;
            var l06 = j as IList<KeyValuePair<object, object>>;
            var l07 = j as IList<KeyValuePair<object, object[]>>;
            var l08 = j as IList<KeyValuePair<object, object[,]>>;
            var l09 = j as IList<KeyValuePair<object[], string>>;
            var l10 = j as IList<KeyValuePair<object[], object>>;
            var l11 = j as IList<KeyValuePair<object[], object[]>>;
            var l12 = j as IList<KeyValuePair<object[], object[,]>>;
            var l13 = j as IList<KeyValuePair<object[,], string>>;
            var l14 = j as IList<KeyValuePair<object[,], object>>;
            var l15 = j as IList<KeyValuePair<object[,], object[]>>;
            var l16 = j as IList<KeyValuePair<object[,], object[,]>>;
            var l17 = j as IList<KeyValuePair<object, string>>;
            var l18 = j as IList<KeyValuePair<object[], string>>;
            var l19 = j as IList<KeyValuePair<object[,], string>>;

            System.Diagnostics.Debugger.Break();
        }

        private async void TestButton_Click(object sender, RoutedEventArgs e)
        {
            var result = await VM.Method.InvokeAsync(new List<object> { /*"en"*/ });
            //var result = await VM.Method.InvokeAsync(new List<object> {  });
            var status = result.Status as AllJoynStatus;

            VM.MethodStatus = status;

            //var i1 = await FindInterface(new Dictionary<string, int> { { "string", 11} });
            //var i2 = await FindInterface(new Dictionary<string, object> { { "string", 11} });
            //var i3 = await FindInterface(new Dictionary<object, object> { { "string", 11} });

            if (result?.Status == null)
            {
                return;
            }

            ////var t1 = GetType2(new Dictionary<string, int> { { "string", 11} }, typeof(Dictionary<string,int>));
            ////var t2 = GetType2(new Dictionary<string, int> { { "string", 11} }, typeof(Dictionary<object,object>));
            ////var t3 = GetType2(new Dictionary<string, int> { { "string", 11} }, typeof(Dictionary<,>));
            if (result.Status.IsSuccess)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in VM.Method.OutSignature)
                {
                    sb.Append($"{item.Name}:");
                    sb.AppendLine($"");

                }

                VM.MethodResult = sb.ToString();
                return;

                var resultList2 = result.Values as IList<object>;



                foreach (var resultListItem in resultList2)
                {
                    // AllJoyn Dictionary is always: IList<KeyValuePair<object, object>>
                    var pairs = resultListItem as IList<KeyValuePair<object, object>>;
                    foreach (var pair in pairs)
                    {
                        var key = pair.Key as string; //<- type string taken from definition 
                        // --> VM.Method.OutSignature[0].TypeDefinition.KeyType.Type
                        var variant = pair.Value as AllJoynMessageArgVariant;//<- type variant (AllJoynMessageArgVariant) taken from definition
                        // --> VM.Method.OutSignature[0].TypeDefinition.ValueType.Type

                        var j = variant.Value;
                        if (variant.TypeDefinition.Type == TypeId.Uint8Array)
                        {
                            var array8 = j as IList<object>;
                            foreach (byte b in array8)
                            {
                                // do something with b
                            }
                        }

                        
                    }

                    //j.ToString();


                }
                //System.Runtime.InteropServices.var
                var resultList = result.Values as IList<object>;

                foreach (var item in resultList)
                {
                    var structure = item as IReadOnlyDictionary<string, AllJoynMessageArgStructure>;
                    item.ToString();



                    //var size = 16;
                    //// Both managed and unmanaged buffers required.
                    //var bytes = new byte[size];
                    //var ptr = Marshal.AllocHGlobal(size);
                    //// Copy object byte-to-byte to unmanaged memory.
                    //Marshal.StructureToPtr(item, ptr, true);
                    //// Copy data from unmanaged memory to managed buffer.
                    //Marshal.Copy(ptr, bytes, 0, size);
                    //// Release unmanaged memory.
                    //Marshal.FreeHGlobal(ptr);

                    //                    result.Values as IList<object>
                    //item
                    //item as AllJoynTypeDefinition
                    //item as IDictionary<object, object>
                    //item as DeviceProviders.TypeId
                    //item as IReadOnlyList<object>
                    //result.Values as IDictionary<object, object>
                    //item as IReadOnlyCollection<object>
                    //item as IReadOnlyDictionary<object, object>
                    //test as IList<IBusObject>
                    //item as IReadOnlyDictionary<object, object>
                    //item as AllJoynMessageArgVariant
                    //item as IList<AllJoynMessageArgVariant>
                    //item as IDictionary<AllJoynMessageArgVariant, AllJoynMessageArgVariant>
                    //item as string
                    //item as AllJoynMessageArgStructure
                    //item as IEnumerable<object>
                    //item as object[]
                    //item as Windows.Foundation.Collections.IObservableVector<object>
                    //item as ICollection<object>
                    //item as IReadOnlyCollection<object>
                    //info.FullName
                    //t.MakeGenericType(genericTypes)
                    //App
                    //item as IDictionary<string, AllJoynMessageArgVariant>
                    //item as AllJoynMessageArgVariant

                    //Type t = await GetType(item);
                }
                //AllJoynMessageArgStructure
            }


            //result.Status

        }

        private void AddFavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            var deviceId = VM.Service.AboutData.DeviceId;
            var objectPath = VM.Interface.BusObject.Path;
            var interfaceName = VM.Interface.Name;
            var methodName = VM.Method.Name;

            var favorite = new Favorite
            {
                DeviceId = VM.Service.AboutData.DeviceId,
                ObjectPath = VM.Interface.BusObject.Path,
                InterfaceName = VM.Interface.Name,
                MethodName = VM.Method.Name
            };

            var added = Favorite.Add(favorite);

            if (!added)
            {
                var dialog = new Windows.UI.Popups.MessageDialog("Method is already a favorite.", "Not added");
                var asyncNoWait = dialog.ShowAsync();
            }
        }
    }
}
