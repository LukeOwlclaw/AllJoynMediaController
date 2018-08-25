using System;
using System.Runtime.InteropServices;
using System.Reflection;
using MediaControl.Support;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DeviceProviders;
using MediaControl.Models;
using System.Runtime.Serialization;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MediaControl
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            DeviceProviders.AllJoynProvider p = new DeviceProviders.AllJoynProvider();
            p.ServiceJoined += ServiceJoined;
            p.ServiceDropped += ServiceDropped;
            p.Start();

            //var bus = new BusAttachment("ServiceTest", true);

        }

        private void ServiceDropped(DeviceProviders.IProvider sender, DeviceProviders.ServiceDroppedEventArgs args)
        {
            var name = args.Service.AboutData?.DeviceName;
            var id = args.Service.AboutData?.DeviceId;
            System.Diagnostics.Debug.WriteLine($"Lost device '{name}' : ID = {id}");
        }

        [DataContract]
        [StructLayout(LayoutKind.Sequential)]
        struct TestStruct
        {
            public string A;
        }

        Object ServiceJoinedLock = new object();
        private void ServiceJoined(DeviceProviders.IProvider sender, DeviceProviders.ServiceJoinedEventArgs args)
        {
            lock (ServiceJoinedLock)
            {
                //var serviceName = args.Service.Name;
                //var services = args.Service.Provider.Services;
                //var t =services.GetType();
                //var props = t.GetRuntimeProperties();
                //var methods= t.GetRuntimeMethods();
                //var fields= t.GetRuntimeFields();
                //var events = t.GetRuntimeEvents();

                //t = args.Service.AboutData.GetType();
                //props = t.GetRuntimeProperties();
                //methods = t.GetRuntimeMethods();
                //fields = t.GetRuntimeFields();
                //events = t.GetRuntimeEvents();

                //var s2 = DispatchUtility.GetType(services, true);
                var name = args.Service.AboutData?.DeviceName;
                var id = args.Service.AboutData?.DeviceId;
                System.Diagnostics.Debug.WriteLine($"Found device '{name}' : ID = {id}");

                foreach (var obj in args.Service.Objects)
                {
                    var path = obj.Path;
                    if (!path.Contains("Playlist"))
                        continue;
                    p($"service {path}");
                    foreach (var i in obj.Interfaces)
                    {
                        if (!i.Name.Contains("Playlist"))
                            continue;
                        var getRange = i.GetMethod("GetRange");
                        var getRangeIn = getRange.InSignature;
                        foreach (var inSig in getRangeIn)
                        {
                            var s = inSig.Name;
                        }
             
                        try
                        {
                            TestStruct t = new TestStruct();
                            t.A = "a";
                            var updateListOp = getRange.InvokeAsync(new List<object> { "\0", 0, 1 });
                            var getRangeTask = updateListOp.AsTask().GetAwaiter().GetResult();

                            var latestSnapshotId = getRangeTask.Values[0];
                            var totalSize = getRangeTask.Values[1];
                            var items = getRangeTask.Values[2] as IList<object>;
                            if (items == null)
                            {
                                if (items[0] is String errorString)
                                {
                                    throw new ArgumentException("Received playlist contains error: " + errorString);
                                }
                            }

                            var medias = items.Select(i2 => new Media(i2 as AllJoynMessageArgStructure));

                            //var m = new Media(items);
                            p($"getPlaylist : {getRangeTask.Status.StatusText}");
                        }
                        catch (Exception ex)
                        {
                            ex = ex;
                        }
                    }
                }

                
                        foreach (var obj in args.Service.Objects)
                {
                    var path = obj.Path;
                    if (!path.Contains("MediaPlayer"))
                        continue;
                    p($"service {path}");
                    foreach (var i in obj.Interfaces)
                    {
                        if (!i.Name.Contains("MediaPlayer"))
                            continue;
                        System.Diagnostics.Debug.WriteLine($"  Interface '{i.Name}'");
                        var properties = i.Properties;
                        //foreach (var prop in properties)
                        //{
                        //    p($"    prop {prop.Name}");
                        //}
                        var methods = i.Methods;
                        //foreach (var m in methods)
                        //{
                        //    p($"    method {m.Name}");
                        //}
                        //var events = i.Signals;
                        //foreach (var e in events)
                        //{
                        //    p($"    event {e.Name}");
                        //}
                        //System.Diagnostics.Debug.WriteLine($"properties '{properties}' : ID = {id}");
                        var getPlaylist = i.GetMethod("GetPlaylist");
                        var outSig = getPlaylist.OutSignature as IList<DeviceProviders.ParameterInfo>;
                        foreach (var para in outSig)
                        {
                            var paraInfo = para as DeviceProviders.ParameterInfo;
                            p($"out : {paraInfo.Name} type: {paraInfo.TypeDefinition.Type}");
                            if (paraInfo.TypeDefinition.Type == TypeId.StructArray)
                            {
                                foreach (var field in paraInfo.TypeDefinition.Fields)
                                {
                                    p($"  field {field.Type}");
                                    if (field.Type == TypeId.Dictionary)
                                        p($"    dict {field.KeyType.Type} -> {field.ValueType.Type}");
                                }
                            }
                        }


                        var getListOp= getPlaylist.InvokeAsync(new List<object> { });
                        var getListTask = getListOp.AsTask().GetAwaiter().GetResult();
                        p($"getPlaylist : {getListTask.Status.StatusText}");
                        var pl = new Playlist(getListTask.Values);

                        var items = getListTask.Values[0]; 
                        var controllerType = getListTask.Values[1];
                        var playlistUserData = getListTask.Values[2];
                        
                        //var t = items as AllJoynMessageArgStructure;
                        //var media = new Media(t);

                        var updatePlaylist = i.GetMethod("UpdatePlaylist");
                        var inSig = updatePlaylist.InSignature as IList<DeviceProviders.ParameterInfo>;
                        foreach (var para in inSig)
                        {
                            var paraInfo = para as DeviceProviders.ParameterInfo;
                            p($"in : {paraInfo.Name} type: {paraInfo.TypeDefinition.Type}");
                            if (paraInfo.TypeDefinition.Type == TypeId.StructArray)
                            {
                                foreach (var field in paraInfo.TypeDefinition.Fields)
                                {
                                    p($"  field {field.Type}");
                                    if (field.Type == TypeId.Dictionary)
                                        p($"    dict {field.KeyType.Type} -> {field.ValueType.Type}");
                                }
                            }
                        }
                        var setOutSig = updatePlaylist.OutSignature;
                        foreach (var para in setOutSig)
                        {
                            var paraInfo = para as DeviceProviders.ParameterInfo;
                            p($"out : {paraInfo.Name} type: {paraInfo.TypeDefinition.Type}");
                        }

                        //IList<object> test = new Dictionary<string, object>();
                        
                            var updateListOp = updatePlaylist.InvokeAsync(new List<object> { pl.ListToParameter(), 1, "ABC", "ABC" });
                        var updateListTask = updateListOp.AsTask().GetAwaiter().GetResult();
                        p($"getPlaylist : {updateListTask.Status.StatusText}");
                        var setResult = getListTask.Values;
                    }
                }
            }
        }

        private void p(string m)
        {
            System.Diagnostics.Debug.WriteLine(m);
        }
    }
}
