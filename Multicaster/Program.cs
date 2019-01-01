using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Multicaster
{
    class Program
    {
        /// <summary>
        /// This method prints usage information for multicast sample
        /// </summary>
        static void usage()
        {
            Console.WriteLine("Executable_file_name [/i ip] [/b ip] [/m ip] [/p port] [/x num] [/t text]");
            Console.WriteLine("              [/n count] [/s | /r]");
            Console.WriteLine("Available options:");
            Console.WriteLine("   /i ip                Local interface to join multicast group(s) on");
            Console.WriteLine("   /b ip               Local IP to bind the socket to");
            Console.WriteLine("   /m ip              Multicast group to join");
            Console.WriteLine("   /p port           Port to listen on or send to");
            Console.WriteLine("   /x size           Length of send or receive buffer");
            Console.WriteLine("   /t text Text message to put in send buffer");
            Console.WriteLine("   /n count  Number of times to send or receive");
            Console.WriteLine("   /s        Send data to a multicast group");
            Console.WriteLine("   /r                    Receive data send to the group");
            Console.WriteLine();
        }
        /// <summary>
        /// This is the main routine which parses the command line and creates the
        /// multicast socket as given by the command line parameters. The sample
        /// can either be invoked as a multicast sender or receiver.
        ///
        /// To invoke as a multicast receiver:
        ///     multicast.exe /r /i 10.10.10.1 /m 234.6.6.6
        /// To invoke as a multicast sender:
        ///     multicast.exe /s /i 10.10.10.99 /m 234.6.6.6
        /// </summary>
        /// <param name="appArguments">Command line arguments</param>
        static void Main(string[] appArguments)
        {
            //ListenToPort(appArguments[0]);

            // Listen for mDNS on network interface
            //var listenMdns = "/r /i 192.168.81.58 /m 224.0.0.251 /p 5353";
            // Listen for mDNS on loop back interface
            //var listenMdns = "/r /i 127.0.0.1 /m 224.0.0.251 /p 5353";
            //appArguments = listenMdns.Split(' ');

            // Send test message to mDNS
            var search = "/s /i 127.0.0.1 /m 224.0.0.251 /p 5353 /x 32 /t \"test_message\"";
            appArguments = search.Split(' ');

            // Open test port
            //var listen = "/r /i 192.168.81.58 /m 224.0.0.113 /p 9956";
            //appArguments = listen.Split(' ');

            // Send message on test port
            //var search = "/s /i 192.168.81.58 /m 224.0.0.113 /p 9956 /x 32 /t \"test_message\"";
            //appArguments = search.Split(' ');




            MulticastEndpoint mcastEndpoint = new MulticastEndpoint();
            string textMessage = "Hello Bastard World";
            int port = 0, bufferLength = 1024, opCount = 10;
            bool isSender = true;
            // Parse the command line
            for (int i = 0; i < appArguments.Length; i++)
            {
                try
                {
                    if ((appArguments[i][0] == '-') || (appArguments[i][0] == '/'))
                    {
                        switch (Char.ToLower(appArguments[i][1]))
                        {
                            case 'b':
                                // Address to bind multicast socket to
                                mcastEndpoint.bindAddress = IPAddress.Parse(appArguments[++i]);
                                break;
                            case 'c':
                                // How many times to send or receive
                                opCount = System.Convert.ToInt32(appArguments[++i].ToString());
                                break;
                            case 'i':
                                // Local interface to join group(s) on
                                mcastEndpoint.localInterfaceList.Add(IPAddress.Parse(appArguments[++i]));
                                break;
                            case 'm':
                                // Multicast group to join
                                mcastEndpoint.multicastJoinList.Add(IPAddress.Parse(appArguments[++i]));
                                break;
                            case 'p':
                                // Port number to bind to or send to
                                port = System.Convert.ToInt32(appArguments[++i].ToString());
                                break;
                            case 'r':
                                // Application invoked as receiver
                                isSender = false;
                                break;
                            case 's':
                                // Application invoked as sender
                                isSender = true;
                                break;
                            case 't':
                                // Text message to send
                                textMessage = appArguments[++i];
                                break;
                            case 'x':
                                // Length of send/receive buffer
                                bufferLength = System.Convert.ToInt32(appArguments[++i].ToString());
                                break;
                            default:
                                usage();
                                return;
                        }
                    }
                }
                catch
                {
                    usage();
                    return;
                }
            }
            // Make sure user specified at least on multicast group and interface to join
            if ((mcastEndpoint.multicastJoinList.Count == 0) || (mcastEndpoint.localInterfaceList.Count == 0))
            {
                Console.WriteLine("Please specify a multicast group and interface to join on!");
                usage();
                return;
            }
            // Now, create the multicast socket
            Console.WriteLine("Creating the multicast socket...");
            try
            {
                if (isSender == true)
                {
                    // For the sender, we don't care what the local port the socket is bound to
                    mcastEndpoint.Create(
                        0,               // If the sender we don't care what local port we bind to
                        bufferLength
                        );
                }
                else
                {
                    mcastEndpoint.Create(port, bufferLength);
                }
                if (isSender == true)
                {
                    IPEndPoint destinationGroup = new IPEndPoint((IPAddress)mcastEndpoint.multicastJoinList[0], port);
                    int rc;
                    // Set the send interface for all outgoing multicast packets
                    Console.WriteLine("Setting the send interface for all outgoing multicast packets...");
                    mcastEndpoint.SetSendInterface((IPAddress)mcastEndpoint.localInterfaceList[0]);
                    mcastEndpoint.FormatBuffer(textMessage);
                    // Send the message the requested number of times
                    Console.WriteLine("Sending the message the requested number of times...");
                    for (int i = 0; i < opCount; i++)
                    {
                        try
                        {
                            rc = mcastEndpoint.mcastSocket.SendTo(mcastEndpoint.dataBuffer, destinationGroup);
                            Console.WriteLine("Multicast SendTo() is OK...");
                            Console.WriteLine("Sent {0} bytes to {1}", rc, destinationGroup.ToString());
                        }
                        catch (SocketException err)
                        {
                            Console.WriteLine("Multiast SendTo() failed: " + err.Message);
                        }
                    }
                }
                else
                {
                    IPEndPoint senderEndPoint = new IPEndPoint((IPAddress)mcastEndpoint.localInterfaceList[0], 0);
                    EndPoint castSenderEndPoint = (EndPoint)senderEndPoint;
                    int rc;
                    for (int i = 0; i < opCount; i++)
                    {
                        try
                        {
                            rc = mcastEndpoint.mcastSocket.ReceiveFrom(mcastEndpoint.dataBuffer, ref castSenderEndPoint);
                            Console.WriteLine("Multicast ReceiveFrom() is OK...");
                            senderEndPoint = (IPEndPoint)castSenderEndPoint;
                            Console.WriteLine("Received {0} bytes from {1}: '{2}'",
                                rc,
                                senderEndPoint.ToString(),
                                System.Text.Encoding.ASCII.GetString(mcastEndpoint.dataBuffer, 0, rc)
                                );
                        }
                        catch (SocketException err)
                        {
                            Console.WriteLine("Multicast ReceiveFrom() failed: " + err.Message);
                        }
                    }
                }
                // Drop membership to groups. This isn't required in this sample as closing the socket
                //    will implicitly leave all groups joined, but it's a good practice.
                mcastEndpoint.LeaveGroups();
            }
            catch
            {
                Console.WriteLine("An error occurred creating the multicast socket");
            }
            finally
            {
                if (mcastEndpoint.mcastSocket != null)
                {
                    Console.WriteLine("Closing the multicast socket...");
                    mcastEndpoint.mcastSocket.Close();
                }
                mcastEndpoint = null;
            }
        }

        private static void ListenToPort(string v)
        {
            // Note: Multicast port 5353 is mDNS; used to find AllJoyn devices. Listen for hello and goodbye messages, as well as for search messages.
            // Multicast port 9956 is used by AllJoyn to send status information (signalling events: begin/stop playing, etc.
            // Port 9955 is "contact port". P2P, no multicast.
            int port = int.Parse(v);
            IPEndPoint localpt = new IPEndPoint(IPAddress.Any, port);
            UdpClient udpListener = new UdpClient();
            udpListener.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udpListener.Client.Bind(localpt);

            //Creates an IPEndPoint to record the IP Address and port number of the sender. 
            // The IPEndPoint will allow you to read datagrams sent from any source.
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            do
            {
                try
                {
                    Console.WriteLine($"Listening on port {port}...");
                    // Blocks until a message returns on this socket from a remote host.
                    Byte[] receiveBytes = udpListener.Receive(ref RemoteIpEndPoint);

                    string returnData = Encoding.ASCII.GetString(receiveBytes);

                    Console.WriteLine("This is the message you received " +
                                                 returnData.ToString());
                    Console.WriteLine("This message was sent from " +
                                                RemoteIpEndPoint.Address.ToString() +
                                                " on their port number " +
                                                RemoteIpEndPoint.Port.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

            } while (true);
        }
    }
}
