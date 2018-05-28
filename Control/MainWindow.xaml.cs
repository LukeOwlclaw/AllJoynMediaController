using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Control
{

    using System;
    using System.Threading.Tasks;
    using Windows.Devices.AllJoyn;
    using Windows.Foundation;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MyClass c = new MyClass();
            c.Run();
           // DeviceProviders.AllJoynProvider p = new DeviceProviders.AllJoynProvider();
            //p.ServiceJoined += ServiceJoined;
            //p.ServiceDropped += ServiceDropped;
            //p.Start();
        }
        class MyClass
        {
            public void  Run()
            {
                //DeviceProviders.AllJoynProvider p = new DeviceProviders.AllJoynProvider();
                //p.ServiceJoined += ServiceJoined;
                //p.ServiceDropped += ServiceDropped;
                //p.Start();
            }

            public void Run2()
            {
                var busAttachment = new AllJoynBusAttachment();

                busAttachment.AuthenticationMechanisms.Add(AllJoynAuthenticationMechanism.SrpAnonymous);
      
            }


        }

        //private void ServiceDropped(DeviceProviders.IProvider sender, DeviceProviders.ServiceDroppedEventArgs args)
        //{
        //    var name = args.Service.AboutData?.DeviceName;
        //    var id = args.Service.AboutData?.DeviceId;
        //    System.Diagnostics.Debug.WriteLine($"Lost device '{name}' : ID = {id}");
        //}

        //private void ServiceJoined(DeviceProviders.IProvider sender, DeviceProviders.ServiceJoinedEventArgs args)
        //{
        //    var name = args.Service.AboutData?.DeviceName;
        //    var id = args.Service.AboutData?.DeviceId;
        //    System.Diagnostics.Debug.WriteLine($"Found device '{name}' : ID = {id}");
        //}
    }
}
