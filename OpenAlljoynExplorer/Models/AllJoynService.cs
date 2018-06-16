using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using DeviceProviders;
using OpenAlljoynExplorer.Support;
using Shared.Support;
using VariableItemListView.Models;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace OpenAlljoynExplorer.Models
{
    public class AllJoynService : Bindable
    {
        public AllJoynService(IService service)
        {
            Service = service;
            VariableListViewModel = new VariableListViewModel();
        }

        public IService Service { get; set; }

        public BitmapImage Icon
        {
            get { return Get<BitmapImage>(); }
            set { Set(value); }
        }

        public object PropertyMap { get; private set; }

        internal async Task ReadIconAsync()
        {
            VariableListViewModel.Items.Add(new VariableType(1));
            var icon = await Service.AboutData.GetIconAsync();
            var iconBytes = icon?.Content?.ToArray();
            if (iconBytes != null)
            {
                await Dispatcher.Dispatch(async () =>
                {
                    Icon = new BitmapImage();
                    using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
                    {
                        await stream.WriteAsync(iconBytes.AsBuffer());
                        stream.Seek(0);
                        await Icon.SetSourceAsync(stream);
                    }
                });
            }
        }

        internal async Task ReadAllAsync()
        {
            //var t = (Service as IService).GetType();
            //var fields = t.GetFields();
            //var props = t.GetProperties();
            //var mems = t.GetMembers();
            //var runprops = t.GetRuntimeProperties();
            //props = props;
            //var mem2 = typeof(IService).GetMembers();
            ////var mem2 = typeof(IService).GetFields

            //MemberInfo test = mem2.FirstOrDefault();
            var propertyReader = new PropertyReader {
                PropertyMap = PropMap,
                Out = VariableListViewModel
            };
            propertyReader.Read(Service, nameof(Service));

            //GetProps(parent: Service.AboutData.DeviceName, obj: Service, type: PropMap[nameof(Service)]);
            //Service.Objects.Count
            //var props2 = typeof(IService).GetProperties();
            //var props3 = Service.GetType().GetProperties();
            //foreach (var prop in props2)
            //{
            //    var n = prop.Name;
            //    var v = prop.GetValue(Service);
                
            //   //v.GetType()
            //    System.Diagnostics.Debug.WriteLine($" {Service.Name} {n} -> {v}");

            //}
            //props = props;
            //var s2 = (Service as IService).GetType().GetTypeInfo();
            //var s = (Service as IService).GetType().GetTypeInfo();
            ////Service.Name
            //var props = s.DeclaredProperties;
            //foreach (var prop in props)
            //{
            //    //prop.DeclaringType
            //    //prop.Name
            //    //prop.GetValue
            //}
        }

        bool IsSimple(object obj)
        {
            return obj == null || obj.GetType() == typeof(string);
        }

        Dictionary<string, Type> PropMap = new Dictionary<string, Type> {
            {"AboutData", typeof(IAboutData) },
            {"Provider", typeof(IProvider) },
            {"Objects", typeof(IList<IBusObject>) },
            {"ChildObjects", typeof(IList<IBusObject>) },
            {"Interfaces", typeof(IList<IInterface>) },
            {nameof(Service), typeof(IService) },
        };

        HashSet<int> KnownObjects = new HashSet<int>();

        private void GetProps(string parent, object obj, Type type = null)
        {
            if (IsSimple(obj)) return;
            var hash = obj.GetHashCode();
            if (KnownObjects.Contains(hash)) return;
            KnownObjects.Add(hash);
            
            PropertyInfo[] props;
            if(type != null) 
                props = type.GetProperties();
            else
             props= obj.GetType().GetProperties();
            //var ddv = typeof(IEnumerable<IBusObject>).IsAssignableFrom(type);
            //if(type != null && type.IsGenericType)
            //{
            //    int ads = 3;
            //}
            
            //pp.get
            foreach (Type interfaceType in obj.GetType().GetInterfaces())
            {
                var countProperty = interfaceType.GetProperty("Count");
                int df = 34;
            }

            var info = type?.GetTypeInfo();
            if (info != null && info.IsGenericType)
            {
                if (info.GetGenericTypeDefinition() == typeof(IList<>))
                {
                    //var pp = typeof(IList<>).GetType().GetTypeInfo().DeclaredProperties;
                    //var pp2 = typeof(IList<>).GetType().GetProperties();
                    //var pp3 = typeof(IEnumerable<>).GetType().GetMembers().Where(m => m.ToString().Contains("num"));
                    
              //      if (method != null)
              //      {
              ////          method = method.MakeGenericMethod(typeof(IBusObject));
              //          var result = method.Invoke(obj, null);

              //      }
                    
                    var listItemType = info.GenericTypeArguments[0];

                    var collection = typeof(ICollection<>).MakeGenericType(listItemType);
                    var cc2 = collection.GetProperty("Count");
                    //var cc = typeof(ICollection<IBusObject>).GetProperty("Count");
                    //var method = typeof(ICollection<IBusObject>).GetMethod("get_Count");
                    var coun22t = (int)cc2?.GetValue(obj);

                    for (int i = 0; i < coun22t; i++)
                    {
                        try
                        {
                            
                            var listItem = info.GetDeclaredMethod("get_Item").Invoke(obj, new object[] { i });
                            var oo = listItem as IBusObject;
                            GetProps(parent + "[" + i + "]", listItem, listItemType);
                        } catch(TargetInvocationException ex)
                        {
                            if (ex.InnerException is ArgumentOutOfRangeException)
                                break;
                            //x.HResult;
                        }
                    }
                    //info.DeclaredProperties.FirstOrDefault().GetValue(obj);
                    
                    //var listDef = typeof(List<>);
                    //var listStr = listDef.MakeGenericType(genType);

                    //dynamic changedObj = Convert.ChangeType(obj, listStr);

                    //var list = obj as listStr.get;
                }
                var genP = info.GenericTypeParameters;
                //var genType = info.GetGenericTypeDefinition();
            }
            else if (type != null && type.IsArray) {
                Array array = (Array)obj;
                var elementType = type.GetElementType();
                int count = 0;
                foreach (object o in array)
                {
                    //System.Diagnostics.Debug.WriteLine(o.ToString());
                    GetProps(parent + "[" + count++ + "]", o, elementType);
                    
                }
            }else { 
            foreach (var prop in props)
            {
                var n = prop.Name;
                object v;
                
                if (type != null)
                    v =prop.GetValue(obj);
                else
                    v =prop.GetValue(obj);
                
                if(v != null && v.ToString() != "System.__ComObject")
                    System.Diagnostics.Debug.WriteLine($" {parent} {n} -> {v}");

                if(PropMap.TryGetValue(n, out Type childType))
                //if(n.Equals("AboutData"))
                    GetProps(parent+"."+n, v, childType);
                else
                    GetProps(parent + "." + n, v);

            }
            }
        }

        public VariableListViewModel VariableListViewModel
        {
            get { return Get<VariableListViewModel>(); }
            set { Set(value); }
        }
    }
}
