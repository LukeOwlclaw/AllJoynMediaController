using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Multicaster;
using Tmds.MDns;

namespace AllPlayMediaPlayerDotNetCore
{
    internal class Program
    {
        static MulticastEndpoint mcastEndpoint;

        static int port = 5353;

        private static void Main(string[] args)
        {
            // Ignore spelling: Multicast 
            
            int bufferLength = 1024;
            int opCount = 10;
            

            Console.WriteLine("Hello World!");
            mcastEndpoint = new MulticastEndpoint();
            // Local interface to join group(s) on
            //mcastEndpoint.localInterfaceList.Add(IPAddress.Parse("192.168.81.58"));
            mcastEndpoint.localInterfaceList.Add(IPAddress.Parse("0.0.0.0"));
            // Multicast group to join
            mcastEndpoint.multicastJoinList.Add(IPAddress.Parse("224.0.0.251"));


            //var writer2 = new Tmds.MDns.DnsMessageWriter();
            //writer2.WriteRecordStart(RecordSection.Answer, new Name("XYZ"), RecordType.PTR, 10);
            //writer2.WriteRecordEnd();
            //IPEndPoint destinationPoint = new IPEndPoint(new IPAddress(new byte[] { 127,0,0,1}), 11000);
            ////IPEndPoint destinationGroup = new IPEndPoint((IPAddress)mcastEndpoint.multicastJoinList[0], port);
            mcastEndpoint.Create(port, bufferLength);
            //mcastEndpoint.mcastSocket.SendTo(writer2.Packets.First().Array, destinationPoint);


            IPAddress serverAddr = IPAddress.Parse("192.168.81.255");
            IPEndPoint endPoint = new IPEndPoint(serverAddr, 11000);
            string text = "Hello1";
            byte[] send_buffer = System.Text.Encoding.ASCII.GetBytes(text);
            for (int i = 0; i < 100; i++)
            {
                var writer3 = new Tmds.MDns.DnsMessageWriter();
                writer3.WriteQueryHeader(0x1235, new DnsMessageWriter.ResponseFlags {
                    Type = DnsMessageWriter.ResponseFlags.MessageType.Response,
                });
                string domainname = string.Concat(Enumerable.Repeat("a", 32));
                domainname = "AWESOME-ALLPLAY-PLAYER";
                // domain name must have length 32 !!
                writer3.WritePtrRecord(RecordSection.Answer, new Name("_alljoyn._tcp.local"), domainname, 120);
                //writer3.WriteRecordStart(RecordSection.Answer, new Name("_alljoyn._tcp.local"), RecordType.PTR, 120, domainname, RecordClass.Internet);
                writer3.WriteSrvRecord(RecordSection.Answer, 120, srvNameOffset: 0x2B, srvPriority: 1, srvWeigth: 1, srvPort: 9955,
                    srcTargetName: "ABCDEFABCDEF", srvTargetNamePtrOffset: 0x1A);
               //writer3.WriteRecordEnd();

                foreach (ArraySegment<byte> segment in writer3.Packets)
                {
                    mcastEndpoint.mcastSocket.SendTo(segment.Array, segment.Offset, segment.Count, SocketFlags.None, endPoint);
                }

                

            }

            //Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,ProtocolType.Udp);
            //IPAddress serverAddr = IPAddress.Parse("192.168.81.255");
            //IPEndPoint endPoint = new IPEndPoint(serverAddr, 11000);
            //string text = "Hello1";
            //byte[] send_buffer = System.Text.Encoding.ASCII.GetBytes(text);
            //for (int i = 0; i < 100; i++)
            //{
            //    //sock.SendTo(send_buffer, endPoint);
            //    mcastEndpoint.mcastSocket.SendTo(send_buffer, endPoint);

            //}


           
            return;

            try
            {
                mcastEndpoint.Create(port, bufferLength);

                IPEndPoint senderEndPoint = new IPEndPoint((IPAddress)mcastEndpoint.localInterfaceList[0], 0);
                EndPoint castSenderEndPoint = (EndPoint)senderEndPoint;
                int rc;
                for (int i = 0; i < opCount; i++)
                {
                    try
                    {
                        rc = mcastEndpoint.mcastSocket.ReceiveFrom(mcastEndpoint.dataBuffer, ref castSenderEndPoint);

                        var sendAnswer = IsAllJoynQuery(mcastEndpoint.dataBuffer);
                        
                        Console.WriteLine("Multicast ReceiveFrom() is OK...");
                        senderEndPoint = (IPEndPoint)castSenderEndPoint;
                        //Console.WriteLine("Received {0} bytes from {1}: '{2}'",
                        //    rc,
                        //    senderEndPoint.ToString(),
                        //    System.Text.Encoding.ASCII.GetString(mcastEndpoint.dataBuffer, 0, rc)
                        //    );

                        if (sendAnswer)
                        {
                            Console.WriteLine($"Receive AllJoyn query from {senderEndPoint}");
                            var writer = new Tmds.MDns.DnsMessageWriter();
                            writer.WriteRecordStart(RecordSection.Answer, new Name("XYZ"), RecordType.TXT, 10);
                            writer.WriteRecordEnd();
                            //IPEndPoint destinationGroup = new IPEndPoint((IPAddress)mcastEndpoint.multicastJoinList[0], port);
                            mcastEndpoint.mcastSocket.SendTo(writer.Packets.First().Array, senderEndPoint);
                            //mcastEndpoint.mcastSocket.Send(writer.Packets);
                        }
                    }
                    catch (SocketException err)
                    {
                        Console.WriteLine("Multicast ReceiveFrom() failed: " + err.Message);
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

        private static bool IsAllJoynQuery(byte[] dataBuffer)
        {
            try
            {
                DnsMessageReader reader = new DnsMessageReader(new MemoryStream(dataBuffer));

                Header header = reader.ReadHeader();

                if (header.IsQuery && header.AnswerCount == 0)
                {
                    for (int i = 0; i < header.QuestionCount; i++)
                    {
                        Question question = reader.ReadQuestion();
                        Name serviceName = question.QName;

                        // _alljoyn._tcp
                        if (serviceName.Equals("_alljoyn._tcp.local."))
                        {
                            // send answer!
                            return true;
                        }
                    }
                }
                
            }
            catch
            {
                bool validPacket = false;
            }
            return false;
        }

        private static void OnARecord(Name name, IPAddress address, uint ttl)
        {
            Console.WriteLine($"A record: {name} {address} {ttl}");
        }
    }
}
