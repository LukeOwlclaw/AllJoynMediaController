using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using DeviceProviders;
using OpenAlljoynExplorer.Models;
using OpenAlljoynExplorer.Support;
using Windows.UI.Xaml.Controls;

namespace OpenAlljoynExplorer.Controllers
{
    /// <summary>
    /// Detects all available AllJoyn Services.
    /// </summary>
    public class AllJoynController : IDisposable
    {
        private AllJoynModel VM;
        private readonly Frame mNavigationFrame;

        public AllJoynController(AllJoynModel VM, Frame frame)
        {
            this.VM = VM;
            mNavigationFrame = frame;
        }

        private DeviceProviders.AllJoynProvider p;

        internal void Start()
        {
            //Task mytask = Task.Run(() =>
            //{
            p = new DeviceProviders.AllJoynProvider();
            p.ServiceJoined += ServiceJoined;
            p.ServiceDropped += ServiceDropped;
            p.Start();
            //});

        }

        private void ServiceDropped(IProvider sender, ServiceDroppedEventArgs args)
        {
            //Task mytask = Task.Run(() =>
            //{
            if (args != null)
            {
                //ReleaseServiceComObject(args.Service);
            }
            //});

        }


        private void ServiceJoined(IProvider sender, ServiceJoinedEventArgs args)
        {
            /// Try to create and keep own object (MyService) and release AllJoyn service ComObjects (args.Service)
            /// --> No effect though. 
            /// Some interfaces are still null. 
            /// To be precise: Only one AllJoyn device offers its interfaces, for all other devices interfaces is null.
            /// This behavior is deterministic. Given the same set of AllJoyn devices always the same device can be used.
            /// Order of detection does not matter. Maybe it is the alphabetic order of some property which matters?
            /// d
            /// Note: While using these dummy objects does not allow invoking AllJoyn actions, it seems to be good
            /// to dispose the ComObjects. OpenAlljoynExplorer hangs/freezes less often than with the ComObjects.
            /// Most important, the warning below about "AllJoyn device simulator" does not apply when using 
            /// own objects. (The disposing of the ComObjects is not mandatory!)
            /// 
            //var asyncOkay10 = Dispatcher.Dispatch(() =>
            //{
            //    try
            //    {
            //        var service = new AllJoynService(new MyService(args.Service));
            //        VM.AllJoynServices.Add(service);
            //    }
            //    catch (Exception ex)
            //    {
            //        System.Diagnostics.Debug.WriteLine(ex);
            //    }
            //});
            //ReleaseServiceComObject(args.Service);
            //return;

            // Warning! If there are too many devices or a bad device, OpenAlljoynExplorer will freeze on refreshing.
            // MainThread hangs in external code. Probably DeviceProvider is misbehaving. Not sure though.
            // This always happens when "AllJoyn device simulator" is mocking a device.
            Task mytask = Task.Run(() =>
            {
                AllJoynService service = null;
                try
                {
                    service = new AllJoynService(args.Service);
                    var asyncOkay1 = Favorite.SetAvailableFavorite(VM.Favorites, service);

                    var asyncOkay2 = service.ReadIconAsync();
                    //service.ReadAll();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                    return;
                }

                var asyncOkay3 = Dispatcher.Dispatch(() =>
                {
                    try
                    {
                        VM.AllJoynServices.Add(service);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex);
                    }
                });
            });

        }

        public void Dispose()
        {
            if (p != null)
            {
                foreach (var service in p.Services)
                {
                    ReleaseServiceComObject(service);
                }
            }
        }

        /// <summary>
        /// Try releasing ComObject IService.
        /// This is a mess, however, because IService is a recursive structure.
        /// </summary>
        /// <param name="service"></param>
        private void ReleaseServiceComObject(IService service)
        {
            if (service.Provider.Services != null)
            {
                foreach (var s in service.Provider.Services)
                {
                    Marshal.ReleaseComObject(s);
                }
            }

            Marshal.ReleaseComObject(service.Provider);
            //service.AboutData.GetAllFields().Where(f => f.Key is ComObject
            Marshal.ReleaseComObject(service.AboutData);
            if (service.Objects != null)
            {
                foreach (var o in service.Objects)
                {
                    if (o.Service != null)
                    {
                        Marshal.ReleaseComObject(o.Service);
                    }

                    if (o.ChildObjects != null)
                    {
                        foreach (var child in o.ChildObjects)
                        {
                            Marshal.ReleaseComObject(child);
                        }
                    }

                    if (o.Interfaces != null)
                    {
                        foreach (var i in o.Interfaces)
                        {
                            Marshal.ReleaseComObject(i.BusObject);
                            if (i.Signals != null)
                            {
                                foreach (var signal in i.Signals)
                                {
                                    Marshal.ReleaseComObject(signal.Interface);
                                    if (signal.Signature != null)
                                    {
                                        foreach (var sig in signal.Signature)
                                        {
                                            Marshal.ReleaseComObject(sig);
                                        }
                                    }

                                    Marshal.ReleaseComObject(signal);
                                }
                            }

                            if (i.Methods != null)
                            {
                                foreach (var method in i.Methods)
                                {
                                    Marshal.ReleaseComObject(method.Interface);

                                    if (method.OutSignature != null)
                                    {
                                        foreach (var outSig in method.OutSignature)
                                        {
                                            ReleaseTypeDefinitionComObject(outSig.TypeDefinition);
                                            Marshal.ReleaseComObject(outSig);
                                        }
                                    }

                                    if (method.InSignature != null)
                                    {
                                        foreach (var inSig in method.InSignature)
                                        {
                                            ReleaseTypeDefinitionComObject(inSig.TypeDefinition);
                                            Marshal.ReleaseComObject(inSig);
                                        }
                                    }

                                    Marshal.ReleaseComObject(method);
                                }
                            }
                            if (i.Properties != null)
                            {
                                foreach (var p in i.Properties)
                                {
                                    Marshal.ReleaseComObject(p.Interface);
                                    ReleaseTypeDefinitionComObject(p.TypeInfo);
                                    Marshal.ReleaseComObject(p);
                                }
                            }
                            Marshal.ReleaseComObject(i);
                        }
                    }
                    Marshal.ReleaseComObject(o);
                }
            }

            Marshal.ReleaseComObject(service);
        }

        private void ReleaseTypeDefinitionComObject(ITypeDefinition typeDefinition)
        {
            if (typeDefinition.ValueType != null)
            {
                Marshal.ReleaseComObject(typeDefinition.ValueType);
            }
            if (typeDefinition.KeyType != null)
            {
                Marshal.ReleaseComObject(typeDefinition.KeyType);
            }
            if (typeDefinition.Fields != null)
            {
                foreach (var field in typeDefinition.Fields)
                {
                    ReleaseTypeDefinitionComObject(field);
                }
            }
            Marshal.ReleaseComObject(typeDefinition);
        }
    }
}