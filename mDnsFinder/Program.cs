using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tmds.MDns;

namespace mDnsFinder
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var types = new string[] { "_workstation._tcp", "_http._tcp", "_ssh._tcp", "_afpovertcp._tcp", "_device-info._tcp",
                "_dacp._tcp", "_daap._tcp", "_alljoyn._tcp", "_dns-sd._udp", "_airplay._tcp", "_raop._tcp" };
            types = new string[] {"_alljoyn._tcp" };

            if (args.Length >= 1)
            {
                types = args;
            }

            ServiceBrowser serviceBrowser = new ServiceBrowser();
            serviceBrowser.ServiceAdded += onServiceAdded;
            serviceBrowser.ServiceRemoved += onServiceRemoved;
            serviceBrowser.ServiceChanged += onServiceChanged;

            Console.WriteLine("Browsing for type(s): {0}", string.Join(", ", types));
            serviceBrowser.StartBrowse(types);
            Console.ReadLine();
        }

        private static void onServiceChanged(object sender, ServiceAnnouncementEventArgs e)
        {
            printService('~', e.Announcement);
        }

        private static void onServiceRemoved(object sender, ServiceAnnouncementEventArgs e)
        {
            printService('-', e.Announcement);
        }

        private static void onServiceAdded(object sender, ServiceAnnouncementEventArgs e)
        {
            printService('+', e.Announcement);
        }

        private static void printService(char startChar, ServiceAnnouncement service)
        {
            Console.WriteLine("{0} '{1}' on {2}", startChar, service.Instance, service.NetworkInterface.Name);
            Console.WriteLine("\tHost: {0} ({1})", service.Hostname, string.Join(", ", service.Addresses));
            Console.WriteLine("\tPort: {0}", service.Port);
            Console.WriteLine("\tType: {0}", service.Type);
            Console.WriteLine("\tTxt : [{0}]", string.Join(", ", service.Txt));
        }
    }
}
