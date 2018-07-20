using System;
using System.Linq;
using DeviceProviders;
using OpenAlljoynExplorer.Models;
using OpenAlljoynExplorer.Support;
using Windows.UI.Xaml.Controls;

namespace OpenAlljoynExplorer.Controllers
{
    /// <summary>
    /// Detects all available AllJoyn Services.
    /// </summary>
    public class AllJoynController
    {
        private AllJoynModel VM;

        public AllJoynController(AllJoynModel VM)
        {
            this.VM = VM;
        }

        internal void Start()
        {
            DeviceProviders.AllJoynProvider p = new DeviceProviders.AllJoynProvider();
            p.ServiceJoined += ServiceJoined;
            p.ServiceDropped += ServiceDropped;
            p.Start();
        }

        private void ServiceDropped(IProvider sender, ServiceDroppedEventArgs args)
        {
            //throw new NotImplementedException();
        }


        private async void ServiceJoined(IProvider sender, ServiceJoinedEventArgs args)
        {
            AllJoynService service = null;
            try
            {
                service = new AllJoynService(args.Service);
                if (mNavigationActive)
                {
                    if (service.Service.AboutData.DeviceId == mNavigationDeviceId)
                    {
                        var navigationObject = service.Service.Objects.FirstOrDefault(o => o.Path == mNavigationObjectPath);
                        if (navigationObject != null)
                        {
                            var navigationInterface = navigationObject.Interfaces.FirstOrDefault(i => i.Name == mNavigationInterfaceName);
                            if (navigationInterface != null)
                            {
                                if (mNavigationMethodName != null)
                                {
                                    var method = navigationInterface.GetMethod(mNavigationMethodName);
                                    if (method != null)
                                    {
                                        await Dispatcher.Dispatch(() =>
                                        {
                                            mNavigationFrame.Navigate(typeof(Pages.MethodPage), method);
                                        });
                                    }
                                }
                                else
                                {
                                    await Dispatcher.Dispatch(() =>
                                    {
                                        mNavigationFrame.Navigate(typeof(Pages.InterfacePage), navigationInterface);
                                    });
                                }
                                return;
                            }
                        }
                    }
                }
                await service.ReadIconAsync();
                service.ReadAll();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return;
            }

            await Dispatcher.Dispatch(() =>
            {
                VM.AllJoynServices.Add(service);
            });
        }

        bool mNavigationActive = false;
        Frame mNavigationFrame;
        string mNavigationDeviceId;
        string mNavigationObjectPath;
        string mNavigationInterfaceName;
        string mNavigationMethodName;

        /// <summary>
        /// As soon as <see cref="Start"/> is called, looks for given interface and then navigates to it.
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="deviceId"></param>
        /// <param name="objectPath"></param>
        /// <param name="interfaceName"></param>
        internal void GoTo(Frame frame, string deviceId, string objectPath, string interfaceName, string methodName = null)
        {
            mNavigationActive = true;
            mNavigationFrame = frame;
            mNavigationDeviceId = deviceId;
            mNavigationObjectPath = objectPath;
            mNavigationInterfaceName = interfaceName;
            mNavigationMethodName = methodName;
            var dummyService = new AllJoynService(new DummyNavigationService());
            var asyncTask = Dispatcher.Dispatch(() =>
            {
                VM.AllJoynServices.Add(dummyService);
            });
        }
    }
}