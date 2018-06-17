# Open AllJoyn Explorer

Open source project to explorer [AllJoyn](https://openconnectivity.org/developer/reference-implementation/alljoyn) devices.

For getting started with AllJoyn, a working explorer is very helpful. Unfortunately, the [official one](https://www.microsoft.com/en-us/p/iot-explorer-for-alljoyn/9nblggh6gpxl) does not work well (program crashes and unable to send null-strings). And even more unfortunately it is [not (going to) open-source](https://github.com/ms-iot/samples/issues/158) to fix these problems. So I started working on my own AllJoynExplorer. Based on the [DeviceProviders](https://github.com/ms-iot/samples/tree/develop/AllJoyn/Platform/DeviceProviders) which are part of the official AllJoyn samples and made available by [mnielsen](https://www.nuget.org/profiles/mnielsen) as [NuGet package dotMorten.AllJoyn.DeviceProviders](https://www.nuget.org/packages/dotMorten.AllJoyn.DeviceProviders/). Thanks a lot!

This project is work-in-progress. And even when I am done with it and I am sure, it will not look nice. Feel free to chime in. 