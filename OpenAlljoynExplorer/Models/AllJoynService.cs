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
            // Define all properties/fields which we want PropertyReader to read
            Dictionary<string, Type> PropMap = new Dictionary<string, Type> {
                {"AboutData", typeof(IAboutData) },
                {"Provider", typeof(IProvider) },
                {"Objects", typeof(IList<IBusObject>) },
                {"ChildObjects", typeof(IList<IBusObject>) },
                {"Interfaces", typeof(IList<IInterface>) },
                //{"Services", typeof(IList<IService>) }, // Services in Provider
                {nameof(Service), typeof(IService) },
            };

            var propertyReader = new PropertyReader
            {
                PropertyMap = PropMap,
                Out = VariableListViewModel
            };

            propertyReader.Read(Service, nameof(Service));
        }

        public VariableListViewModel VariableListViewModel
        {
            get { return Get<VariableListViewModel>(); }
            set { Set(value); }
        }
    }
}
