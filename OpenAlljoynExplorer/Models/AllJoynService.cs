using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using DeviceProviders;
using OpenAlljoynExplorer.Support;
using Shared.Support;
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
            //await service.AboutData.GetIconAsync();
            //var getUrl = service.AboutData.GetIconAsync();
            //var task = getUrl.AsTask();
            //var icon = task.Result;
            //var bytes = icon?.Content?.ToArray();
            //var url= icon?.Url;



        }

        public IService Service { get; set; }

        public BitmapImage Icon
        {
            get { return Get<BitmapImage>(); }
            set { Set(value); }
        }

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
    }
}
