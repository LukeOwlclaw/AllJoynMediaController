using DeviceProviders;
using OpenAlljoynExplorer.Controllers;
using OpenAlljoynExplorer.Models;
using OpenAlljoynExplorer.Support;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
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
            //Controller = new ServicePageController(VM);
            //Controller.Start();

            ReadAll();

            //foreach (var inParameter in VM.InSignature)
            //{
            //    //inParameter.Name;
            //    inParameter.TypeDefinition.
            //}
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            BackButton.IsEnabled = this.Frame.CanGoBack;
            //VM = new MethodModel { Method = (IMethod)e.Parameter };
            VM = (MethodModel)e.Parameter;
        }

        private async Task<IEnumerable<Assembly>> GetAssemblyListAsync()
        {
            var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;

            List<Assembly> assemblies = new List<Assembly>();
            foreach (Windows.Storage.StorageFile file in await folder.GetFilesAsync())
            {
                try
                {
                    if (file.FileType == ".dll" || file.FileType == ".exe")
                    {
                        AssemblyName name = new AssemblyName()
                        {
                            Name = Path.GetFileNameWithoutExtension(file.Name)
                        };
                        Assembly asm = Assembly.Load(name);
                        assemblies.Add(asm);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }

            return assemblies;
        }

        private async Task<Type> FindInterface(object unknown)
        {
            try
            {
                var assemblies = await GetAssemblyListAsync();
                foreach (Assembly a in assemblies)
                {
                    Type wantedType = FindInterfaceForAssembly(unknown, a);
                    if (wantedType != null)
                        return wantedType;

                }
            }
            catch (Exception) { }
            return null;
        }

        private Type FindInterfaceForAssembly(object unknown, Assembly interopAssembly)
        {
            Func<Type, bool> implementsInterface = iface =>
            {
                try
                {
                    Marshal.GetComInterfaceForObject(unknown, iface);
                    return true;
                }
                catch (InvalidCastException)
                {
                    return false;
                }
            };

            List<Type> supportedInterfaces = interopAssembly.
                GetTypes().
                Where(t => t.GetTypeInfo().IsInterface).
                Where(implementsInterface).
                ToList();

            return supportedInterfaces.FirstOrDefault();
        }

        //public static unsafe byte[] Binarize(object obj, int size)
        //{
        //    var r = new byte[size];
        //    var rf = __makeref(obj);
        //    var a = **(IntPtr**)(&rf);
        //    Marshal.Copy(a, r, 0, size);
        //    return res;
        //}

        private async void BackButton_Click(object sender, RoutedEventArgs e)
        {
            On_BackRequested();
            return;

        }

        private Type GetType2(object item, Type t)
        {

            var info = t.GetTypeInfo();
            if (info.IsAbstract)
                return null;
            Type usedType;
            if (info.IsGenericType != info.IsGenericParameter)
            {
                int b = 3;
                //continue;
            }
            if (info.FullName == "OpenAlljoynExplorer.App")
                return null;
            try
            {
                if (info.IsGenericTypeDefinition)
                {
                    Type[] genericTypes = new Type[info.GenericTypeParameters.Length];
                    for (int i = 0; i < genericTypes.Length; i++)
                    {
                        genericTypes[i] = typeof(object);
                    }
                    usedType = t.MakeGenericType(genericTypes);
                }
                else
                {
                    usedType = t;
                }

                dynamic comClassInstance = Activator.CreateInstance(usedType);
                comClassInstance.ComClassMethod();
                var test = comClassInstance.ComClassFMethod(item);
                if (test != null)
                {
                    return usedType;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

            }
            return null;
        }
        private async Task<Type> GetType(object item)
        {
            var assemblies = await GetAssemblyListAsync();
            foreach (Assembly a in assemblies)
            {
                foreach (Type t in a.GetTypes())
                {
                    Type wantedType = GetType2(item, t);
                    if (wantedType != null)
                        return wantedType;
                }
            }
            return null;
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
            var objectPath  = VM.Interface.BusObject.Path;
            var interfaceName = VM.Interface.Name;
            var methodName = VM.Method.Name;

            var favorite = new Favorite {
                DeviceId = VM.Service.AboutData.DeviceId,
                ObjectPath = VM.Interface.BusObject.Path,
                InterfaceName = VM.Interface.Name,
                MethodName = VM.Method.Name
            };

            Favorite.Add(favorite);

        }
    }
}
