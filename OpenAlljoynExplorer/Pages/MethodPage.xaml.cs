using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using DeviceProviders;
using Newtonsoft.Json.Linq;
using OpenAlljoynExplorer.Controllers;
using OpenAlljoynExplorer.Models;
using OpenAlljoynExplorer.Support;
using OpenAlljoynExplorer.TypeDefinitions;
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
        static object MyVariant;
        public MethodModel VM { get; set; }

        public ServicePageController Controller { get; set; }

        public MethodPage() : base()
        {
            this.InitializeComponent();
            Loaded += MethodPage_Loaded;
        }

        private void MethodPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //ReadAll();
                VM.InvocationParametersAsJson = GetInSignatureAsJson();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private string GetInSignatureAsJson()
        {
            JObject json = new JObject();
            foreach (var inSignature in VM.Method.InSignature)
            {
                json[inSignature.Name + "_TypeDef"] = TypeDefinitionToString(inSignature.TypeDefinition);
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
            // Ignore spelling: sv ssssxsssa ss
            //propertyReader.Read(AllJoynTypeDefinition.CreateTypeDefintions("a{sv}") , "Map string->variant");
            //propertyReader.Read(AllJoynTypeDefinition.CreateTypeDefintions("(ssssxsssa{ss}a{sv}v)") , "Definition of media song");
        }

        /// <summary>
        /// Convert JSON object <paramref name="parameter"/> according to <paramref name="typeDefinition"/> to object which can be sent to
        /// AllJoyn receiver.
        /// </summary>
        /// <param name="typeDefinition">Type definition for given parameter</param>
        /// <param name="parameter">Parameter as JSON object</param>
        /// <returns></returns>
        private object GetValueAsObject(ITypeDefinition typeDefinition, JToken parameter, string parameterName)
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
                    var dictionary = new List<KeyValuePair<object, object>>();
                    var keyType = typeDefinition.KeyType;
                    var valueType = typeDefinition.ValueType;
                    //JProperty dictionaryItemTupleJson = dictionaryItemTuple.Value<JProperty>();
                    foreach (var dictionaryItemTuple in parameter)
                    {
                        if (dictionaryItemTuple is JProperty dictionaryItemProperty)
                        {
                            var keyValue = dictionaryItemProperty.Name;
                            var valueValue = dictionaryItemProperty.Value;
                            var keyObject = GetValueAsObject(keyType, keyValue, parameterName);
                            var valueObject = GetValueAsObject(valueType, valueValue, parameterName);
                            dictionary.Add(new KeyValuePair<object, object>(keyObject, valueObject));
                        }
                        else
                        {
                            throw new Exception($"Parameter '{parameterName}' of type {typeDefinition.Type} requires" +
                                $" to contain JPropertys. Got: {dictionaryItemTuple?.GetType()}");
                        }
                    }
                    return dictionary;
                case TypeId.Signature:
                    break;
                case TypeId.Int32:
                    return parameter.ToObject<Int32>();
                case TypeId.Int16:
                    return parameter.ToObject<Int16>();
                case TypeId.ObjectPath:
                    return parameter.ToObject<string>();
                case TypeId.Uint16:
                    return parameter.ToObject<UInt16>();
                case TypeId.Struct:
                    break;
                case TypeId.String:
                    return parameter.ToObject<string>();
                case TypeId.Uint64:
                    return parameter.ToObject<UInt64>();
                case TypeId.Uint32:
                    return parameter.ToObject<UInt32>();
                case TypeId.Variant:
                    //var variantObject = GetJsonValueAsObject(parameter, parameterName);
                    //variantObject = "s";

                    //return MyVariant;

                    //var variant = new System2.Variant2(false);
                    //variant = System2.Variant2.DBNull;
                    //return (Object)(0 != 0);
                    //return variant;
                    var asObject = GetJsonValueAsObject(parameter, parameterName);
                    return asObject;
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
                    var list = new List<object>(parameter.Count());
                    int itemCount = 0;
                    foreach (var structItemSet in parameter)
                    {
                        if (typeDefinition.Fields.Count != structItemSet.Count())
                        {
                            throw new Exception($"Parameter '{parameterName}' of type {typeDefinition.Type} requires" +
                                $" {typeDefinition.Fields.Count()} fields. Got {structItemSet.Count()} fields for item" +
                                $" at index {itemCount} (0-based)");
                        }

                        var argument = new AllJoynMessageArgStructure(typeDefinition);

                        var structArrayValues = new List<object>(typeDefinition.Fields.Count);
                        for (int i = 0; i < typeDefinition.Fields.Count; i++)
                        {
                            var fieldDefinition = typeDefinition.Fields[i];
                            var fieldValue = structItemSet[i];
                            var parameterObject = GetValueAsObject(fieldDefinition, fieldValue, parameterName);
                            argument.Add(parameterObject);
                            structArrayValues.Add(parameterObject);
                        }
                        list.Add(argument);
                        //return argument;
                        //return structArrayValues;
                        itemCount++;
                    }
                    return list;
                default:
                    break;
            }
            throw new NotImplementedException($"typeDefinition.Type={typeDefinition.Type}");
        }

        private object GetJsonValueAsObject(JToken parameter, string parameterName)
        {
            switch (parameter.Type)
            {
                case JTokenType.None:
                    break;
                case JTokenType.Object:
                    // if object contains two JProperty named "variant" and "value", parse object as variant type.
                    if (parameter.Count() == 2 &&
                        parameter.Children().First() is JProperty variantHeader && parameter.Children().Last() is JProperty variantValue
                        && variantHeader.Name.Equals("variant") && variantValue.Name.Equals("value"))
                    {
                        ////var variantHeader = parameter.Children().First();
                        ////var variantValue = parameter.Children().Last() as JProperty;
                        //var variantHeaderJson = GetJsonValueAsObject(variantHeader  , parameterName) as KeyValuePair<object, object>?;
                        //var variantValueJson = GetJsonValueAsObject(variantValue, parameterName) as KeyValuePair<object, object>?;
                        //if (variantHeaderJson.HasValue && variantHeaderJson.Value.Key.Equals("variant") &&
                        //    variantValueJson.HasValue && variantValueJson.Value.Key.Equals("value")) {
                        var variantType = variantHeader.Value;
                        var variantContent = variantValue.Value;
                        //return variantContent;
                        switch (variantType.ToString().ToLowerInvariant())
                        {
                            case "dictionary":
                                return new List<object>();
                            case "uint8":
                                return GetValueAsObject(AlljoynTypeDefinition.Uint8, variantContent, parameterName);
                            case "uint16":
                                return GetValueAsObject(AlljoynTypeDefinition.Uint16, variantContent, parameterName);
                            case "uint32":
                                return GetValueAsObject(AlljoynTypeDefinition.Uint32, variantContent, parameterName);
                            case "uint64":
                                return GetValueAsObject(AlljoynTypeDefinition.Uint64, variantContent, parameterName);
                            case "int16":
                                return GetValueAsObject(AlljoynTypeDefinition.Int16, variantContent, parameterName);
                            case "int32":
                                return GetValueAsObject(AlljoynTypeDefinition.Int32, variantContent, parameterName);
                            case "int64":
                                return GetValueAsObject(AlljoynTypeDefinition.Int64, variantContent, parameterName);
                            case "string":
                                return GetValueAsObject(AlljoynTypeDefinition.String, variantContent, parameterName);
                        }
                        //}
                    }
                    //var objectItems = new List<object>();
                    //foreach (var item in parameter)
                    //{
                    //    objectItems.Add(GetJsonValueAsObject(item, parameterName));
                    //}
                    //return objectItems;
                    break;
                case JTokenType.Array:
                    var arrayItems = new List<object>();
                    foreach (var item in parameter)
                    {
                        arrayItems.Add(GetJsonValueAsObject(item, parameterName));
                    }
                    return arrayItems;
                case JTokenType.Constructor:
                    break;
                case JTokenType.Property:
                    JProperty property = parameter.Value<JProperty>();
                    var propertyObject = GetJsonValueAsObject(property.Value, parameterName);
                    return new KeyValuePair<object, object>(property.Name, propertyObject);
                case JTokenType.Comment:
                    break;
                case JTokenType.Integer:
                    // Note: There is no unambiguous mapping from JSON Integer to AllJoyn type (Int16, Int32, etc).
                    // We'd need some meta data to choose the correct type. Some JSON-encoded descriptive tag would be nice.
                    // For now we just use arbitrarily Int64.
                    return GetValueAsObject(AlljoynTypeDefinition.Uint16, parameter, parameterName);
                case JTokenType.Float:
                    return GetValueAsObject(AlljoynTypeDefinition.Double, parameter, parameterName);
                case JTokenType.String:
                    return GetValueAsObject(AlljoynTypeDefinition.String, parameter, parameterName);
                case JTokenType.Boolean:
                    break;
                case JTokenType.Null:
                    // Consider Null actually Null-string.
                    // This again is somewhat arbitrary.
                    return "\0";
                case JTokenType.Undefined:
                    break;
                case JTokenType.Date:
                    break;
                case JTokenType.Raw:
                    break;
                case JTokenType.Bytes:
                    break;
                case JTokenType.Guid:
                    break;
                case JTokenType.Uri:
                    break;
                case JTokenType.TimeSpan:
                    break;
                default:
                    break;
            }
            throw new NotSupportedException($"variant->parameter.Type={parameter.Type}");
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
                            new KeyValuePair<object, object>(null, null)
                        };
                    }
                    if (dictionary == null)
                    {
                        string guess = GuessObjectType(value);
                        return JToken.FromObject("According to type definition value should be a dictionary but got: " + guess);
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
                    return JToken.FromObject(createTypeTemplate ? "/Object/Path" : value ?? "\0");
                case TypeId.Uint16:
                    return JToken.FromObject(value ?? default(UInt16));
                case TypeId.Struct:
                    var structValues = value as IList<object>;
                    if (typeDefinition.Fields.Count != structValues.Count)
                    {
                        return JToken.FromObject($"Expected struct with {typeDefinition.Fields.Count} elements but got {structValues.Count}");
                    }
                    var returnStructList = new List<JToken>(structValues.Count);
                    for (int i = 0; i < structValues.Count; i++)
                    {
                        var structValueDefinition = typeDefinition.Fields[i];
                        var structValue = structValues[i];
                        returnStructList.Add(GetValueTypeAsJson(structValueDefinition, structValue, createTypeTemplate));
                    }
                    return JToken.FromObject(returnStructList);
                case TypeId.String:
                    // for template return "string", otherwise default value is null string
                    return JToken.FromObject(createTypeTemplate ? "string" : value ?? "\0");
                case TypeId.Uint64:
                    return JToken.FromObject(value ?? default(UInt64));
                case TypeId.Uint32:
                    return JToken.FromObject(value ?? default(UInt32));
                case TypeId.Variant:
                    var variant = value as AllJoynMessageArgVariant;
                    if (value != null) { MyVariant = value; }
                    if (createTypeTemplate)
                    {


                        JProperty variantDefinition = new JProperty("variant", "uint32");
                        JProperty variantValue = new JProperty("value", 1);
                        JObject jObject = new JObject(variantDefinition, variantValue);
                        return jObject;
                        // Variant can contain anything. Use simple string "variant" for template
                        //return GetValueTypeAsJson(AlljoynTypeDefinition.String, "variant", false);
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
                case TypeId.DoubleArray:
                case TypeId.Int32Array:
                case TypeId.Int16Array:
                case TypeId.Uint16Array:
                case TypeId.Uint64Array:
                case TypeId.Uint32Array:
                case TypeId.Int64Array:
                case TypeId.Uint8Array:
                case TypeId.StringArray:
                case TypeId.ObjectPathArray:
                    if (value == null) { return JToken.FromObject(new List<JToken>()); }
                    var arrayItems = value as IList<object>;
                    var returnArrayList = new List<JToken>(arrayItems.Count);
                    foreach (var s in arrayItems)
                    {
                        returnArrayList.Add(GetValueTypeAsJson(AlljoynTypeDefinition.TypeInstanceByArrayType(typeDefinition.Type), s,
                            createTypeTemplate));
                    }
                    return JToken.FromObject(returnArrayList);
                case TypeId.VariantArray:
                    break;
                case TypeId.SignatureArray:
                    break;
                case TypeId.StructArray:
                    //TestObjectType(value);
                    var fields = typeDefinition.Fields as IReadOnlyList<ITypeDefinition>;
                    if (fields == null)
                    {
                        throw new ArgumentNullException("typeDefinition must contains type fields type definition for StructArray");
                    }
                    var structArrayValues = value as IList<object>;
                    if (createTypeTemplate)
                    {
                        var structTemplateItem = new AllJoynMessageArgStructure(typeDefinition);
                        foreach (var field in fields)
                        {
                            structTemplateItem.Add(null);
                        }
                        structArrayValues = new List<object>() { structTemplateItem };
                    }
                    if (structArrayValues == null)
                    {
                        return JToken.FromObject("According to type definition value should be a StructArray but it is not!");
                    }
                    var returnList = new List<JToken[]>(structArrayValues.Count);
                    foreach (AllJoynMessageArgStructure structEntry in structArrayValues)
                    {
                        if (structEntry.Count != fields.Count)
                        {
                            return JToken.FromObject($"Got {structArrayValues.Count} values in struct entry, expected {fields.Count} ");
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
            throw new NotImplementedException($"typeDefinition.Type={typeDefinition.Type}");
        }

        /// <summary>
        /// Try to find out type (and if possible content) of given value (which usually is a n AllJoyn ComObject.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GuessObjectType(object value)
        {
            if (value == null)
            {
                return "null";
            }

            if (value is IList<object> list)
            {
                return "list of " + GuessObjectType(list.FirstOrDefault());
            }

            return value.GetType() + ": " + value.ToString();
        }

        private async void InvokeButton_Click(object sender, RoutedEventArgs e)
        {
            try
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
                        var guessingStringBuilder = new StringBuilder();
                        guessingStringBuilder.Append($"BAD RESPONSE.");
                        guessingStringBuilder.Append($" Got {result.Values.Count} return values, expected {VM.Method.OutSignature.Count}.");
                        guessingStringBuilder.AppendLine(" Try guessing return values:");
                        int i = 1;
                        foreach (var returnValue in result.Values)
                        {
                            guessingStringBuilder.AppendLine(i++ + " " + GuessObjectType(returnValue));
                        }
                        VM.MethodResult = guessingStringBuilder.ToString();
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
                        valueResultItems.Add(signature.Name + "_TypeDef", TypeDefinitionToString(signature.TypeDefinition));
                        valueResultItems.Add(signature.Name, valueAsJson);
                    }

                    VM.MethodResult = JToken.FromObject(valueResultItems).ToString();
                    return;
                }
            }
            catch (COMException ex)
            {
                var dialog = new Windows.UI.Popups.MessageDialog("This usually means that receiver expected a different data type for" +
                    " some parameter which can easily happen for Variant types which can contain any data type. Please try another." +
                    "\r\n\r\n" + ex,
                    $"Receiver failed to handle request");
                var asyncNoWait = dialog.ShowAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    System.Diagnostics.Debugger.Break();
                }
            }
        }

        private JToken TypeDefinitionToString(ITypeDefinition typeDefinition)
        {
            string dictInfo = null;
            if (typeDefinition.KeyType != null && typeDefinition.ValueType != null)
            {
                dictInfo = $"({TypeDefinitionToString(typeDefinition.KeyType)}->{TypeDefinitionToString(typeDefinition.ValueType)})";
            }
            string fieldsInfo = null;
            if (typeDefinition.Fields != null)
            {
                fieldsInfo = $"(Fields:[{typeDefinition.Fields.Count}]"
                    + string.Join(",", typeDefinition.Fields.Select(f => TypeDefinitionToString(f))) + ")";
            }
            return typeDefinition.Type.ToString() + dictInfo + fieldsInfo;
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
                    object parameterAsObject = GetValueAsObject(inSignature.TypeDefinition, parameter, inSignature.Name);
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
            }
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
